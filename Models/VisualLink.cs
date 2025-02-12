using ReactiveUI;

namespace YDs_Link_Saver.Models;

public class VisualLink : ReactiveObject
{
    private int _id;
    public int Id { 
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    private string _title;
    public string Title
    {
        get => _title; 
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }
    
    private string _url;
    public string Url
    {
        get => _url;
        set => this.RaiseAndSetIfChanged(ref _url, value);
    }

    private string _description;
    public string? Description
    {
        get => _description; 
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    
    private int _groupId;
    public int GroupId
    {
        get => _groupId; 
        set => this.RaiseAndSetIfChanged(ref _groupId, value);
    }

    private bool _isVisible = true;
    public bool IsVisible
    {
        get => _isVisible; 
        set => this.RaiseAndSetIfChanged(ref _isVisible, value);
    }
    
    public StoredLink ToStoredLink()
    {
        return new StoredLink()
        {
            Id = this.Id,
            Title = this.Title,
            Url = this.Url,
            Description = this.Description,
            GroupId = this.GroupId
        };
    }
}