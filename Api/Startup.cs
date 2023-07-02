namespace Api;

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

public class Startup
{
    public IConfiguration ConfigRoot { get; }

    public Startup(IConfiguration configuration) => this.ConfigRoot = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new DefaultContractResolver())
            .AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddApplication();
        
        services.AddSwaggerGen();

        services.AddDbContext<DatabaseContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString())
        );
    }

    public static void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pezza API V1"));
        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        app.UseAuthorization();
        app.Run();
    }
}