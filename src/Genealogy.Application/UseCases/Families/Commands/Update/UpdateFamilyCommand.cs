using Genealogy.Application.Models;
using Genealogy.Infrastructure.Neo4j.Dtos.Families;
using MediatR;

namespace Genealogy.Application.UseCases.Families.Commands.Update;

public record UpdateFamilyCommand(string PersonId1, string PersonId2, UpdateFamilyDto UpdateFamilyDto)
    : IRequest<Response<bool>>;