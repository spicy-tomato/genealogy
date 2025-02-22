using Genealogy.Application.Models;
using Genealogy.Domain.Enums;
using MediatR;

namespace Genealogy.Application.UseCases.Relationships.Update;

public record UpdateRelationshipCommand(string PersonId1, string PersonId2, RelationshipChangeType ChangeType)
    : IRequest<Response<KeyValuePair<string, string>>>;