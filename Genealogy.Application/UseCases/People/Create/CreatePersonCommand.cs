using Genealogy.Application.Models;
using MediatR;

namespace Genealogy.Application.UseCases.People.Create;

public record CreatePersonCommand(string Name, string BirthDate) : IRequest<Response<Guid>>;