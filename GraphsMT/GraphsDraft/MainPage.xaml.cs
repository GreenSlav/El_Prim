using System.Timers;

namespace GraphsDraft;

public partial class MainPage : ContentPage
{
    
    
    public static System.Timers.Timer MinimumSpanningTreeTimer;
    public static System.Timers.Timer FullGraphTimer;

    public MainPage()
    {
        InitializeComponent();
        
        var timerFullGraph = new System.Timers.Timer(2000);
        timerFullGraph.AutoReset = true;
        timerFullGraph.Elapsed += async (sender, e) => await RedrawClock(sender, e);
        timerFullGraph.Start();
        // // можно вот так сделать
        // // timerFullGraph.Stop();
        // // timerFullGraph.Start();
    }
    
    public async Task RedrawClock(object source, ElapsedEventArgs e)
    {
        // var graphicsView = this.GraphViewXAML;
        //
        // graphicsView.Invalidate(); 
    }
}