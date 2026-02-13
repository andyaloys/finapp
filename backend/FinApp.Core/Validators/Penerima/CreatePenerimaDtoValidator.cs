using FinApp.Core.DTOs.Penerima;
using FluentValidation;

namespace FinApp.Core.Validators.Penerima;

public class CreatePenerimaDtoValidator : AbstractValidator<CreatePenerimaDto>
{
    public CreatePenerimaDtoValidator()
    {
        RuleFor(x => x.Nama)
            .NotEmpty().WithMessage("Nama penerima harus diisi")
            .MaximumLength(200).WithMessage("Nama penerima maksimal 200 karakter");

        RuleFor(x => x.Npwp)
            .MaximumLength(20).WithMessage("NPWP maksimal 20 karakter")
            .When(x => !string.IsNullOrEmpty(x.Npwp));

        RuleFor(x => x.Alamat)
            .MaximumLength(500).WithMessage("Alamat maksimal 500 karakter")
            .When(x => !string.IsNullOrEmpty(x.Alamat));
    }
}
