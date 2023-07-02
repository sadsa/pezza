namespace Test.Setup.TestData;

public static class CustomerTestData
{
    private static readonly Faker Faker = new("en_ZA");

    public static Common.Entities.Customer Customer = new()
    {
        Id = 1,
        Name = Faker.Person.FullName,
        Address = Faker.Address.FullAddress(),
        Cellphone = Faker.Phone.PhoneNumber(),
        Email = Faker.Person.Email,
        DateCreated = DateTime.Now,
    };

    public static CustomerModel CustomerModel = new()
    {
        Id = 1,
        Name = Faker.Person.FullName,
        Address = Faker.Address.FullAddress(),
        Cellphone = Faker.Phone.PhoneNumber(),
        Email = Faker.Person.Email,
        DateCreated = DateTime.Now,
    };
}