using Genealogy.Application.Models;
using Genealogy.Infrastructure.Neo4j.Dtos.People;
using MediatR;

namespace Genealogy.Application.UseCases.People.Queries.GetRelatedByPersonId;

public record GetRelatedByPersonIdQuery(string? RootId, int Depth) : IRequest<Response<GetRelatedPersonResult>>;