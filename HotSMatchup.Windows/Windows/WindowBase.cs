using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HotSMatchup.Core;
using HotSMatchup.Controls;
using System.Windows.Controls;

namespace HotSMatchup
{
public class WindowBase : Window
{
    //UI Configuration class containing System.Drawing.Color 
    //and Brush properties (platform-agnostic styling)
    public UIStyle UIStyle => Core.UIStyle.Current;

    //IValueConverter that converts System.Drawing.Color properties into WPF-equivalent Colors and Brushes 
    public static UniversalValueConverter UniversalValueConverter { get; } = new UniversalValueConverter();

    public WindowBase()
    {
        //Add window name to namespace so that runtime properties can be referenced from XAML
        //(Name setting must be done here and not in xaml because this is a base class)
        //You probably won't need to, but working example is here in case you do.
        var ns = new NameScope();
        NameScope.SetNameScope(this, ns);
        ns["window"] = this;

        //Call Initialize Component via Reflection, so you do not need 
        //to call InitializeComponent() every time in your base class
        this.GetType()
            .GetMethod("InitializeComponent", 
                System.Reflection.BindingFlags.Public | 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance)
            .Invoke(this, null);

        //Set runtime DataContext - Designer mode will not run this code
        this.DataContext = this;
    }

    //Stub class here so that the design-time instantiator has a method to call
    void InitializeComponent() { }
}
}
