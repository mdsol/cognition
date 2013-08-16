namespace Cognition.Shared.Configuration
{
    public interface IPermissionTokenProvider
    {
        string GetTokensForViewPublic();
        string GetTokensForViewInternal();
        string GetTokenForEdit();
        string GetTokenForRegistration();
        string GetTokenForAdmin();
    }
}