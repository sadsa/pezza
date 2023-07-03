namespace Core.Pizza.Queries;

public class GetPizzasQuery : IRequest<ListResult<PizzaModel>>
{
    public SearchPizzaModel Data { get; set; }

    public class GetPizzasQueryHandler : IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>
    {
        private readonly DatabaseContext databaseContext;

        public GetPizzasQueryHandler(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<ListResult<PizzaModel>> Handle(GetPizzasQuery request, CancellationToken cancellationToken)
        {
            var entity = request.Data;
            if (string.IsNullOrEmpty(entity.OrderBy))
            {
                entity.OrderBy = "DateCreated desc";
            }

            var entities = databaseContext.Pizzas
                .Select(x => x)
                .AsNoTracking()
                .FilterByName(entity.Name)
                .FilterByDescription(entity.Description)
                .OrderBy(entity.OrderBy);

            var count = entities.Count();
            var paged = await entities.ApplyPaging(entity.PagingArgs).ToListAsync(cancellationToken);

            return ListResult<PizzaModel>.Success(paged.Map(), count);
        }
    }
}