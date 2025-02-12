using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
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
    public ObservableCollection<VisualGroup> Groups { get; } = new();

    private VisualGroup _selectedGroup;

    public VisualGroup SelectedGroup
    {
        get => _selectedGroup;
        set => this.RaiseAndSetIfChanged(ref _selectedGroup, value);
    }

    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                foreach (var gp in Groups)
                {
                    foreach (var link in gp.SubNodes)
                    {
                        link.IsVisible = true;
                    }
                }
            }
            else
            {
                foreach (var gp in Groups)
                {
                    foreach (var link in gp.SubNodes)
                    {
                        link.IsVisible = link.Title.Contains(value) ||
                                         link.Url.Contains(value) ||
                                         (link.Description?.Contains(value) ?? false);
                    }
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
        _db.Groups.Load();
        _db.Links.Load();

        RefreshGroups();
    }

    private void RefreshGroups()
    {
        Groups.Clear();
        
        List<VisualLink> visualLinks = new();
        List<VisualGroup> visualGroups = new();

        VisualGroup emptyGp = new VisualGroup() { Id = 0, Title = "Без группы" };
        visualGroups.Add(emptyGp);
        if (_db.Groups.Any())
        {
            visualGroups.AddRange(_db.Groups.Select(x => x.ToVisualGroup()));
        }
        if (_db.Links.Any())
        {
            visualLinks.AddRange(_db.Links.Select(x => x.ToVisualLink()));
        }

        if (visualLinks.Count > 0)
        {
            foreach (var gp in visualGroups)
            {
                gp.SubNodes.AddRange(visualLinks.Where(x => x.GroupId == gp.Id));
            }
        }

        Groups.AddRange(visualGroups);
        SelectedGroup = emptyGp;
    }

    public void StoreLinkCommand(object eGrid)
    {
        Grid root = (Grid)eGrid;
        string? title = root.FindControl<TextBox>("TbTitle").Text;
        string? url = root.FindControl<TextBox>("TbUrl").Text;
        string? desc = root.FindControl<TextBox>("TbDescription").Text;
        VisualGroup? group = (VisualGroup)root.FindControl<ComboBox>("CbGroup").SelectedItem;

        VisualLink link = new VisualLink()
        {
            Title = title,
            Url = url,
            Description = desc,
            GroupId = group.Id
        };
        
        _logger.Debug("save to db...");

        group.SubNodes.Add(link);
        _db.Links.Add(link.ToStoredLink());
        _db.SaveChanges();
        
        _logger.Debug("success!");
    }
    
    public void CreateGroupCommand(object eGrid)
    {
        Grid root = (Grid)eGrid;
        string? title = root.FindControl<TextBox>("TbGroupName").Text;

        VisualGroup group = new VisualGroup()
        {
            Title = title
        };
        
        _logger.Debug("save to db...");

        Groups.Add(group);
        _db.Groups.Add(group.ToStoredGroup());
        _db.SaveChanges();
        
        _logger.Debug("success!");
    }

    public void RefreshCommand()
    {
        SearchQuery = string.Empty;
        
        _logger.Debug("refreshing...");

        RefreshGroups();
        
        _logger.Debug("success!");
    }

    public void RemoveLinkCommand(object eLink)
    {
        VisualLink link = (VisualLink)eLink;

        _logger.Debug("removing...");
        
        Groups.SingleOrDefault(x => x.Id == link.Id).SubNodes.Remove(link);
        StoredLink? del = _db.Links.FirstOrDefault(x => x.Id == link.GroupId);
        _db.Remove(del);
        _db.SaveChanges();
        
        _logger.Debug("success!");
    }

    public void NavigateCommand(object eLink)
    {
        VisualLink link = (VisualLink)eLink;

        try
        {
            Process proc = new Process();
            proc.StartInfo.FileName = link.Url;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
        }
    }
}