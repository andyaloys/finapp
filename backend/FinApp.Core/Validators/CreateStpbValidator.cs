using FinApp.Core.DTOs.Stpb;
using FluentValidation;

namespace FinApp.Core.Validators;

public class CreateStpbValidator : AbstractValidator<CreateStpbDto>
{
    public CreateStpbValidator()
    {
        RuleFor(x => x.NomorSTPB)
            .MaximumLength(50).WithMessage("Nomor STPB must not exceed 50 characters");

        RuleFor(x => x.Tanggal)
            .NotEmpty().WithMessage("Tanggal is required")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("Tanggal cannot be in the future");

        RuleFor(x => x.NilaiTotal)
            .GreaterThan(0).WithMessage("Nilai total must be greater than 0");

        RuleFor(x => x.Status)
            .Must(status => new[] { "Draft", "Submitted", "Approved", "Rejected" }.Contains(status))
            .WithMessage("Invalid status value");
    }
}
