namespace YDs_Link_Saver.Models;

public class StoredLink
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string? Description { get; set; }
}