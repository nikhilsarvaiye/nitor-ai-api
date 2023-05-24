namespace Services.Validators
{
    using FluentValidation;
    using Models;

    public class SettingValidator : AbstractValidator<Setting>
    {
        public SettingValidator()
        {
            CascadeMode = CascadeMode.Stop;
        }
    }
}
