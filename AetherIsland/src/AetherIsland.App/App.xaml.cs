using System;
using System.Threading;
using System.Windows;

namespace AetherIsland;

public partial class App : Application
{
    private Mutex? _mutex;
    private bool _ownsMutex;

    protected override void OnStartup(StartupEventArgs e)
    {
        _mutex = new Mutex(true, @"Local\AetherIsland.Desktop", out _ownsMutex);
        if (!_ownsMutex)
        {
            MessageBox.Show("Aether Island is already running.", "Aether Island");
            Shutdown();
            return;
        }

        base.OnStartup(e);
        MainWindow = new MainWindow();
        MainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (_ownsMutex)
        {
            try { _mutex?.ReleaseMutex(); }
            catch (ApplicationException) { }
        }
        _mutex?.Dispose();
        base.OnExit(e);
    }
}
