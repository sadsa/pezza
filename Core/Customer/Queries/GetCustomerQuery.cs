namespace Core.Customer.Queries;

public class GetCustomerQuery : IRequest<Result<CustomerModel>>
{
    public int Id { get; set; }

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Result<CustomerModel>>
    {
        private readonly DatabaseContext _databaseContext;

        public GetCustomerQueryHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public async Task<Result<CustomerModel>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
            var entity = await query(_databaseContext, request.Id);
            if(entity == null)
            {
                return Result<CustomerModel>.Failure("Not Found");
            }

            return Result<CustomerModel>.Success(entity.Map());
        }
    }
}