using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Zenref.Ava.ViewModels;
using Zenref.Ava.Views;

namespace Zenref.Ava
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new ExportView
                {
                    DataContext = new ExportViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}