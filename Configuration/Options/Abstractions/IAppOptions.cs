namespace Configuration.Options
{
    public interface IAppOptions
    {
        string Secret { get; set; }

        bool Cache { get; set; }
    }
}
