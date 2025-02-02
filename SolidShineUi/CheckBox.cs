﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SolidShineUi
{
    /// <summary>
    /// A CheckBox control with more customization over the appearance, and a larger box for a more touch-friendly UI.
    /// </summary>
    [DefaultEvent(nameof(CheckChanged))]
    [Localizability(LocalizationCategory.CheckBox)]
    public class CheckBox : ContentControl
    {
        static CheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckBox), new FrameworkPropertyMetadata(typeof(CheckBox)));
        }

        /// <summary>
        /// Create a new CheckBox control.
        /// </summary>
        public CheckBox()
        {
            //InitializeComponent();
            Padding = new Thickness(5, 0, 0, 0);

            InternalIsCheckedChanged += CheckBox_InternalIsCheckedChanged;
            InternalIsIndeterminateChanged += CheckBox_InternalIsIndeterminateChanged;

            CommandBindings.Add(new CommandBinding(CheckBoxClickCommand, OnCheckBoxClick));

            SetValue(BackgroundProperty, ColorsHelper.CreateFromHex("01FFFFFF").ToBrush());

            //if (Template != null)
            //{
            //    try
            //    {
            //        ((Border)Template.FindName("brdrChk", this)).MouseEnter += Border_MouseEnter;
            //        ((Border)Template.FindName("brdrChk", this)).MouseLeave += Border_MouseLeave;
            //    }
            //    catch (NullReferenceException)
            //    {

            //    }
            //}

            //Focusable = true;
            KeyboardNavigation.SetIsTabStop(this, true);

            MouseDown += UserControl_MouseDown;
            MouseUp += UserControl_MouseUp;
            //MouseEnter += UserControl_MouseEnter;
            //MouseLeave += UserControl_MouseLeave;
            TouchDown += UserControl_TouchDown;
            TouchUp += UserControl_TouchUp;
            StylusDown += UserControl_StylusDown;
            StylusUp += UserControl_StylusUp;

            //GotFocus += UserControl_GotFocus;
            //GotKeyboardFocus += UserControl_GotKeyboardFocus;
            //LostFocus += UserControl_LostFocus;
            //LostKeyboardFocus += UserControl_LostKeyboardFocus;

            KeyDown += UserControl_KeyDown;
            KeyUp += UserControl_KeyUp;
        }

        //private void Border_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (Debugger.IsAttached) Debugger.Log(0, "CheckBox", "Border leave");
        //}

        //private void Border_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    if (Debugger.IsAttached) Debugger.Log(0, "CheckBox", "Border enter");
        //}

        /// <summary>
        /// The command that activates when the box of the checkbox itself has been clicked.
        /// </summary>
        public static readonly RoutedCommand CheckBoxClickCommand = new RoutedCommand();


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly RoutedEvent CheckBoxClickEvent = EventManager.RegisterRoutedEvent(
            "CheckBoxClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckBox));

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// An event that raises only when the checkbox itself is clicked.
        /// </summary>
        public event RoutedEventHandler CheckBoxClick
        {
            add { AddHandler(CheckBoxClickEvent, value); }
            remove { RemoveHandler(CheckBoxClickEvent, value); }
        }

        bool checkBoxClick = false;

        private void OnCheckBoxClick(object sender, ExecutedRoutedEventArgs e)
        {
            checkBoxClick = true;
            RoutedEventArgs re = new RoutedEventArgs(CheckBoxClickEvent);
            RaiseEvent(re);
            DoClick();
            checkBoxClick = false;
        }

        #region IsChecked/IsIndeterminate/CheckState & Events

        //private byte sel = 0;

        #region Routed Events
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly RoutedEvent CheckChangedEvent = EventManager.RegisterRoutedEvent(
            "CheckChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckBox));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Raised when the CheckState property is changed, either to Checked, Indeterminate, or Unchecked.
        /// </summary>
        public event RoutedEventHandler CheckChanged
        {
            add { AddHandler(CheckChangedEvent, value); }
            remove { RemoveHandler(CheckChangedEvent, value); }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly RoutedEvent CheckedEvent = EventManager.RegisterRoutedEvent(
            "Checked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckBox));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Raised when the CheckState property is changed to Checked.
        /// </summary>
        public event RoutedEventHandler Checked
        {
            add { AddHandler(CheckedEvent, value); }
            remove { RemoveHandler(CheckedEvent, value); }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly RoutedEvent UncheckedEvent = EventManager.RegisterRoutedEvent(
            "Unchecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckBox));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Raised when the CheckState property is changed to Unchecked.
        /// </summary>
        public event RoutedEventHandler Unchecked
        {
            add { AddHandler(UncheckedEvent, value); }
            remove { RemoveHandler(UncheckedEvent, value); }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly RoutedEvent IndeterminateEvent = EventManager.RegisterRoutedEvent(
            "Indeterminate", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckBox));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Raised when the CheckState property is changed to Indeterminate.
        /// </summary>
        public event RoutedEventHandler Indeterminate
        {
            add { AddHandler(IndeterminateEvent, value); }
            remove { RemoveHandler(IndeterminateEvent, value); }
        }
        #endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
            "IsChecked", typeof(bool), typeof(CheckBox),
            new PropertyMetadata(false, new PropertyChangedCallback(OnInternalIsCheckedChanged)));

        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
            "IsIndeterminate", typeof(bool), typeof(CheckBox),
            new PropertyMetadata(false, new PropertyChangedCallback(OnInternalIsIndeterminateChanged)));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Internal event for handling a property changed. Please view the event that is not prefixed as "Internal".
        /// </summary>
        protected event DependencyPropertyChangedEventHandler InternalIsCheckedChanged;

        /// <summary>
        /// Internal event for handling a property changed. Please view the event that is not prefixed as "Internal".
        /// </summary>
        protected event DependencyPropertyChangedEventHandler InternalIsIndeterminateChanged;

        /// <summary>
        /// Get or set if the check box is checked. (Note: if in the Indeterminate state, it will still return true as checked.)
        /// </summary>
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        /// <summary>
        /// Get or set if the check box is in the Indeterminate state. 
        /// </summary>
        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

        private CheckState state;

        /// <summary>
        /// Get or set the state of the checkbox, via a CheckState enum. Can be set via this property or via the IsChecked and IsIndeterminate properties.
        /// </summary>
        public CheckState CheckState
        {
            get { return state; }
            set
            {
                state = value;
                if (updateBoolValues)
                {
                    switch (state)
                    {
                        case CheckState.Unchecked:
                            IsChecked = false;
                            IsIndeterminate = false;
                            break;
                        case CheckState.Checked:
                            IsChecked = true;
                            IsIndeterminate = false;
                            break;
                        case CheckState.Indeterminate:
                            IsChecked = true;
                            IsIndeterminate = true;
                            break;
                        default:
                            IsChecked = false;
                            IsIndeterminate = false;
                            break;
                    }
                }
            }
        }

        bool raiseChangedEvent = true;
        bool updateBoolValues = true;

        private static void OnInternalIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBox c)
            {
                c.InternalIsCheckedChanged?.Invoke(c, e);
            }
        }

        private static void OnInternalIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBox c)
            {
                c.InternalIsIndeterminateChanged?.Invoke(c, e);
            }
        }

        private void CheckBox_InternalIsCheckedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // code to run whenever the IsChecked value is changed
            //if (raiseChangedEvent)
            //{
            //    RoutedEventArgs t = new RoutedEventArgs(CheckChangedEvent);
            //    RaiseEvent(t);
            //}

            if (!IsChecked)
            {
                raiseChangedEvent = false;
                updateBoolValues = false;
                IsIndeterminate = false;
                CheckState = CheckState.Unchecked;
                raiseChangedEvent = true;
                updateBoolValues = true;
            }
            else
            {
                if (IsIndeterminate)
                {
                    updateBoolValues = false;
                    CheckState = CheckState.Indeterminate;
                    updateBoolValues = true;
                }
                else
                {
                    updateBoolValues = false;
                    CheckState = CheckState.Checked;
                    updateBoolValues = true;
                }
            }

            if (raiseChangedEvent)
            {
                RoutedEventArgs t = new RoutedEventArgs(CheckChangedEvent);
                RaiseEvent(t);
                RaiseCheckedEvent();
            }
        }

        private void CheckBox_InternalIsIndeterminateChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // code to run whenever the IsIndeterminate value is changed

            //if (raiseChangedEvent)
            //{
            //    RoutedEventArgs t = new RoutedEventArgs(CheckChangedEvent);
            //    RaiseEvent(t);
            //}

            if (IsIndeterminate)
            {
                raiseChangedEvent = false;
                updateBoolValues = false;
                IsChecked = true;
                CheckState = CheckState.Indeterminate;
                raiseChangedEvent = true;
                updateBoolValues = true;
            }
            else
            {
                if (IsChecked)
                {
                    updateBoolValues = false;
                    CheckState = CheckState.Checked;
                    updateBoolValues = true;
                }
                else
                {
                    updateBoolValues = false;
                    CheckState = CheckState.Unchecked;
                    updateBoolValues = true;
                }
            }

            if (raiseChangedEvent)
            {
                RoutedEventArgs t = new RoutedEventArgs(CheckChangedEvent);
                RaiseEvent(t);
                RaiseCheckedEvent();
            }
        }

        void RaiseCheckedEvent()
        {
            RoutedEventArgs t;

            switch (CheckState)
            {
                case CheckState.Unchecked:
                    t = new RoutedEventArgs(UncheckedEvent);
                    break;
                case CheckState.Checked:
                    t = new RoutedEventArgs(CheckedEvent);
                    break;
                case CheckState.Indeterminate:
                    t = new RoutedEventArgs(IndeterminateEvent);
                    break;
                default:
                    return;
            }

            RaiseEvent(t);
        }

        #endregion

        #region Color Scheme
        /// <summary>
        /// Raised when the ColorScheme property is changed.
        /// </summary>
#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? ColorSchemeChanged;
#else
        public event DependencyPropertyChangedEventHandler ColorSchemeChanged;
#endif


        /// <summary>
        /// A dependency property object backing the related ColorScheme property. See <see cref="ColorScheme"/> for more details.
        /// </summary>
        public static readonly DependencyProperty ColorSchemeProperty
            = DependencyProperty.Register("ColorScheme", typeof(ColorScheme), typeof(CheckBox),
            new FrameworkPropertyMetadata(new ColorScheme(), new PropertyChangedCallback(OnColorSchemeChanged)));

        /// <summary>
        /// Perform an action when the ColorScheme property has changed. Primarily used internally.
        /// </summary>
        /// <param name="d">The object containing the property that changed.</param>
        /// <param name="e">Event arguments about the property change.</param>
        public static void OnColorSchemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if NETCOREAPP
            ColorScheme cs = (e.NewValue as ColorScheme)!;
#else
            ColorScheme cs = e.NewValue as ColorScheme;
#endif

            if (d is CheckBox c)
            {
                c.ColorSchemeChanged?.Invoke(d, e);
                c.ApplyColorScheme(cs);
            }
        }

        /// <summary>
        /// Get or set the color scheme used for this checkbox. For easier color scheme management, bind this to the window or larger control you're using.
        /// </summary>
        public ColorScheme ColorScheme
        {
            get => (ColorScheme)GetValue(ColorSchemeProperty);
            set => SetValue(ColorSchemeProperty, value);
        }

        /// <summary>
        /// Apply a color scheme to this control. The color scheme can quickly apply a whole visual style to the control.
        /// </summary>
        /// <param name="cs">The color scheme to apply.</param>
        public void ApplyColorScheme(ColorScheme cs)
        {
            if (cs == null)
            {
                return;
            }
            if (cs != ColorScheme)
            {
                ColorScheme = cs;
                return;
            }

            if (cs.IsHighContrast)
            {
                //Background = cs.BackgroundColor.ToBrush();
                BackgroundDisabledBrush = cs.BackgroundColor.ToBrush();
                CheckForeground = cs.ForegroundColor.ToBrush();
            }
            else
            {
                //Background = Colors.White.ToBrush();
                BackgroundDisabledBrush = cs.LightDisabledColor.ToBrush();
                CheckForeground = Colors.Black.ToBrush();
            }

            //Background = cs.SecondaryColor.ToBrush();
            BorderBrush = cs.BorderColor.ToBrush();
            //HighlightBrush = cs.SecondHighlightColor.ToBrush();
            BackgroundDisabledBrush = cs.LightDisabledColor.ToBrush();
            BorderDisabledBrush = cs.DarkDisabledColor.ToBrush();
            CheckDisabledBrush = cs.DarkDisabledColor.ToBrush();
            //SelectedBrush = cs.ThirdHighlightColor.ToBrush();
            BorderHighlightBrush = cs.HighlightColor.ToBrush();
            //BorderSelectedBrush = cs.SelectionColor.ToBrush();
            Foreground = cs.ForegroundColor.ToBrush();
        }
        #endregion

        #region Brushes

        /// <summary>
        /// Get or set the brush used for the background of the checkbox's box. This is not set via a color scheme.
        /// </summary>
        [Category("Brushes")]
        public Brush CheckBackground
        {
            get => (Brush)GetValue(CheckBackgroundProperty);
            set => SetValue(CheckBackgroundProperty, value);
        }
        
        /// <summary>
        /// Get or set the brush used for the check mark in the checkbox's box.
        /// </summary>
        [Category("Brushes")]
        public Brush CheckForeground
        {
            get => (Brush)GetValue(CheckForegroundProperty);
            set => SetValue(CheckForegroundProperty, value);
        }

        //[Category("Brushes")]
        //public Brush ClickBrush
        //{
        //    get => (Brush)GetValue(ClickBrushProperty);
        //    set => SetValue(ClickBrushProperty, value);
        //}

        //[Category("Brushes")]
        //public Brush SelectedBrush
        //{
        //    get => (Brush)GetValue(SelectedBrushProperty);
        //    set => SetValue(SelectedBrushProperty, value);
        //}

        //[Category("Brushes")]
        //public Brush HighlightBrush
        //{
        //    get => (Brush)GetValue(HighlightBrushProperty);
        //    set => SetValue(HighlightBrushProperty, value);
        //}

        /// <summary>
        /// Get or set the brush to use for the background of the checkbox's box when it is disabled.
        /// </summary>
        [Category("Brushes")]
        public Brush BackgroundDisabledBrush
        {
            get => (Brush)GetValue(BackgroundDisabledBrushProperty);
            set => SetValue(BackgroundDisabledBrushProperty, value);
        }

        /// <summary>
        /// Get or set the brush used for the border of the checkbox's box when it is disabled.
        /// </summary>
        [Category("Brushes")]
        public Brush BorderDisabledBrush
        {
            get => (Brush)GetValue(BorderDisabledBrushProperty);
            set => SetValue(BorderDisabledBrushProperty, value);
        }

        /// <summary>
        /// Get or set the brush used for the check mark when the control is disabled.
        /// </summary>
        [Category("Brushes")]
        public Brush CheckDisabledBrush
        {
            get => (Brush)GetValue(CheckDisabledBrushProperty);
            set => SetValue(CheckDisabledBrushProperty, value);
        }

        /// <summary>
        /// Get or set the brush used for the border of the checkbox's box.
        /// </summary>
        [Category("Brushes")]
        public new Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        /// <summary>
        /// Get or set the brush used for the border of the checkbox's box, while the mouse is over the control or it has keyboard focus.
        /// </summary>
        [Category("Brushes")]
        public Brush BorderHighlightBrush
        {
            get => (Brush)GetValue(BorderHighlightBrushProperty);
            set => SetValue(BorderHighlightBrushProperty, value);
        }

        //[Category("Brushes")]
        //public Brush BorderSelectedBrush
        //{
        //    get => (Brush)GetValue(BorderSelectedBrushProperty);
        //    set => SetValue(BorderSelectedBrushProperty, value);
        //}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly DependencyProperty CheckBackgroundProperty = DependencyProperty.Register(
            "CheckBackground", typeof(Brush), typeof(CheckBox),
            new PropertyMetadata(new SolidColorBrush(ColorsHelper.White)));

        public static readonly DependencyProperty CheckForegroundProperty = DependencyProperty.Register(
            "CheckForeground", typeof(Brush), typeof(CheckBox),
            new PropertyMetadata(new SolidColorBrush(ColorsHelper.Black)));

        //public static readonly DependencyProperty ClickBrushProperty = DependencyProperty.Register(
        //    "ClickBrush", typeof(Brush), typeof(CheckBox),
        //    new PropertyMetadata(Colors.Gainsboro.ToBrush()));

        //public static readonly DependencyProperty SelectedBrushProperty = DependencyProperty.Register(
        //    "SelectedBrush", typeof(Brush), typeof(CheckBox),
        //    new PropertyMetadata(Colors.WhiteSmoke.ToBrush()));

        //public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.Register(
        //    "HighlightBrush", typeof(Brush), typeof(CheckBox),
        //    new PropertyMetadata(Colors.LightGray.ToBrush()));

        public static readonly DependencyProperty BackgroundDisabledBrushProperty = DependencyProperty.Register(
            "BackgroundDisabledBrush", typeof(Brush), typeof(CheckBox),
            new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public static readonly DependencyProperty BorderDisabledBrushProperty = DependencyProperty.Register(
            "BorderDisabledBrush", typeof(Brush), typeof(CheckBox),
            new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public static readonly DependencyProperty CheckDisabledBrushProperty = DependencyProperty.Register(
            "CheckDisabledBrush", typeof(Brush), typeof(CheckBox),
            new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        public static readonly new DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            "BorderBrush", typeof(Brush), typeof(CheckBox),
            new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty BorderHighlightBrushProperty = DependencyProperty.Register(
            "BorderHighlightBrush", typeof(Brush), typeof(CheckBox),
            new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty BorderSelectedBrushProperty = DependencyProperty.Register(
            "BorderSelectedBrush", typeof(Brush), typeof(CheckBox),
            new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));
        #endregion

        #region Border

        public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
            "BorderThickness", typeof(Thickness), typeof(CheckBox),
            new PropertyMetadata(new Thickness(1)));

        public static readonly DependencyProperty BorderSelectionThicknessProperty = DependencyProperty.Register(
            "BorderSelectionThickness", typeof(Thickness), typeof(CheckBox),
            new PropertyMetadata(new Thickness(1)));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(CheckBox),
            new PropertyMetadata(new CornerRadius(0)));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Get or set the thickness of the border of the check box.
        /// </summary>
        [Category("Appearance")]
        public new Thickness BorderThickness
        {
            get => (Thickness)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        /// <summary>
        /// Get or set the thickness of the border of the check box, while the check box's IsChecked property is true.
        /// </summary>
        [Category("Appearance")]
        public Thickness BorderSelectionThickness
        {
            get => (Thickness)GetValue(BorderSelectionThicknessProperty);
            set => SetValue(BorderSelectionThicknessProperty, value);
        }

        /// <summary>
        /// Get or set the corner radius of the check box.
        /// </summary>
        [Category("Appearance")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region Click Handling

        #region Routed Events

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckBox));
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
            "RightClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckBox));
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
        [Category("Common")]
        public bool ClickOnPress { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the checkbox should cycle through three states (rather than two) when clicked. The third state is the "Indeterminate" state, which can be checked via the IsIndeterminate property.
        /// </summary>
        [Category("Common")]
        public bool TriStateClick { get => (bool)GetValue(TriStateClickProperty); set => SetValue(TriStateClickProperty, value); }

        /// <summary>
        /// A dependency property object backing the related property. See the property itself for more details.
        /// </summary>
        public static DependencyProperty TriStateClickProperty
            = DependencyProperty.Register("TriStateClick", typeof(bool), typeof(CheckBox),
            new FrameworkPropertyMetadata(false));


        /// <summary>
        /// Gets or sets whether clicking should only occur when the checkbox's box is clicked, and not the rest of the control.
        /// </summary>
        [Category("Common")]
        public bool OnlyAllowCheckBoxClick { get => (bool)GetValue(OnlyAllowCheckBoxClickProperty); set => SetValue(OnlyAllowCheckBoxClickProperty, value); }

        /// <summary>
        /// A dependency property object backing the related property. See the property itself for more details.
        /// </summary>
        public static DependencyProperty OnlyAllowCheckBoxClickProperty
            = DependencyProperty.Register("OnlyAllowCheckBoxClick", typeof(bool), typeof(CheckBox),
            new FrameworkPropertyMetadata(false));


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

                if (OnlyAllowCheckBoxClick && !checkBoxClick)
                {
                    // exit out
                    initiatingClick = false;
                    return;
                }

                if (TriStateClick)
                {
                    if (IsChecked && IsIndeterminate)
                    {
                        IsChecked = false;
                    }
                    else if (IsChecked && !IsIndeterminate)
                    {
                        IsIndeterminate = true;
                    }
                    else
                    {
                        IsChecked = true;
                        IsIndeterminate = false;
                    }
                }
                else
                {
                    IsChecked = !IsChecked;
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

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PerformPress(e.ChangedButton == MouseButton.Right);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PerformClick(e.ChangedButton == MouseButton.Right);
            e.Handled = true;
        }

#if NETCOREAPP
        private void UserControl_TouchDown(object? sender, TouchEventArgs e)
        {
            PerformPress();
        }

        private void UserControl_TouchUp(object? sender, TouchEventArgs e)
        {
            PerformClick();
        }

#else
        private void UserControl_TouchDown(object sender, TouchEventArgs e)
        {
            PerformPress();
        }

        private void UserControl_TouchUp(object sender, TouchEventArgs e)
        {
            PerformClick();
        }
#endif

        private void UserControl_StylusDown(object sender, StylusDownEventArgs e)
        {
            PerformPress();
        }

        private void UserControl_StylusUp(object sender, StylusEventArgs e)
        {
            PerformClick();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                PerformPress();
            }
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
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

    /// <summary>
    /// Represents the current state of a CheckBox.
    /// </summary>
    public enum CheckState
    {
        /// <summary>
        /// The CheckBox is unchecked. Nothing is displayed in the box.
        /// </summary>
        Unchecked = 0,
        /// <summary>
        /// The CheckBox is checked. A check mark is displayed in the box.
        /// </summary>
        Checked = 1,
        /// <summary>
        /// The CheckBox is indeterminate. A square is displayed in the box.
        /// </summary>
        Indeterminate = 2,
    }
}
