using Genealogy.Application.Models;
using Genealogy.Domain.Models;
using MediatR;

namespace Genealogy.Application.UseCases.People.Queries.GetAll;

public record GetAllPeopleQuery(string? RootId, int Depth) : IRequest<Response<IEnumerable<Person>>>;