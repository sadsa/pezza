namespace Core.Pizza.Queries;

public class GetPizzasQuery : IRequest<ListResult<PizzaModel>>
{
    public class GetPizzasQueryHandler : IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>
    {
        private readonly DatabaseContext _databaseContext;

        public GetPizzasQueryHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public async Task<ListResult<PizzaModel>> Handle(GetPizzasQuery request, CancellationToken cancellationToken)
        {
            var entities = _databaseContext.Pizzas.Select(x => x).AsNoTracking();

            var count = entities.Count();
            var paged = await entities.ToListAsync(cancellationToken);

            return ListResult<PizzaModel>.Success(paged.Map(), count);
        }
    }
}