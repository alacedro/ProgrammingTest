namespace ProgrammingTestInfrastructure.Interfaces
{
    public interface IAvatarService
    {
        Task<string> GetDicebearAvatarUrl(string identifier);

        string GetAvatarUrl(string identifier);
    }
}
