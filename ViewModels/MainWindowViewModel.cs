using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.VisualTree;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using NLog;
using ReactiveUI;
using YDs_Link_Saver.Database;
using YDs_Link_Saver.Models;

namespace YDs_Link_Saver.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<VisualLink> Links { get; } = new();

    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                foreach (var storedLink in Links)
                {
                    storedLink.IsVisible = true;
                }
            }
            else
            {
                foreach (var storedLink in Links)
                {
                    storedLink.IsVisible = storedLink.Title.Contains(value) ||
                                           storedLink.Url.Contains(value) ||
                                           (storedLink.Description?.Contains(value) ?? false);
                }
            }
            this.RaiseAndSetIfChanged(ref _searchQuery, value);
        }
    }

    private ApplicationContext _db = new ApplicationContext();

    private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
    
    public MainWindowViewModel()
    {
        _db.Database.EnsureCreated();
        _db.Links.Load();
        if (_db.Links.Any())
        {
            Links.AddRange(_db.Links.Select(x => x.ToVisualLink()));
        }
    }

    public void StoreLinkCommand(object eGrid)
    {
        Grid root = (Grid)eGrid;
        string? title = root.FindControl<TextBox>("TbTitle").Text;
        string? url = root.FindControl<TextBox>("TbUrl").Text;
        string? desc = root.FindControl<TextBox>("TbDescription").Text;

        VisualLink link = new VisualLink()
        {
            Title = title,
            Url = url,
            Description = desc
        };
        
        _logger.Debug("save to db...");

        Links.Add(link);
        _db.Links.Add(link.ToStoredLink());
        _db.SaveChanges();
        
        _logger.Debug("success!");
    }

    public void RefreshCommand()
    {
        SearchQuery = string.Empty;
        
        Links.Clear();
        if (_db.Links.Any())
        {
            Links.AddRange(_db.Links.Select(x => x.ToVisualLink()));
        }
    }

    public void RemoveLinkCommand(object eLink)
    {
        VisualLink link = (VisualLink)eLink;

        Links.Remove(link);
        StoredLink? del = _db.Links.FirstOrDefault(x => x.Id == link.Id);
        _db.Remove(del);
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