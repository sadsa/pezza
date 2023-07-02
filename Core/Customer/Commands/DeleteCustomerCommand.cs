namespace Core.Customer.Commands;

public class DeleteCustomerCommand : IRequest<Result>
{
    public int Id { get; set; }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly DatabaseContext _databaseContext;

        public DeleteCustomerCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _databaseContext.Customers
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken: cancellationToken);

            if (customer == null)
            {
                return Result.Failure("Error");
            }

            _databaseContext.Customers.Remove(customer);
            var result = await _databaseContext.SaveChangesAsync(cancellationToken);

            return result > 0 ? Result.Success() : Result.Failure("Error");
        }
    }
}