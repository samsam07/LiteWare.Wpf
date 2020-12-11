using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace LiteWare.Wpf.TypeConverters
{
    /// <summary>
    /// Converts instances of other types to and from <see cref="LinearGradientBrush"/> instances.
    /// </summary>
    public sealed class LinearGradientBrushConverter : TypeConverter
    {
        private const string LinearGradientParsingPattern = "^ *LinearGradient +(((?<startPoint>\\d+\\.?\\d*,\\d+\\.?\\d*) +(?<endPoint>\\d+\\.?\\d*,\\d+\\.?\\d*)?)|((?<angle>\\d+\\.?\\d*)(deg)?)) +(?<stops>.*)$";

        /// <summary>
        /// Constructs a <see cref="LinearGradientBrush"/> from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">A string representation of a <see cref="LinearGradientBrush"/>.</param>
        /// <returns>The equivalent <see cref="LinearGradientBrush"/>.</returns>
        /// <remarks>Format: LinearGradient (&lt;start_point&gt; [&lt;end_point&gt;])|(&lt;angle&gt;["deg"]) (&lt;stop_color&gt;","&lt;stop_offset&gt; ){2..n}</remarks>
        public static LinearGradientBrush Parse(string source)
        {
            Match bodyMatch = Regex.Match(source, LinearGradientParsingPattern, RegexOptions.IgnoreCase);
            if (!bodyMatch.Success)
            {
                throw new ArgumentException("Linear gradient literal is not valid.");
            }

            GradientStopCollection gradientStopCollection;
            if (bodyMatch.Groups.ContainsKey("stops"))
            {
                gradientStopCollection = GradientStopCollection.Parse(bodyMatch.Groups["stops"].Value);
            }
            else
            {
                throw new ArgumentException("Linear gradient literal does not define any gradient stops.");
            }

            LinearGradientBrush linearGradientBrush;
            if (bodyMatch.Groups.ContainsKey("angle"))
            {
                double angle = Convert.ToDouble(bodyMatch.Groups["angle"].Value);
                linearGradientBrush = new LinearGradientBrush(gradientStopCollection, angle);
            }
            else
            {
                Point startPoint = Point.Parse(bodyMatch.Groups["start_point"].Value);

                Point endPoint = new Point(1, 1);
                if (bodyMatch.Groups.ContainsKey("end_point"))
                {
                    endPoint = Point.Parse(bodyMatch.Groups["end_point"].Value);
                }

                linearGradientBrush = new LinearGradientBrush(gradientStopCollection, startPoint, endPoint);
            }

            return linearGradientBrush;
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to an instance of <see cref="LinearGradientBrush"/>, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
        /// <returns><see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            (sourceType == typeof(LinearGradientBrush) || sourceType == typeof(string));

        /// <summary>
        /// Attempts to convert a specified object to an instance of <see cref="LinearGradientBrush"/>.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>The instance of <see cref="LinearGradientBrush"/> that is created from the converted source.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            object convertedObj = value switch
            {
                LinearGradientBrush linearGradientBrush => linearGradientBrush,
                string linearGradientLiteral => Parse(linearGradientLiteral),
                _ => throw new ArgumentException($"{value} object is not a valid type that can be converted to {nameof(LinearGradientBrush)}.")
            };

            return convertedObj;
        }

        /// <summary>Returns whether this converter can convert an instance of <see cref="LinearGradientBrush"/> to the specified type, using the specified context.</summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to.</param>
        /// <returns><see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            (destinationType == typeof(LinearGradientBrush) || destinationType == typeof(string));

        /// <summary>
        /// Attempts to convert an instance of <see cref="LinearGradientBrush"/> to a specified type.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If <see langword="null" /> is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
        /// <returns>The object that is created from the converted instance of <see cref="LinearGradientBrush"/>.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value is LinearGradientBrush linearGradientBrush)
            {
                if (destinationType == typeof(LinearGradientBrush))
                {
                    return linearGradientBrush;
                }

                if (destinationType == typeof(string))
                {
                    return $"LinearGradient {linearGradientBrush.StartPoint} {linearGradientBrush.EndPoint} {linearGradientBrush.GradientStops}";
                }
            }

            throw new ArgumentException($"{nameof(value)} is not of the '{nameof(LinearGradientBrush)}' type.");
        }
    }
}