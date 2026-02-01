using FinApp.Core.DTOs.PpkBendahara;
using FluentValidation;

namespace FinApp.Core.Validators;

public class CreatePpkBendaharaValidator : AbstractValidator<CreatePpkBendaharaDto>
{
    public CreatePpkBendaharaValidator()
    {
        RuleFor(x => x.Nama)
            .NotEmpty().WithMessage("Nama is required")
            .MaximumLength(200).WithMessage("Nama must not exceed 200 characters");

        RuleFor(x => x.NIP)
            .NotEmpty().WithMessage("NIP is required")
            .MaximumLength(20).WithMessage("NIP must not exceed 20 characters")
            .Matches(@"^\d+$").WithMessage("NIP must contain only numbers");

        RuleFor(x => x.Jabatan)
            .IsInEnum().WithMessage("Jabatan must be either 1 (PPK) or 2 (Bendahara)");
    }
}
