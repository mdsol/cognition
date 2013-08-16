using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Cognition.Shared.DynamicTypes;
using Cognition.Shared.Permissions;
using Cognition.Web.Helpers;

namespace Cognition.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDynamicTypeService dynamicTypeService;
        private readonly IDynamicTypeCompiler dynamicTypeCompiler;
        private readonly IPermissionService permissionService;

        public AdminController(IDynamicTypeService dynamicTypeService, IDynamicTypeCompiler dynamicTypeCompiler, IPermissionService permissionService)
        {
            this.dynamicTypeService = dynamicTypeService;
            this.dynamicTypeCompiler = dynamicTypeCompiler;
            this.permissionService = permissionService;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!permissionService.CanUserAdmin())
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public async Task<ActionResult> Types()
        {
            var types = await dynamicTypeService.GetAllAsync();

            return View(types);
        }

        public async Task<ActionResult> CreateType()
        {
            var type = new DynamicTypeDefinition();
            type.Tenant = "default";
            type.Name = "custom";
            type.Code = DefaultDynamicTypeSource.DefaultSource;

            return View(type);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CreateType(DynamicTypeDefinition type)
        {

            if (ModelState.IsValid)
            {
                TryCompilationAndAddErrors(type);
            }
            if (ModelState.IsValid)
            {
                // add the type to the repo
                type.Id = Guid.NewGuid();
                await dynamicTypeService.AddOrUpdateType(type);

                return RedirectToAction("Types");
            }

            return View(type);
        }

        private void TryCompilationAndAddErrors(DynamicTypeDefinition type)
        {
            var compileResult = dynamicTypeCompiler.Compile(type.Code, HttpContext.Server.MapPath("~/bin"));
            if (!compileResult.Success)
            {
                ModelState.AddModelError("Code", "There are " + compileResult.Errors.Count() + " errors with the code.");
                ViewData["CompileErrors"] = compileResult.Errors;
            }
        }

        public async Task<ActionResult> EditType(Guid id)
        {
            var type = await dynamicTypeService.GetTypeById(id);

            return View(type);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> EditType(DynamicTypeDefinition type)
        {
            if (ModelState.IsValid)
            {
                TryCompilationAndAddErrors(type);

            }
            if (ModelState.IsValid)
            {
                // add the type to the repo
                
                await dynamicTypeService.AddOrUpdateType(type);

                return RedirectToAction("Types");
            }

            return View(type);
        }
    }
}