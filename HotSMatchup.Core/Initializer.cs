using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HotSMatchup.Core
{
    public static class Initializer
    {
        static bool initialized = false;
        public static void Initialize()
        {
            if (initialized)
                return;
            initialized = true;

            var guid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
            var rando = Guid.NewGuid().ToString();
            Globals.TempFolderForInstance = Path.Combine(Path.GetTempPath(), guid + "hm", rando); 

            ResXWebRequestFactory.Register();
        }

        public static void Shutdown()
        {
            var temproot = Path.Combine(Globals.TempFolderForInstance, "..");

            try { Directory.Delete(temproot, true); }
            catch (Exception) { }
        }
    }
}
