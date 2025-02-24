using Genealogy.Application.Models;
using Genealogy.Infrastructure.Dtos.People;
using MediatR;

namespace Genealogy.Application.UseCases.People.Queries.GetRelatedByPersonId;

public record GetRelatedByPersonIdQuery(string? RootId, int Depth) : IRequest<Response<GetRelatedPersonResult>>;