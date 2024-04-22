//using System.Windows.Forms;

using System.Diagnostics;
using Graphs.ViewModels;

namespace Graphs;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    // OnLoad method
    protected override void OnStart()
    {
        base.OnStart();
        
        Debug.WriteLine("OnLoad method worked!!!!");
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        // разрешение окна
        // const int newWidth = 1920;
        // const int newHeight = 1080;
        // перед этим нужно не забыть добавить reference к C:\WINDOWS\Microsoft.NET\Framework\v4...\System.Windows.Forms.dll
        // и скачать через нагетсы этот пакет исключений: Microsoft.Win32.SystemEvents
        int newWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        int newHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        
        // где должно появляться окно
        window.X = 0;
        window.Y = 0;
        
        window.Width = newWidth;
        window.Height = newHeight;

        return window;
    }
}