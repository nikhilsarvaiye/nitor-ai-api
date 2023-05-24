namespace Services.Validators
{
    using FluentValidation;
    using Models;

    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Address1).IsAddress();
            RuleFor(x => x.Address2).IsAddress();
            RuleFor(x => x.City);
            RuleFor(x => x.StateId);
            RuleFor(x => x.CountryId);
            RuleFor(x => x.ZipCode).IsZipCode();

            /*
            RuleFor(x => x.Address1).Required().IsAddress();
            RuleFor(x => x.Address2).IsAddress();
            RuleFor(x => x.City).Required();
            RuleFor(x => x.StateId).Required();
            RuleFor(x => x.CountryId).Required();
            RuleFor(x => x.ZipCode).IsZipCode();
             */
        }
    }
}
