using System;
using System.Windows;
using System.Windows.Threading;

namespace SPC
{
    public partial class App : Application
    {
        //    public DispatcherTimer _closeTimer { get; set; }

        //    public App()
        //    {
        //        _closeTimer = new DispatcherTimer();
        //        _closeTimer.Interval = TimeSpan.FromSeconds(120); //closes after 2 minuts
        //        _closeTimer.Tick += (s, e) => Current.Shutdown();
        //        _closeTimer.Start();

        //        EventManager.RegisterClassHandler(typeof(UIElement), UIElement.MouseMoveEvent,
        //            new RoutedEventHandler((s, e) => ResetTimer()), true);

        //        EventManager.RegisterClassHandler(typeof(UIElement), UIElement.KeyDownEvent,
        //            new RoutedEventHandler((s, e) => ResetTimer()), true);

        //        EventManager.RegisterClassHandler(typeof(UIElement), UIElement.TouchMoveEvent,
        //            new RoutedEventHandler((s, e) => ResetTimer()), true);
        //    }

        //    private void ResetTimer()
        //    {
        //        _closeTimer.Stop();
        //        _closeTimer.Start();
        //    }
    }
}