﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using D3HeroesTool.Extensions;

namespace D3HeroesTool.Behaviors
{
    /// <summary>
    /// Allows to autosize (and eventually restrict minimum width of a) combobox from XAML
    /// Example:
    /// <![CDATA[<Setter Property="d3htu:ComboBoxWidthFromItemBehavior.ComboBoxWidthFromItems" Value="True" />]]>
    /// </summary>
    public static class ComboBoxWidthFromItemBehavior
    {
        public static readonly DependencyProperty ComboBoxWidthFromItemProperty =
            DependencyProperty.RegisterAttached(
                "ComboBoxWidthFromItems",
                typeof(bool),
                typeof(ComboBoxWidthFromItemBehavior),
                new UIPropertyMetadata(false, OnComboBoxWidthFromItemsPropertyChanged)
            );

        public static readonly DependencyProperty ComboBoxWidthFromItemRestrictMinWidthProperty =
            DependencyProperty.RegisterAttached(
                "ComboBoxRestrictMinWidthFromItems",
                typeof(bool),
                typeof(ComboBoxWidthFromItemBehavior),
                new UIPropertyMetadata(false, OnComboBoxWidthFromItemsPropertyChanged)
            );

        public static bool GetComboBoxWidthFromItems(DependencyObject d)
        {
            return (bool)d.GetValue(ComboBoxWidthFromItemProperty);
        }

        public static void SetComboBoxWidthFromItems(DependencyObject d, bool value)
        {
            d.SetValue(ComboBoxWidthFromItemProperty, value);
        }

        public static bool GetComboBoxRestrictMinWidthFromItems(DependencyObject d)
        {
            return (bool)d.GetValue(ComboBoxWidthFromItemRestrictMinWidthProperty);
        }

        public static void SetComboBoxRestrictMinWidthFromItems(DependencyObject d, bool value)
        {
            d.SetValue(ComboBoxWidthFromItemRestrictMinWidthProperty, value);
        }

        private static void OnComboBoxWidthFromItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboBox cb = d as ComboBox;
            if (cb != null)
            {
                if ((bool)e.NewValue)
                    cb.Loaded += OnComboBoxLoaded;
                else
                    cb.Loaded -= OnComboBoxLoaded;
            }
        }

        private static void OnComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            bool restrictMin = GetComboBoxRestrictMinWidthFromItems(cb);

            Action sizeOnIdle = new Action(() => { cb.SetWidthFromItems(restrictMin); });
            cb.Dispatcher.BeginInvoke(sizeOnIdle, DispatcherPriority.ApplicationIdle);
        }
    }
}
