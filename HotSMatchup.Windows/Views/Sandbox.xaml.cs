using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NuzzFramework.Windows;

namespace HotSMatchup.Controls
{
    /// <summary>
    /// Interaction logic for Sandbox.xaml
    /// </summary>
    public partial class Sandbox : ControlBase
    {
        public readonly DependencyProperty xOffsetProperty;

        public Sandbox()
        {
            xOffsetProperty =
        DependencyProperty.RegisterAttached(
            "xOffset",
            typeof(double),
            typeof(Sandbox), // Change this line
            new FrameworkPropertyMetadata(
                0, new PropertyChangedCallback(OnxOffsetPropertyChanged)));
        }

        void OnxOffsetPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
        }

        public int xOffset
        {
            get { return 100; }
        }
        public int yOffset
        {
            get { return 50; }
        }

    }
}
