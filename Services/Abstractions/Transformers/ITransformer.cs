namespace Services.Transformers
{
    public interface ITransformer<T>
    {
        T Transform(T t);
    }
}
