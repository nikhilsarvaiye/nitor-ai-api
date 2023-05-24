namespace Services.Validators
{
    using FluentValidation;
    using Models;

    public class ProductVariantValidator : AbstractValidator<ProductVariant>
    {
        public ProductVariantValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.ProductId).Required();

            RuleFor(x => x.Name).Required();

            RuleFor(x => x.Type).Required();
        }
    }
}
