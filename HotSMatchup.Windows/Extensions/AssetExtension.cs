using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace HotSMatchup
{
    [MarkupExtensionReturnType(typeof(object))]
    internal class AssetExtension : MarkupExtension
    {
        private enum ResourceType
        {
            None,
            Xaml,
            Image
        }

        private static Regex pattern = new Regex(
          @"
        (?<Image>(\.png|\.gif|\.jpg|\.bmp|\.ico)$)
        | (?<Font>\#.+$)
        | (?<Xaml>\.xaml$)
      ",
          RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace |
          RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);


        //========================================================================================
        // Constructor
        //========================================================================================

        [ConstructorArgument("path")]
        public string Path { get; set; }

        public AssetExtension()
        {
        }

        public AssetExtension(string path)
        {
            this.Path = path;
        }


        //========================================================================================
        // Methods
        //========================================================================================

        /// <summary>
        /// Returns a fully formatted resource URI derived from the given relative path.
        /// This is used by code-behind to specify full resource paths.
        /// </summary>
        /// <param name="path">A relative path of the resource.</param>
        /// <returns>A pack URI specification.</returns>
        public static Uri GetResourceUri(string path) => new Uri(path);

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(
              typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (target == null)
            {
                return null;
            }

            ResourceType type = ParseResourceType(Path);
            switch (type)
            {
                case ResourceType.Image:
                    BitmapImage image = new BitmapImage(GetResourceUri(Path));
                    return image;
                case ResourceType.Xaml:
                    return GetResourceUri(Path);
            }

            return null;
        }


        private ResourceType ParseResourceType(string path)
        {
            ResourceType type = ResourceType.None;

            var match = pattern.Match(path);

            if (match.Groups["Image"].Success)
            {
                type = ResourceType.Image;
            }
            else if (match.Groups["Xaml"].Success)
            {
                type = ResourceType.Xaml;
            }

            return type;
        }
    }
}
