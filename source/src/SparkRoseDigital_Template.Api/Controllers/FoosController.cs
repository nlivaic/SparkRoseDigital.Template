using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SparkRoseDigital_Template.Api.Models;
using SparkRoseDigital_Template.Application.Foos.Commands;
using SparkRoseDigital_Template.Application.Foos.Queries;

namespace SparkRoseDigital_Template.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class FoosController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly ILogger<FoosController> _logger;

    public FoosController(
        ISender sender,
        IMapper mapper,
        ILogger<FoosController> logger)
    {
        _sender = sender;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Get a single foo.
    /// </summary>
    /// <param name="getFooQuery">Specifies which foo to fetch.</param>
    /// <returns>Foo data.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [HttpGet("{id}", Name = "GetFoo")]
    public async Task<ActionResult<FooGetModel>> GetAsync([FromRoute] GetFooQuery getFooQuery)
    {
        var foo = await _sender.Send(getFooQuery);
        var response = _mapper.Map<FooGetModel>(foo);
        return Ok(response);
    }

    /// <summary>
    /// Create a new foo.
    /// </summary>
    /// <param name="request">Foo create body.</param>
    /// <returns>Newly created foo.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [Produces("application/json")]
    [Consumes("application/json")]
    [HttpPost]
    public async Task<ActionResult<FooGetModel>> PostAsync([FromBody] CreateFooRequest request)
    {
        _logger.LogInformation("Creating foo with text: {Text}", request.Text);
        var command = _mapper.Map<CreateFooCommand>(request);
        var foo = await _sender.Send(command);
        var response = _mapper.Map<FooGetModel>(foo);
        return CreatedAtRoute("GetFoo", new { id = foo.Id }, response);
    }

    /// <summary>
    /// Edit foo.
    /// </summary>
    /// <param name="id">Foo identifier.</param>
    /// <param name="updateFooCommand">Foo edit data.</param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Produces("application/json")]
    [Consumes("application/json")]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync([FromBody] UpdateFooRequest request)
    {
        _logger.LogInformation("Updating foo {id} with text: {Text}", request.Id, request.Text);
        var command = _mapper.Map<UpdateFooCommand>(request);
        await _sender.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Delete foo.
    /// </summary>
    /// <param name="id">Foo identifier.</param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var deleteQuestionCommand = new DeleteFooCommand
        {
            Id = id
        };
        await _sender.Send(deleteQuestionCommand);
        return NoContent();
    }
}
