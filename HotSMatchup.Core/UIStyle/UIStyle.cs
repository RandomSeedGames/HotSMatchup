using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public UIStyle()
        {
            //Set default colors here
            BackgroundColor = Color.Black;
            MainTextColor = Color.White;
            TextColor1 = Color.FromArgb(100, Color.White);
            TextColor2 = Color.FromArgb(255, 255, 153, 0);
            MapPanelBackgroundBrush = new SolidBrush(Color.FromArgb(99, 72, 213));
            
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
