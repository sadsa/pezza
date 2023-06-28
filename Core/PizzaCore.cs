namespace Core;

public class PizzaCore : IPizzaCore
{
    private readonly DatabaseContext _databaseContext;

    public PizzaCore(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<PizzaModel?> GetAsync(int id)
    {
        var entity = await _databaseContext.Pizzas.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null)
        {
            return null;
        }

        return entity.Map();
    }

    public async Task<IEnumerable<PizzaModel>?> GetAllAsync()
    {
        var entities = await _databaseContext.Pizzas.Select(x => x).AsNoTracking().ToListAsync();
        if (entities.Count == 0)
        {
            return null;
        }

        return entities.Map();
    }

    public async Task<PizzaModel?> SaveAsync(PizzaModel pizza)
    {
        var entity = pizza.Map();
        entity.DateCreated = DateTime.UtcNow;
        _databaseContext.Pizzas.Add(entity);
        await _databaseContext.SaveChangesAsync();
        pizza.Id = entity.Id;

        return entity.Map();
    }

    public async Task<PizzaModel?> UpdateAsync(PizzaModel Pizza)
    {
        var findEntity = await _databaseContext.Pizzas.FirstOrDefaultAsync(x => x.Id == Pizza.Id);
        if (findEntity == null)
        {
            return null;
        }

        findEntity.Name = !string.IsNullOrEmpty(Pizza.Name) ? Pizza.Name : findEntity.Name;
        findEntity.Description = !string.IsNullOrEmpty(Pizza.Description) ? Pizza.Description : findEntity.Description;
        findEntity.Price = Pizza.Price ?? findEntity.Price;
        _databaseContext.Pizzas.Update(findEntity);
        await _databaseContext.SaveChangesAsync();

        return findEntity.Map();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _databaseContext.Pizzas
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync();

        return result == 1;
    }
}