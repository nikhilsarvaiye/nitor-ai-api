namespace Services.Validators
{
    using FluentValidation;
    using Models;

    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name).Required();
        }
    }
}
