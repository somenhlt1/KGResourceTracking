using System.Windows.Input;

using KGResourceTracking.KingdomAP;

namespace KGResourceTracking;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow(ViewPlayerApViewModel viewPlayerApViewModel, AddNewApViewModel addNewApViewModel)
    {
        //DataContext = viewModel;
        InitializeComponent();

        AddAp.DataContext = addNewApViewModel;
        ViewAp.DataContext = viewPlayerApViewModel;

        CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, OnClose));
    }

    private void OnClose(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }
}
