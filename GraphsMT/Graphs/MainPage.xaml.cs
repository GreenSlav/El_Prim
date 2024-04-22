using Graphs.ViewModels;

namespace Graphs;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        InitializeComponent();

        // BorderButton.IsVisible = false;
        // BorderButton.IsEnabled = false;
        //
        BorderEntry.IsEnabled = true;
        BorderEntry.IsVisible = true;
        //
        BindingContext = mainPageViewModel;
        Title = "El Prim";
    }
}