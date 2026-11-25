using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using AetherIsland.Services;

namespace AetherIsland;

public partial class MainWindow : Window
{
    private readonly SettingsService _settings = new();
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };
    private DateTime? _focusEnd;
    private bool _expanded;

    public MainWindow()
    {
        InitializeComponent();
        _settings.Load();
        _timer.Tick += (_, _) => RefreshClock();
        _timer.Start();
        Loaded += (_, _) =>
        {
            var area = SystemParameters.WorkArea;
            Left = area.Left + (area.Width - Width) / 2;
            Top = area.Top + 12;
        };
        Closed += (_, _) => _timer.Stop();
        RefreshClock();
    }

    private void RefreshClock()
    {
        var now = DateTime.Now;
        ClockText.Text = now.ToString("HH:mm", CultureInfo.CurrentCulture);
        DateText.Text = now.ToString("ddd, dd MMM", CultureInfo.CurrentCulture).ToUpperInvariant();

        if (_focusEnd is null) return;
        var remaining = _focusEnd.Value - now;
        if (remaining <= TimeSpan.Zero)
        {
            _focusEnd = null;
            Countdown.Text = "DONE";
            FocusButton.Content = "Start 25 min";
            StatusText.Text = "FOCUS COMPLETE";
            return;
        }
        Countdown.Text = $"{(int)remaining.TotalMinutes:00}:{remaining.Seconds:00}";
    }

    private void Toggle_Click(object sender, RoutedEventArgs e)
    {
        _expanded = !_expanded;
        Details.Visibility = _expanded ? Visibility.Visible : Visibility.Collapsed;
        Height = _expanded ? 272 : 118;
        ((Button)sender).Content = _expanded ? "⌃" : "⌄";
    }

    private void Focus_Click(object sender, RoutedEventArgs e)
    {
        if (_focusEnd is null)
        {
            _focusEnd = DateTime.Now.AddMinutes(25);
            FocusButton.Content = "End session";
            StatusText.Text = "FOCUS MODE ON";
            RefreshClock();
        }
        else
        {
            _focusEnd = null;
            Countdown.Text = "25:00";
            FocusButton.Content = "Start 25 min";
            StatusText.Text = "READY";
        }
    }

    private void Downloads_Click(object sender, RoutedEventArgs e)
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        try
        {
            Directory.CreateDirectory(path);
            Process.Start(new ProcessStartInfo("explorer.exe", $"\"{path}\"") { UseShellExecute = true });
            StatusText.Text = "DOWNLOADS OPENED";
        }
        catch (Exception ex) { ShowError("Could not open Downloads.", ex); }
    }

    private void Notepad_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo("notepad.exe") { UseShellExecute = true });
            StatusText.Text = "NOTEPAD OPENED";
        }
        catch (Exception ex) { ShowError("Could not start Notepad.", ex); }
    }

    private void Settings_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show($"Settings file location:\n{_settings.FilePath}\n\nA full settings window is planned next.",
            "Aether Island", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();

    private void Surface_Drag(object sender, MouseButtonEventArgs e)
    {
        DependencyObject? current = e.OriginalSource as DependencyObject;
        while (current is not null)
        {
            if (current is ButtonBase) return;
            current = VisualTreeHelper.GetParent(current);
        }
        if (e.ButtonState == MouseButtonState.Pressed)
        {
            try { DragMove(); }
            catch (InvalidOperationException) { }
        }
    }

    private void ShowError(string message, Exception ex)
    {
        StatusText.Text = "ACTION FAILED";
        MessageBox.Show($"{message}\n{ex.Message}", "Aether Island",
            MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
