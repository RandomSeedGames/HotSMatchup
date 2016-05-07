using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NuzzFramework;

namespace HotSMatchup.Windows
{
    [PrimaryAppClass]
    public class Application : HotSMatchup.Core.Application
    {
        void Start()
        {
            Initialize();
            var app = new WPFApp();
            app.Run(new MainWindow());
        }

        [STAThread]
        static void Main()
        {
            new Application().Start();
        }
    }
}
