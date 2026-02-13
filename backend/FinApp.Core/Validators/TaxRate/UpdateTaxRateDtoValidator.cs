using FinApp.Core.DTOs.TaxRate;
using FluentValidation;

namespace FinApp.Core.Validators.TaxRate;

public class UpdateTaxRateDtoValidator : AbstractValidator<UpdateTaxRateDto>
{
    public UpdateTaxRateDtoValidator()
    {
        RuleFor(x => x.TaxName)
            .NotEmpty().WithMessage("Nama pajak tidak boleh kosong")
            .MaximumLength(100).WithMessage("Nama pajak maksimal 100 karakter");

        RuleFor(x => x.Rate)
            .NotEmpty().WithMessage("Tarif tidak boleh kosong")
            .GreaterThan(0).WithMessage("Tarif harus lebih besar dari 0")
            .LessThanOrEqualTo(100).WithMessage("Tarif tidak boleh lebih dari 100%");
    }
}
