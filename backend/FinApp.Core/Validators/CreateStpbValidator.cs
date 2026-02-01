using FinApp.Core.DTOs.Stpb;
using FluentValidation;

namespace FinApp.Core.Validators;

public class CreateStpbValidator : AbstractValidator<CreateStpbDto>
{
    public CreateStpbValidator()
    {
        RuleFor(x => x.NomorSTPB)
            .MaximumLength(50).WithMessage("Nomor STPB must not exceed 50 characters");

        RuleFor(x => x.TanggalSTPB)
            .NotEmpty().WithMessage("Tanggal is required")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("Tanggal cannot be in the future");

        RuleFor(x => x.PpkBendaharaId)
            .NotEmpty().WithMessage("PPK/Bendahara is required");

        RuleFor(x => x.Tahun)
            .GreaterThan(2000).WithMessage("Tahun must be greater than 2000")
            .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage("Tahun cannot be more than next year");

        // Details optional - bisa ditambah setelah STPB dibuat
    }
}
