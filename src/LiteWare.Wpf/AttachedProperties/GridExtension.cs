using System;
using System.Windows;
using System.Windows.Controls;

namespace LiteWare.Wpf.AttachedProperties
{
    /// <summary>
    /// Class that provides attached property shorthands for <see cref="Grid.RowDefinitions"/> and <see cref="Grid.ColumnDefinitions"/>.
    /// </summary>
    public class GridExtension
    {
        /// <summary>
        /// Identifies the RowDefinitions attached property.
        /// </summary>
        public static readonly DependencyProperty RowDefinitionsProperty = DependencyProperty.RegisterAttached
        (
            "RowDefinitions",
            typeof(string),
            typeof(GridExtension),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender, RowDefinitionsPropertyChangedCallback)
        );

        /// <summary>
        /// Gets the value of the RowDefinitions attached property from a given <see cref="System.Windows.Controls.Grid"/>.
        /// </summary>
        /// <param name="grid">The <see cref="System.Windows.Controls.Grid"/> from which to read the property value.</param>
        /// <returns>The value of the RowDefinitions attached property.</returns>
        public static string GetRowDefinitions(Grid grid) => (string)grid.GetValue(RowDefinitionsProperty);

        /// <summary>
        /// Sets the value of the RowDefinitions attached property to a given <see cref="System.Windows.Controls.Grid"/>.
        /// </summary>
        /// <param name="grid">The <see cref="System.Windows.Controls.Grid"/> on which to set the RowDefinitions attached property.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetRowDefinitions(Grid grid, string value) => grid.SetValue(RowDefinitionsProperty, value);

        private static void RowDefinitionsPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Grid grid && e.NewValue is string newValue)
            {
                GridLengthConverter gridLengthConverter = new GridLengthConverter();
                grid.RowDefinitions.Clear();

                string[] gridLengthLiterals = newValue.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (string gridLengthLiteral in gridLengthLiterals)
                {
                    if (gridLengthConverter.ConvertFrom(gridLengthLiteral) is GridLength gridLength)
                    {
                        RowDefinition rowDefinition = new RowDefinition { Height = gridLength };
                        grid.RowDefinitions.Add(rowDefinition);
                    }
                }
            }
        }

        /// <summary>
        /// Identifies the ColumnDefinitions attached property.
        /// </summary>
        public static readonly DependencyProperty ColumnDefinitionsProperty = DependencyProperty.RegisterAttached
        (
            "ColumnDefinitions",
            typeof(string),
            typeof(GridExtension),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender, ColumnDefinitionsPropertyChangedCallback)
        );

        /// <summary>
        /// Gets the value of the ColumnDefinitions attached property from a given <see cref="System.Windows.Controls.Grid"/>.
        /// </summary>
        /// <param name="grid">The <see cref="System.Windows.Controls.Grid"/> from which to read the property value.</param>
        /// <returns>The value of the ColumnDefinitions attached property.</returns>
        public static string GetColumnDefinitions(Grid grid) => (string)grid.GetValue(ColumnDefinitionsProperty);

        /// <summary>
        /// Sets the value of the ColumnDefinitions attached property to a given <see cref="System.Windows.Controls.Grid"/>.
        /// </summary>
        /// <param name="grid">The <see cref="System.Windows.Controls.Grid"/> on which to set the ColumnDefinitions attached property.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetColumnDefinitions(Grid grid, string value) => grid.SetValue(ColumnDefinitionsProperty, value);

        private static void ColumnDefinitionsPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Grid grid && e.NewValue is string newValue)
            {
                GridLengthConverter gridLengthConverter = new GridLengthConverter();
                grid.ColumnDefinitions.Clear();

                string[] gridLengthLiterals = newValue.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (string gridLengthLiteral in gridLengthLiterals)
                {
                    if (gridLengthConverter.ConvertFrom(gridLengthLiteral) is GridLength gridLength)
                    {
                        ColumnDefinition columnDefinition = new ColumnDefinition { Width = gridLength };
                        grid.ColumnDefinitions.Add(columnDefinition);
                    }
                }
            }
        }
    }
}