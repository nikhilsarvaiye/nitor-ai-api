namespace Models
{
    public interface IView<T>
        where T: IViews
    {
        T Properties { get; set; }
    }
}
