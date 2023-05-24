namespace Services.Validators
{
    using FluentValidation;
    using Models;

    public class PatientValidator : AbstractValidator<Patient>
    {
        public PatientValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name).Required();
        }
    }
}
