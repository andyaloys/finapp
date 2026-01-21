using FluentValidation;
using FinApp.Core.DTOs.User;

namespace FinApp.Core.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username harus diisi")
                .MinimumLength(3).WithMessage("Username minimal 3 karakter");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password harus diisi")
                .MinimumLength(6).WithMessage("Password minimal 6 karakter");

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
