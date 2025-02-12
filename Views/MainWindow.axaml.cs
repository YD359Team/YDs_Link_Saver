using Avalonia.Controls;
using Avalonia.Input;

namespace YDs_Link_Saver.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        TbTitle.KeyDown += TbTitleOnKeyDown;
        TbUrl.KeyDown += TbUrlOnKeyDown;
        TbDescription.KeyDown += TbDescriptionOnKeyDown;
    }

    private void TbTitleOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            TbUrl.Focus();
    }
    
    private void TbUrlOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            TbDescription.Focus();
    }

    private void TbDescriptionOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            BtnStore.Focus();
    }
}