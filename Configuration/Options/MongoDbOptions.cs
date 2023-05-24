namespace Configuration.Options
{
    public class MongoDbOptions : IDbOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string SchemaName { get; set; }
    }
}
