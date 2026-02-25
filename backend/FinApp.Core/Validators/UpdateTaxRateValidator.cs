using FinApp.Core.DTOs.TaxRate;
using FluentValidation;

namespace FinApp.Core.Validators;

public class UpdateTaxRateValidator : AbstractValidator<UpdateTaxRateDto>
{
    public UpdateTaxRateValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Kategori harus diisi")
            .MaximumLength(100).WithMessage("Kategori maksimal 100 karakter");

        RuleFor(x => x.Rate)
            .GreaterThan(0).WithMessage("Rate harus lebih dari 0")
            .LessThanOrEqualTo(100).WithMessage("Rate maksimal 100%");

        RuleFor(x => x.DisplayOrder)
            .GreaterThan(0).WithMessage("Display order harus lebih dari 0");
    }
}
