namespace Genealogy.Application.UseCases.People.Queries.GetAll;

public class GetAllPeopleRequest
{
    public string? RootId { get; set; }
    public int Depth { get; set; }
}