namespace Blinds.Data.RepoFactory
{
    public interface IRepoFactory
    {
        T Get<T>() where T : class;
    }
}
