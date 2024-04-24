using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
#endif

namespace GraphsDraft;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        builder.ConfigureLifecycleEvents(events =>
        {
#if WINDOWS
            events.AddWindows(w =>
            {
                w.OnWindowCreated(window =>
                {
                    window.ExtendsContentIntoTitleBar = true; //If you need to completely hide the minimized maximized close button, you need to set this value to false.
                    IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
                    var appWindow = AppWindow.GetFromWindowId(myWndId);
                    appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                });
            });
#endif
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}