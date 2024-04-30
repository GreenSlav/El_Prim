using System.Diagnostics;
using System.Timers;
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
        
        BEntry.IsEnabled = true;
        BEntry.IsVisible = true;
        //
        BindingContext = mainPageViewModel;
        Title = "El Prim";


        Debug.WriteLine(Environment.CurrentDirectory);

        
        // var timer = new System.Timers.Timer(1000);
        // timer.AutoReset = true;
        // timer.Elapsed += async (sender, e) => await RedrawClock(sender, e);
        // timer.Start();
    }
    
    
    public async Task RedrawClock(object source, ElapsedEventArgs e)
    {
        //var clock = (ClockDrawable) this.ClockGraphicsView.Drawable;
        var graphicsView = this.GraphViewXAML;

        graphicsView.Invalidate(); 
    }
}