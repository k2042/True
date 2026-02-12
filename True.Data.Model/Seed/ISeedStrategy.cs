namespace True.Data.Model.Seed
{
    public interface ISeedStrategy
    {
        Task SeedData(CancellationToken cancellationToken);
    }
}
