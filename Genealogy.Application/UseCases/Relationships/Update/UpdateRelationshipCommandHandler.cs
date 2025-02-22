using Genealogy.Application.Models;
using Genealogy.Application.Services.Abstractions;
using Genealogy.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.Relationships.Update;

// public class UpdateRelationshipCommandHandler(IRelationshipRepository relationshipRepository,
//     IRequestValidator<UpdateRelationshipCommand> validator)
//     : IRequestHandler<UpdateRelationshipCommand, Response<KeyValuePair<string, string>>>
// {
//     public async Task<Response<KeyValuePair<string, string>>> Handle(UpdateRelationshipCommand request,
//         CancellationToken cancellationToken)
//     {
//         await validator.Validate(request);
//
//         Tuple<string, string, string> result = await relationshipRepository.Change(request.PersonId1, request.PersonId2,
//             request.ChangeType);
//
//         return result != null
//             ? Response.Succeed(result.Value)
//             : Response.Error<KeyValuePair<string, string>>("Cannot connect people");
//     }
// }