﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SolidShineUi.Utils
{
    /// <summary>
    /// Interaction logic for GradientStopItem.xaml
    /// </summary>
    public partial class GradientStopItem : UserControl
    {
        /// <summary>
        /// Create a GradientStopItem, with default property values.
        /// </summary>
        public GradientStopItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create a GradientStopItem, with preset property values.
        /// </summary>
        /// <param name="offset">The offset from the gradient's start point where this stop appears.</param>
        /// <param name="color">The color of this gradient stop.</param>
        public GradientStopItem(double offset, Color color)
        {
            Offset = offset;
            Color = color;
            InitializeComponent();
        }

        /// <summary>
        /// Create a GradientStopItem, generated from a GradientStop.
        /// </summary>
        /// <param name="stop">The GradientStop to load property values from.</param>
        public GradientStopItem(GradientStop stop)
        {
            Offset = stop.Offset;
            Color = stop.Color;
            InitializeComponent();
        }

        public GradientStop GenerateStop()
        {
            return new GradientStop(Color, Offset);
        }

        private void Setup()
        {
            Click += control_Click;
        }

        public double Offset { get => (double)GetValue(OffsetProperty); set => SetValue(OffsetProperty, value); }

        public static DependencyProperty OffsetProperty
            = DependencyProperty.Register("Offset", typeof(double), typeof(GradientStopItem),
            new FrameworkPropertyMetadata(0.0));

        public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

        public static DependencyProperty ColorProperty
            = DependencyProperty.Register("Color", typeof(Color), typeof(GradientStopItem),
            new FrameworkPropertyMetadata(Colors.Black));

        public bool IsSelected { get => (bool)GetValue(IsSelectedProperty); set => SetValue(IsSelectedProperty, value); }

        public static DependencyProperty IsSelectedProperty
            = DependencyProperty.Register("IsSelected", typeof(bool), typeof(GradientStopItem),
            new FrameworkPropertyMetadata(false ));

        private void Highlight()
        {
            pathOutline.Stroke = Colors.DimGray.ToBrush();
        }

        private void Unhighlight()
        {
            pathOutline.Stroke = Colors.Black.ToBrush();
        }

        private void pathOutline_MouseEnter(object sender, MouseEventArgs e)
        {
            Highlight();
        }

        private void pathOutline_MouseLeave(object sender, MouseEventArgs e)
        {
            Unhighlight();
        }

        private void pathOutline_TouchEnter(object sender, TouchEventArgs e)
        {
            Highlight();
        }

        private void pathOutline_TouchLeave(object sender, TouchEventArgs e)
        {
            Unhighlight();
        }

        private void pathOutline_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            brdrFocus.Visibility = Visibility.Visible;
        }

        private void pathOutline_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            brdrFocus.Visibility = Visibility.Collapsed;
        }


        private void control_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Click Handling

        #region Routed Events

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GradientStopItem));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Raised when the check box is clicked.
        /// </summary>
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }
        
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly RoutedEvent RightClickEvent = EventManager.RegisterRoutedEvent(
            "RightClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GradientStopItem));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Raised when the check box is right-clicked.
        /// </summary>
        public event RoutedEventHandler RightClick
        {
            add { AddHandler(RightClickEvent, value); }
            remove { RemoveHandler(RightClickEvent, value); }
        }

        #endregion

        #region Variables/Properties
        bool initiatingClick = false;
        /// <summary>
        /// Gets or sets whether the Click event should be raised when the checkbox is pressed, rather than when it is released.
        /// </summary>
        public bool ClickOnPress { get; set; } = false;

        #endregion

        /// <summary>
        /// Sets up the button to be clicked. This must be run before PerformClick.
        /// </summary>
        /// <param name="rightClick">Determine whether this should be treated as a right click (which usually invokes a context menu).</param>
        void PerformPress(bool rightClick = false)
        {
            initiatingClick = true;

            if (ClickOnPress)
            {
                PerformClick(rightClick);
            }
        }

        /// <summary>
        /// If the button is prepared by PerformPress, perform the Click actions, including raising the Click event.
        /// </summary>
        /// <param name="rightClick">Determine whether this should be treated as a right click (which usually invokes a context menu).</param>
        void PerformClick(bool rightClick = false)
        {
            if (initiatingClick)
            {
                if (rightClick)
                {
                    RoutedEventArgs rre = new RoutedEventArgs(RightClickEvent);
                    RaiseEvent(rre);
                    initiatingClick = false;
                    return;
                }

                RoutedEventArgs re = new RoutedEventArgs(ClickEvent);
                RaiseEvent(re);
                initiatingClick = false;
            }
        }

        /// <summary>
        /// Perform a click programattically. The checkbox responds the same way as if it was clicked by the user.
        /// </summary>
        public void DoClick()
        {
            PerformPress();
            PerformClick();
        }

        private void pathOutline_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PerformPress(e.ChangedButton == MouseButton.Right);
        }

        private void pathOutline_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PerformClick(e.ChangedButton == MouseButton.Right);
            e.Handled = true;
        }

#if NETCOREAPP
        private void pathOutline_TouchDown(object? sender, TouchEventArgs e)
        {
            PerformPress();
        }

        private void pathOutline_TouchUp(object? sender, TouchEventArgs e)
        {
            PerformClick();
        }

#else
        private void pathOutline_TouchDown(object sender, TouchEventArgs e)
        {
            PerformPress();
        }

        private void pathOutline_TouchUp(object sender, TouchEventArgs e)
        {
            PerformClick();
        }
#endif

        private void pathOutline_StylusDown(object sender, StylusDownEventArgs e)
        {
            PerformPress();
        }

        private void pathOutline_StylusUp(object sender, StylusEventArgs e)
        {
            PerformClick();
        }

        private void pathOutline_StylusEnter(object sender, StylusEventArgs e)
        {
            Highlight();
        }

        private void pathOutline_StylusLeave(object sender, StylusEventArgs e)
        {
            Unhighlight();
        }

        private void pathOutline_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                PerformPress();
            }
        }

        private void pathOutline_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                PerformClick();
            }
            else if (e.Key == Key.Apps)
            {
                PerformClick(true);
            }
        }
        #endregion
    }
}