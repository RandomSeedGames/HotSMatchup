using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HotSMatchup.Core
{
    /// <summary>
    /// Configuration for UI colors
    /// </summary>
    public class UIStyle : IDisposable
    {
        public static readonly UIStyle Default = new UIStyle();
        public static readonly UIStyle Current = new UIStyle();

        public Color BackgroundColor { get; set; }
        public Color MainTextColor { get; set; }
        public Color TextColor1 { get; set; }
        public Color TextColor2 { get; set; }

        private Brush _MapPanelBackgroundBrush;
        public Brush MapPanelBackgroundBrush
        {
            get
            {
                return _MapPanelBackgroundBrush;
            }
            set
            {
                if (_MapPanelBackgroundBrush != null)
                    _MapPanelBackgroundBrush.Dispose();
                _MapPanelBackgroundBrush = value;
            }
        }

        PrivateFontCollection Fonts { get; } = new PrivateFontCollection();
        FontFamily Font1 { get; set; }

        public UIStyle()
        {
            //Set default colors here
            BackgroundColor = Color.Black;
            MainTextColor = Color.White;
            TextColor1 = Color.FromArgb(100, Color.White);
            TextColor2 = Color.FromArgb(255, 255, 153, 0);
            MapPanelBackgroundBrush = new SolidBrush(Color.FromArgb(99, 72, 213));
            Font1 = LoadFont("ONRAMP");
         //   System.Drawing.Font fnt = new Font(Font1, 10f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point);
            Font1 = Font1;
        }

        //Loads a font from assembly resources
        FontFamily LoadFont(string name)
        {
            var bytes = GetResource($"{Assembly.GetExecutingAssembly().GetName().Name}.Assets.Fonts.{name}.ttf");
            IntPtr ptr = Marshal.AllocCoTaskMem(bytes.Length);
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
            Fonts.AddMemoryFont(ptr, bytes.Length);
            //Fonts.AddFontFile(@"C:\dev\Projects\Blizzard\HotSMatchup\HotSMatchup.Core\Assets\Fonts\ONRAMP.ttf")
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(ptr);
            return Fonts.Families.Last();
            
        }

        byte[] GetResource(string name)
        {
            using (var str = Assembly.GetExecutingAssembly().GetManifestResourceStream(name) as UnmanagedMemoryStream)
            {
                var buf = new byte[str.Length];
                str.Read(buf, 0, buf.Length);
                return buf;
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls
        public virtual void Dispose()
        {
            if (!disposedValue)
            {
                disposedValue = true;
                MapPanelBackgroundBrush.Dispose();
            }
        }
        #endregion
    }
}
