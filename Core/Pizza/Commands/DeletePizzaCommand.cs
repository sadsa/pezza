namespace Core.Pizza.Commands;

public class DeletePizzaCommand : IRequest<Result>
{
    public int Id { get; set; }

    public class DeletePizzaCommandHandler : IRequestHandler<DeletePizzaCommand, Result>
    {
        private readonly DatabaseContext databaseContext;

        public DeletePizzaCommandHandler(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<Result> Handle(DeletePizzaCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await databaseContext.Pizzas.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);
            if (findEntity == null)
            {
                return Result.Failure("Not found");
            }

            databaseContext.Pizzas.Remove(findEntity);
            var result = await databaseContext.SaveChangesAsync(cancellationToken);

            return result > 0 ? Result.Success() : Result.Failure("Error");
        }
    }
}