using System.ComponentModel.DataAnnotations.Schema;

namespace YDs_Link_Saver.Models;

public class StoredLink
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string? Description { get; set; }
    public int GroupId { get; set; }

    public VisualLink ToVisualLink()
    {
        return new VisualLink()
        {
            Id = this.Id,
            Title = this.Title,
            Url = this.Url,
            Description = this.Description,
            GroupId = this.GroupId
        };
    }
}