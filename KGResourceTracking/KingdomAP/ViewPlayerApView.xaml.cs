
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KGResourceTracking.KingdomAP;

/// <summary>
/// Interaction logic for ViewPlayerApView.xaml
/// </summary>
public partial class ViewPlayerApView 
{
    private bool isBindingSet = false;
    public ViewPlayerApView()
    {
        InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is ViewPlayerApViewModel viewModel)
        {
            await viewModel.RefreshCommand.ExecuteAsync(null);
            if (!isBindingSet)
            {
                Binding binding = new Binding("IsAllPicked");
                binding.Source = this.DataContext;
                headerCheckbox.SetBinding(CheckBox.IsCheckedProperty, binding);
                isBindingSet = true;
            }
        }
    }
}
