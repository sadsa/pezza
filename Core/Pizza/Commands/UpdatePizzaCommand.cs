namespace Core.Pizza.Commands;

public class UpdatePizzaCommand : IRequest<Result<PizzaModel>>
{
    public int? Id { get; set; }

    public UpdatePizzaModel? Data { get; set; }

    public class UpdatePizzaCommandHandler : IRequestHandler<UpdatePizzaCommand, Result<PizzaModel>>
    {
        private readonly DatabaseContext _databaseContext;

        public UpdatePizzaCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public async Task<Result<PizzaModel>> Handle(UpdatePizzaCommand request, CancellationToken cancellationToken)
        {
            if (request.Data == null || request.Id == null)
            {
                return Result<PizzaModel>.Failure("Error");
            }

            var model = request.Data;
            var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
            var findEntity = await query(_databaseContext, request.Id.Value);
            if (findEntity == null)
            {
                return Result<PizzaModel>.Failure("Not found");
            }

            findEntity.Name = !string.IsNullOrEmpty(model?.Name) ? model?.Name : findEntity.Name;
            findEntity.Description = !string.IsNullOrEmpty(model?.Description) ? model?.Description : findEntity.Description;
            findEntity.Price = model.Price.HasValue ? model.Price.Value : findEntity.Price;

            var outcome = _databaseContext.Pizzas.Update(findEntity);
            var result = await _databaseContext.SaveChangesAsync(cancellationToken);

            return result > 0 ? Result<PizzaModel>.Success(findEntity.Map()) : Result<PizzaModel>.Failure("Error");
        }
    }
}