namespace Cognition.Shared.Permissions
{
    public interface IPermissionService
    {
        bool CurrentUserCanViewPublic();
        bool CurrentUserCanViewInternal();
        bool CurrentUserCanEdit();
        bool CanUserRegister(string userId);
        bool CanUserAdmin();
    }
}