// MIT License
//
// Copyright (c) 2020 Hisham Maudarbocus
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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