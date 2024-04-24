using System.Windows.Forms;
using Graphs.ViewModels;

namespace Graphs;

public partial class MainPage : ContentPage
{
    public static GraphicsView GraphView; 
    
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        InitializeComponent();

        // Передал из XAML ссылку на GraphicsView, в котором и будут отрисовываться вершины
        GraphView = GraphViewXAML;
        GraphView.HeightRequest = Screen.PrimaryScreen.Bounds.Height;
        GraphView.WidthRequest = Screen.PrimaryScreen.Bounds.Width;
        GraphView.BackgroundColor = Colors.Gray;

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