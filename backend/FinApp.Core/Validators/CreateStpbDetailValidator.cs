using FinApp.Core.DTOs.Stpb;
using FluentValidation;

namespace FinApp.Core.Validators;

public class CreateStpbDetailValidator : AbstractValidator<CreateStpbDetailDto>
{
    public CreateStpbDetailValidator()
    {
        RuleFor(x => x.KodeSuboutput)
            .NotEmpty().WithMessage("KodeSuboutput is required");

        RuleFor(x => x.Volume)
            .GreaterThan(0).WithMessage("Volume must be greater than 0");

        RuleFor(x => x.Satuan)
            .NotEmpty().WithMessage("Satuan is required")
            .MaximumLength(50).WithMessage("Satuan must not exceed 50 characters");

        RuleFor(x => x.HargaSatuan)
            .GreaterThan(0).WithMessage("HargaSatuan must be greater than 0");
    }
}
