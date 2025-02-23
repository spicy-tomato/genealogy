using Genealogy.Application.Models;
using MediatR;

namespace Genealogy.Application.UseCases.People.Commands.Delete;

public record DeletePersonCommand(string Id) : IRequest<Response<bool>>;