namespace Core.Customer.Commands;

public class DeleteCustomerCommand : IRequest<Result>
{
    public int Id { get; set; }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly DatabaseContext databaseContext;

        public DeleteCustomerCommandHandler(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await databaseContext.Customers
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken: cancellationToken);

            if (customer == null)
            {
                return Result.Failure("Error");
            }

            databaseContext.Customers.Remove(customer);
            var result = await databaseContext.SaveChangesAsync(cancellationToken);

            return result > 0 ? Result.Success() : Result.Failure("Error");
        }
    }
}