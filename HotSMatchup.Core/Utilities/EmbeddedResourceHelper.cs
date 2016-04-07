using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HotSMatchup.Core
{
    public static class EmbeddedResourceHelper
    {
        public static byte[] GetAssemblyResource(string path)
        {
            using (var str = GetAssemblyResourceStream(path))
            {
                var buf = new byte[str.Length];
                str.Read(buf, 0, buf.Length);
                return buf;
            }
        }

        public static UnmanagedMemoryStream GetAssemblyResourceStream(string path)
        {
            var fullname = GetFullResourceName(path);
            return (UnmanagedMemoryStream)Assembly.GetExecutingAssembly().GetManifestResourceStream(fullname);
        }

        private static string GetFullResourceName(string path) => $"{Assembly.GetExecutingAssembly().GetName().Name}.{path.Replace("/", ".")}";

        //localfilepath has the form Assets/Fonts/xxx.ttf
        public static string GetEmbeddedResourceURI(string localfilepath)
        {
            if (!localfilepath.StartsWith("/"))
                localfilepath = "/" + localfilepath;
            return $"resx://{localfilepath}/";
        }

        public static string GetFileUri(string localpath) => $"file:///{localpath.Replace('\\', '/')}";
    }

    //Adds a new URI scheme so that embedded resources can be used in WPF
    public sealed class ResXWebRequestFactory : IWebRequestCreate
    {
        public const string Scheme = "resx";
        private static ResXWebRequestFactory _factory = new ResXWebRequestFactory();

        private ResXWebRequestFactory()
        {
        }

        // call this before anything else
        public static void Register()
        {
            WebRequest.RegisterPrefix(Scheme, _factory);
        }

        WebRequest IWebRequestCreate.Create(Uri uri)
        {
            return new ResXWebRequest(uri);
        }

        private class ResXWebRequest : WebRequest
        {
            public ResXWebRequest(Uri uri)
            {
                Uri = uri;
            }

            public Uri Uri { get; set; }

            public override WebResponse GetResponse()
            {
                return new ResXWebResponse(Uri);
            }
        }

        private class ResXWebResponse : WebResponse
        {
            public ResXWebResponse(Uri uri)
            {
                Uri = uri;
            }

            public Uri Uri { get; set; }

            public override Stream GetResponseStream()
            {
                int filePos = Uri.LocalPath.LastIndexOf('/');
                string path = Uri.LocalPath.Substring(1, filePos - 1);
                var resourceStream = EmbeddedResourceHelper.GetAssemblyResourceStream(path);
                return resourceStream;
            }
        }
    }
}
