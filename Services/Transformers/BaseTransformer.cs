namespace Services.Transformers
{
    public abstract class BaseTransformer<T> : ITransformer<T>
    {
        public abstract T Transform(T t);
    }
}
