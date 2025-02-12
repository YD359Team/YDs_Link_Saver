namespace YDs_Link_Saver.Models;

public class StoredGroup
{
    public int Id { get; set; }
    public string Title { get; set; }

    public VisualGroup ToVisualGroup()
    {
        return new VisualGroup()
        {
            Id = this.Id,
            Title = this.Title
        };
    }
}