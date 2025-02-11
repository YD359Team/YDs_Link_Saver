using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.VisualTree;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using NLog;
using YDs_Link_Saver.Database;
using YDs_Link_Saver.Models;

namespace YDs_Link_Saver.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<StoredLink> Links { get; } = new();

    private ApplicationContext _db = new ApplicationContext();

    private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
    
    public MainWindowViewModel()
    {
        _db.Database.EnsureCreated();
        _db.Links.Load();
        if (_db.Links.Any())
        {
            Links.AddRange(_db.Links);
        }
    }

    public void StoreLinkCommand(object eGrid)
    {
        Grid root = (Grid)eGrid;
        string? title = root.FindControl<TextBox>("Title").Text;
        string? url = root.FindControl<TextBox>("Url").Text;
        string? desc = root.FindControl<TextBox>("Description").Text;

        StoredLink link = new StoredLink()
        {
            Title = title,
            Url = url,
            Description = desc
        };
        
        _logger.Debug("save to db...");

        Links.Add(link);
        _db.Links.Add(link);
        _db.SaveChanges();
        
        _logger.Debug("success!");
    }
}