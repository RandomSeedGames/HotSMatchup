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
using NuzzFramework;
#if WINDOWS
using InnerFont = System.Windows.Media.FontFamily;
#else
using InnerFont = System.Drawing.FontFamily;
#endif

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

        public InnerFont Font1 { get; set; }
        public InnerFont Font2 { get; set; }

        public UIStyle()
        {
            //Set default colors here
            BackgroundColor = Color.Black;
            MainTextColor = Color.White;
            TextColor1 = Color.FromArgb(100, Color.White);
            TextColor2 = Color.FromArgb(255, 255, 153, 0);
            MapPanelBackgroundBrush = new SolidBrush(Color.FromArgb(99, 72, 213));
            Font1 = FontManager.GetFont("ONRAMP");
            Font2 = FontManager.GetFont("Maven Pro 300");
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
