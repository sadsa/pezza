namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    private readonly IPizzaCore _pizzaCore;

    public PizzaController(IPizzaCore pizzaCore)
    {
        _pizzaCore = pizzaCore;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> Get(int id)
    {
        var search = await _pizzaCore.GetAsync(id);

        return (search == null) ? NotFound() : Ok(search);
    }

    [HttpPost("Search")]
    [ProducesResponseType(200)]
    public async Task<ActionResult> Search()
        => Ok(await _pizzaCore.GetAllAsync());

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Pizza>> Create([FromBody] PizzaModel model)
    {
        var result = await _pizzaCore.SaveAsync(model);

        return (result == null) ? BadRequest() : Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> Update([FromBody] PizzaModel model)
    {
        var result = await _pizzaCore.UpdateAsync(model);

        return (result == null) ? BadRequest() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _pizzaCore.DeleteAsync(id);

        return (!result) ? BadRequest() : Ok(result);
    }
}