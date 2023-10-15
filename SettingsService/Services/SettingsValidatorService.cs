namespace SettingsService.Services
{
    using FluentValidation;
    using SettingsService.Data.Models;

    public class SettingsValidatorService : AbstractValidator<Setting>
    {
        public SettingsValidatorService()
        {
            RuleFor(setting => setting.Key).NotEmpty().NotNull();
            RuleFor(setting => setting.Value).NotEmpty().NotNull();
        }
    }
}
