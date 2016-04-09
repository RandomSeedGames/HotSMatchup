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

namespace HotSMatchup
{
    /// <summary>
    /// Interaction logic for CharacterPicker.xaml
    /// </summary>
    public partial class CharacterPicker : UserControl
    {
        //IValueConverter that converts System.Drawing.Color properties into WPF-equivalent Colors and Brushes 
        public static AddValueConverter AddValueConverter { get; } = new AddValueConverter();

        public CharacterPicker()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //Get source code of CalcBinding
         //   BindingExpression be = imageHexBorder.GetBindingExpression(Canvas.LeftProperty);
          //      string format = be.ParentBinding.StringFormat;
            
        //    var obj = be.Target;
        //    var prop = be.TargetProperty;
       //     var result = obj.GetValue(prop);

            //Create a crippled copy of CBinding and then see which line of code breaks the resolution
            base.OnRender(drawingContext);
        }
    }
}
