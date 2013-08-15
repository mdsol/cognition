using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Cognition.Shared.DynamicTypes;

namespace Cognition.Web.Controllers
{
    public class AdminController : AsyncController
    {
        private readonly IDynamicTypeService dynamicTypeService;

        public AdminController(IDynamicTypeService dynamicTypeService)
        {
            this.dynamicTypeService = dynamicTypeService;
        }

        public async Task<ActionResult> Types()
        {
            var types = await dynamicTypeService.GetAll();

            return View(types);
        }

        public async Task<ActionResult> CreateType()
        {
            var type = new DynamicTypeDefinition();

            return View(type);
        }

        public async Task<ActionResult> EditType(Guid id)
        {
            var type = await dynamicTypeService.GetTypeById(id);

            return View(type);
        }
    }
}