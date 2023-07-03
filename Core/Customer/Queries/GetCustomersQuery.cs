namespace Core.Customer.Queries;

public class GetCustomersQuery : IRequest<ListResult<CustomerModel>>
{
    public SearchCustomerModel Data { get; set; }
    
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, ListResult<CustomerModel>>
    {
        private readonly DatabaseContext _databaseContext;

        public GetCustomersQueryHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ListResult<CustomerModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var entity = request.Data;
            if (string.IsNullOrEmpty(entity.OrderBy))
            {
                entity.OrderBy = "DateCreated desc";
            }
            var entities = _databaseContext.Customers
                .Select(x => x)
                .AsNoTracking()
                .FilterByName(entity.Name)
                .FilterByAddress(entity.Address)
                .FilterByPhone(entity.Cellphone)
                .FilterByEmail(entity.Email)
                .OrderBy(entity.OrderBy);

            var count = entities.Count();
            var paged = await entities.ApplyPaging(entity.PagingArgs).ToListAsync(cancellationToken);

            return ListResult<CustomerModel>.Success(paged.Map(), count);
        }
    }
}