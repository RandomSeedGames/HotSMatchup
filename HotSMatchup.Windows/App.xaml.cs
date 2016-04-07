using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HotSMatchup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Core.Initializer.Initialize();
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(TryFindResource(typeof(Window))));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Core.Initializer.Shutdown();
            base.OnExit(e);
        }

        public static bool InDesignMode => !(Application.Current is App);
    }
}
