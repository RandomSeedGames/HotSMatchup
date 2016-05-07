using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotSMatchup.Core
{
    public class Application : NuzzFramework.Application
    {
        public override void Initialize()
        {
            NuzzFramework.Configuration.AssetAssembly = typeof(HotSMatchup.Core.Application).Assembly;
            base.Initialize();
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}
