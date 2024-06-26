﻿using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using Graphs.ViewModels;

namespace Graphs;

public partial class MainPage : ContentPage
{
    public static GraphicsView GraphView; 
    public static System.Timers.Timer TimerToRedraw = new System.Timers.Timer(1000)
    {
        AutoReset = true
    };
    
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        InitializeComponent();
        
        // Передал из XAML ссылку на GraphicsView, в котором и будут отрисовываться вершины
        GraphView = GraphViewXAML;
        GraphView.HeightRequest = Screen.PrimaryScreen.Bounds.Height;
        GraphView.WidthRequest = Screen.PrimaryScreen.Bounds.Width;
        
        BEntry.IsEnabled = true;
        BEntry.IsVisible = true;
        
        BindingContext = mainPageViewModel;
        Title = "El Prim";


        Debug.WriteLine(Environment.CurrentDirectory);
    }
    

    public static void ReDrawGraph()
    {
        var graphicsView = GraphView;

        graphicsView.Invalidate(); 
    }
}