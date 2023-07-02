namespace Test.Setup.TestData;

public static class PizzaTestData
{
    private static readonly Faker Faker = new();

    private static readonly List<string> Pizzas = new()
    {
        "Veggie Pizza",
        "Pepperoni Pizza",
        "Meat Pizza",
        "Margherita Pizza",
        "BBQ Chicken Pizza",
        "Hawaiian Pizza"
    };
    
    public static Pizza Pizza = new()
    {
        Id = 1,
        Name = Faker.PickRandom(Pizzas),
        Description = string.Empty,
        Price = Faker.Finance.Amount(),
        DateCreated = DateTime.Now,
    };

    public static readonly PizzaModel PizzaModel = new()
    {
        Id = 1,
        Name = Faker.PickRandom(Pizzas),
        Description = string.Empty,
        Price = Faker.Finance.Amount(),
        DateCreated = DateTime.Now
    };

}