namespace True.Data.Model.Seed
{
    public interface ISeedDataProvider<TSeedEntity>
        where TSeedEntity : class
    {
        Task<IReadOnlyCollection<TSeedEntity>> LoadSeedData(CancellationToken cancellationToken);
    }
}
