using FluentValidation;
using FinApp.Core.DTOs.User;

namespace FinApp.Core.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Password minimal 6 karakter")
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Nama lengkap harus diisi");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email harus diisi")
                .EmailAddress().WithMessage("Format email tidak valid");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role harus diisi")
                .Must(role => role == "Admin" || role == "User")
                .WithMessage("Role harus Admin atau User");
        }
    }
}
