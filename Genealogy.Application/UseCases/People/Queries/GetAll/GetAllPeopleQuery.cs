using Genealogy.Application.Models;
using Genealogy.Domain.Models;
using MediatR;

namespace Genealogy.Application.UseCases.People.Queries.GetAll;

public record GetAllPeopleQuery : IRequest<Response<IEnumerable<Person>>>;