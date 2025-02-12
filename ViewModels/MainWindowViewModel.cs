using System.Collections.ObjectModel;
using System.Diagnostics;
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
        string? title = root.FindControl<TextBox>("TbTitle").Text;
        string? url = root.FindControl<TextBox>("TbUrl").Text;
        string? desc = root.FindControl<TextBox>("TbDescription").Text;

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

    public void RemoveLinkCommand(object eLink)
    {
        StoredLink link = (StoredLink)eLink;

        Links.Remove(link);
        _db.Links.Remove(link);
        _db.SaveChanges();
    }

    public void NavigateCommand(object eLink)
    {
        StoredLink link = (StoredLink)eLink;

        Process proc = new Process();
        proc.StartInfo.FileName = link.Url;
        proc.StartInfo.UseShellExecute = true;
        proc.Start();
    }
}