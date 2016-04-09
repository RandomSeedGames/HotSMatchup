using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotSMatchup
{
    public class Binding : System.Windows.Data.Binding
    {
        public Binding()
            : base()
        {
            this.Converter = WindowBase.UniversalValueConverter;
        }

        public Binding(string path)
            : base(path)
        {
            this.Converter = WindowBase.UniversalValueConverter;
        }
    }
}
