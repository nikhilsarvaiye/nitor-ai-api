namespace Configuration.Options
{
    public class AppOptions : IAppOptions
    {
        public string Secret { get; set; }
        
        public bool Cache { get; set; }
    }
}
