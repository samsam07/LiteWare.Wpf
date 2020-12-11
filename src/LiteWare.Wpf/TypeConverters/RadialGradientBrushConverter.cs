using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace LiteWare.Wpf.TypeConverters
{
    /// <summary>
    /// Converts instances of other types to and from <see cref="RadialGradientBrush"/> instances.
    /// </summary>
    public sealed class RadialGradientBrushConverter : TypeConverter
    {
        private const string RadialGradientParsingPattern = "^ *RadialGradient +((?<centerPoint>\\d+\\.?\\d*,\\d+\\.?\\d*) +(?<gradientOrigin>\\d+\\.?\\d*,\\d+\\.?\\d*)? *)?((?<radiusX>\\d+\\.?\\d*) +(?<radiusY>\\d+\\.?\\d*))? *(?<stops>.*)$";

        /// <summary>
        /// Constructs a <see cref="RadialGradientBrush"/> from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">A string representation of a <see cref="RadialGradientBrush"/>.</param>
        /// <returns>The equivalent <see cref="RadialGradientBrush"/>.</returns>
        /// <remarks>Format: RadialGradient [&lt;center&gt; [&lt;gradient_origin&gt;]] [&lt;radius_x&gt; &lt;radius_y&gt;] (&lt;stop_color&gt;","&lt;stop_offset&gt; ){2..n}</remarks>
        public static RadialGradientBrush Parse(string source)
        {
            Match bodyMatch = Regex.Match(source, RadialGradientParsingPattern, RegexOptions.IgnoreCase);
            if (!bodyMatch.Success)
            {
                throw new ArgumentException("Radial gradient literal is not valid.");
            }

            GradientStopCollection gradientStopCollection;
            if (bodyMatch.Groups.ContainsKey("stops"))
            {
                gradientStopCollection = GradientStopCollection.Parse(bodyMatch.Groups["stops"].Value);
            }
            else
            {
                throw new ArgumentException("Radial gradient literal does not define any gradient stops.");
            }

            RadialGradientBrush radialGradientBrush = new RadialGradientBrush(gradientStopCollection);

            if (bodyMatch.Groups.ContainsKey("center"))
            {
                radialGradientBrush.Center = Point.Parse(bodyMatch.Groups["center"].Value);
            }

            if (bodyMatch.Groups.ContainsKey("gradient_origin"))
            {
                radialGradientBrush.GradientOrigin = Point.Parse(bodyMatch.Groups["gradient_origin"].Value);
            }

            if (bodyMatch.Groups.ContainsKey("radius_x"))
            {
                double radiusX = Convert.ToDouble(bodyMatch.Groups["radius_x"].Value);
                radialGradientBrush.RadiusX = radiusX;
            }

            if (bodyMatch.Groups.ContainsKey("radius_y"))
            {
                double radiusY = Convert.ToDouble(bodyMatch.Groups["radius_y"].Value);
                radialGradientBrush.RadiusY = radiusY;
            }

            return radialGradientBrush;
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to an instance of <see cref="RadialGradientBrush"/>, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
        /// <returns><see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            (sourceType == typeof(RadialGradientBrush) || sourceType == typeof(string));

        /// <summary>
        /// Attempts to convert a specified object to an instance of <see cref="RadialGradientBrush"/>.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>The instance of <see cref="RadialGradientBrush"/> that is created from the converted source.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            object convertedObj = value switch
            {
                RadialGradientBrush radialGradientBrush => radialGradientBrush,
                string radialGradientLiteral => Parse(radialGradientLiteral),
                _ => throw new ArgumentException($"{value} object is not a valid type that can be converted to {nameof(RadialGradientBrush)}.")
            };

            return convertedObj;
        }

        /// <summary>Returns whether this converter can convert an instance of <see cref="RadialGradientBrush"/> to the specified type, using the specified context.</summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to.</param>
        /// <returns><see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            (destinationType == typeof(RadialGradientBrush) || destinationType == typeof(string));

        /// <summary>
        /// Attempts to convert an instance of <see cref="RadialGradientBrush"/> to a specified type.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If <see langword="null" /> is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
        /// <returns>The object that is created from the converted instance of <see cref="RadialGradientBrush"/>.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value is RadialGradientBrush radialGradientBrush)
            {
                if (destinationType == typeof(RadialGradientBrush))
                {
                    return radialGradientBrush;
                }

                if (destinationType == typeof(string))
                {
                    return $"LinearGradient {radialGradientBrush.Center} {radialGradientBrush.GradientOrigin} {radialGradientBrush.RadiusX} {radialGradientBrush.RadiusY} {radialGradientBrush.GradientStops}";
                }
            }

            throw new ArgumentException($"{nameof(value)} is not of the '{nameof(RadialGradientBrush)}' type.");
        }
    }
}