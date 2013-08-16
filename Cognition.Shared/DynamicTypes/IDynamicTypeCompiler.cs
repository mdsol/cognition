namespace Cognition.Shared.DynamicTypes
{
    public interface IDynamicTypeCompiler
    {
        DynamicTypeCompileResult Compile(string code, string binPath);
    }
}