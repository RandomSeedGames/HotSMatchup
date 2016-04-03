using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;

namespace HotSMatchup.Controls
{
    public class UniversalValueConverter : IValueConverter
    {
        internal static readonly List<Type> ValidTargetTypes = new List<Type>{
                typeof(System.Windows.Media.Brush),
                typeof(System.Windows.Media.Color)
            };
        private static Dictionary<Type, TypeConverterBase> ConverterMap = new Dictionary<Type, TypeConverterBase>();

        static UniversalValueConverter()
        {
            MapTypeConverters();
        }

        private static void MapTypeConverters()
        {
            //Get a list of our type converters, and map target types to them
            var ourTypeConverters = typeof(UniversalValueConverter).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(TypeConverterBase)))
                .ToList();
            foreach (var converterType in ourTypeConverters)
            {
                //Get TypeConverterTo attributes and map our dictionary accordingly
                var targetTypes = converterType.GetCustomAttributes(typeof(TypeConverterToAttribute), false)
                    .ToList()
                    .Select(x => ((TypeConverterToAttribute)x).TargetType)
                    .ToList();
                var converterInstance = (TypeConverterBase)Activator.CreateInstance(converterType);
                foreach (var type in targetTypes)
                {
                    if (ConverterMap.ContainsKey(type))
                        throw new InvalidOperationException("You cannot have multiple type converters for a single target type. Check your TypeConverterToAttribute declarations for duplicates.");
                    ConverterMap[type] = converterInstance;
                }
            }
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            TypeConverter converter = null;
            if (ConverterMap.ContainsKey(targetType))
                converter = ConverterMap[targetType];
            else
                converter = TypeDescriptor.GetConverter(targetType);

            try
            {
                // determine if the supplied value is of a suitable type
                if (converter.CanConvertFrom(value.GetType()) && converter.CanConvertTo(targetType))
                {
                    // return the converted value
                    return converter.ConvertTo(value, targetType);
                }
                else
                {
                    // try to convert from the string representation
                    return converter.ConvertFrom(value.ToString());
                }
            }
            catch (Exception ex)
            {
                return value;
            }
        }

        public object ConvertBack
        (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private class TypeConverterBase : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                //This check is not necessary
                return true;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                //This check is not necessary
                return true;
            }
        }

        [TypeConverterTo(typeof(System.Windows.Media.Brush))]
        [TypeConverterTo(typeof(System.Windows.Media.Color))]
        private class BrushConverter : TypeConverterBase
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value == null)
                    return null;

                var originalType = value.GetType();

                if (destinationType == value.GetType())
                    return value;

                if (value is System.Drawing.SolidBrush)
                {
                    System.Drawing.SolidBrush brush = (SolidBrush)value;
                    value = brush.Color;
                    if (destinationType == value.GetType())
                        return value;
                }
                if (value is System.Windows.Media.SolidColorBrush)
                {
                    System.Windows.Media.SolidColorBrush brush = (System.Windows.Media.SolidColorBrush)value;
                    value = brush.Color;
                    if (destinationType == value.GetType())
                        return value;
                }
                if (value is System.Drawing.Color)
                {
                    Color color = (Color)value;
                    value = new System.Windows.Media.Color() { A = color.A, R = color.R, G = color.G, B = color.B };
                    if (destinationType == value.GetType())
                        return value;
                }
                if (value is System.Windows.Media.Color)
                {
                    if (destinationType == value.GetType())
                        return value;
                }
                return new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)value);
                
                throw new InvalidOperationException($"Unsupported source type [{originalType.Name}]");
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return null;
            }
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class TypeConverterToAttribute : Attribute
        {
            public Type TargetType { get; }
            public TypeConverterToAttribute(Type targetType)
            {
                TargetType = targetType;
            }
        }
    }
}
