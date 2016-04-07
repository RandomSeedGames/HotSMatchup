using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS
using InnerFont = System.Windows.Media.FontFamily;
#else
using InnerFont = System.Drawing.FontFamily;
#endif

namespace HotSMatchup.Core
{
    /// <summary>
    /// Platform-Agnostic Font Manager
    /// </summary>
    public static class FontManager
    {
        static string FontsPath { get { return Path.Combine(Globals.TempFolderForInstance, "fonts"); } }
        static Dictionary<string, FontFamily> Cache { get; } = new Dictionary<string, FontFamily>();

        static FontManager()
        {
            LoadResourceFont("ONRAMP", "ONRAMP.ttf");
            LoadResourceFont("Maven Pro 300", "MavenProLight-300.otf");
        }

        public static InnerFont GetFont(string name)
        {
            if (Cache.ContainsKey(name))
                return Cache[name].Font;
            return null;
        }

        //Loads a font from assembly resources
        static void LoadResourceFont(string name, string filename)
        {
            InnerFont innerfont;
            FontFamily outerfont;
            var fullpath = $"Assets/Fonts/{filename}";
#if WINDOWS
            //Write temp file from resource
            var bytes = EmbeddedResourceHelper.GetAssemblyResource(fullpath);
            var tempfile = Path.Combine(FontsPath, filename);
            if (!Directory.Exists(FontsPath))
                Directory.CreateDirectory(FontsPath);
            System.IO.File.WriteAllBytes(tempfile, bytes);

            //Create font
            var uri = EmbeddedResourceHelper.GetFileUri(FontsPath) + $"/#{name}";
            innerfont = new System.Windows.Media.FontFamily(uri);
            outerfont = new FontFamily(innerfont);
#else
            var bytes = EmbeddedResourceHelper.GetAssemblyResource(fullpath);
            innerfont = LoadDrawingFontFamily(bytes);
            outerfont = new FontFamily(innerfont);
#endif
            Cache[name] = outerfont;
        }

        static System.Drawing.FontFamily LoadDrawingFontFamily(byte[] bytes)
        {
            using (var fonts = new System.Drawing.Text.PrivateFontCollection())
            {
                IntPtr ptr = Marshal.AllocCoTaskMem(bytes.Length);
                Marshal.Copy(bytes, 0, ptr, bytes.Length);
                fonts.AddMemoryFont(ptr, bytes.Length);
                System.Runtime.InteropServices.Marshal.FreeCoTaskMem(ptr);
                return fonts.Families.Last();
            }
        }
    }
    /// <summary>
    /// Wrapper surrounding platform-specific font type
    /// </summary>
    public class FontFamily
    {
        public InnerFont Font { get; private set; }

        public FontFamily(InnerFont font)
        {
            this.Font = font;
        }
    }
}
