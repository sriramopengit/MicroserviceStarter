using FluentValidation;
using InventoryService.Models;

public class InventoryItemValidator : AbstractValidator<InventoryItem>
{
    public InventoryItemValidator()
    {
        RuleFor(x => x.ProductName).NotEmpty().WithMessage("Name is required.").MaximumLength(100);
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity must be non-negative.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}
