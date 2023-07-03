namespace Core.Pizza.Commands;

public class DeletePizzaCommandValidator : AbstractValidator<DeletePizzaCommand>
{
    public DeletePizzaCommandValidator()
    {
        this.RuleFor(r => r.Id)
            .NotEmpty();
    }
}