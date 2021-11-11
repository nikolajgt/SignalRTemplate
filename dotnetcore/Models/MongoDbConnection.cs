namespace dotnetcore.Models
{
    public class MongoDbConnection : IMongoDbConnection
    {
        public string CollectionName { get; set; } = "";
        public string ConnectionString { get; set; } = "";
        public string DatabaseName { get; set; } = "";
    }

    public interface IMongoDbConnection
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
