using System.Collections.ObjectModel;
using ReactiveUI;

namespace YDs_Link_Saver.Models;

public class VisualGroup : ReactiveObject
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

    public ObservableCollection<VisualLink> SubNodes  { get; set; } = new();
    
    public StoredGroup ToStoredGroup()
    {
        return new StoredGroup()
        {
            Id = this.Id,
            Title = this.Title
        };
    }

    public override string ToString()
    {
        return Title;
    }
}