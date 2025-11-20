using FarmXpert.Application.Todo.Commands.CreateTodo;
using FarmXpert.Application.Todo.Commands.DeleteTodo;
using FarmXpert.Application.Todo.Commands.UpdateTodo;
using FarmXpert.Application.Todo.Queries.GetAllTodos;
using FarmXpert.Application.Todo.Queries.GetTodoById;
using FarmXpert.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Todo> todos = await _mediator.Send(new GetAllTodosQuery());
        return Ok(todos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var todo = await _mediator.Send(new GetTodoByIdQuery(id));
        if (todo == null)
        {
            return NotFound();
        }
        return Ok(todo);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoCommand command)
    {
        try
        {
            var todo = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(UpdateTodoCommand command)
    {
        try
        {
            var updatedTodo = await _mediator.Send(command);
            if (updatedTodo == null)
            {
                return BadRequest("Failed to modify salary.");
            }

            return Ok(updatedTodo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var todo = await _mediator.Send(new GetTodoByIdQuery(id));
        if (todo == null)
        {
            return NotFound();
        }

        await _mediator.Send(new DeleteTodoCommand(id));
        return Ok(todo);
    }
}
