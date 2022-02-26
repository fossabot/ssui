﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xaml;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.ComponentModel;

namespace SolidShineUi
{
    /// <summary>
    /// A tab that can be added onto a <see cref="TabControl"/>. Each tab has a title, icon, and close button by default.
    /// </summary>
    [ContentProperty("Content")]
    public class TabItem : DependencyObject
    {
        public TabItem()
        {
            //Self = this;

            InternalTitleChanged += tabItem_InternalTitleChanged;
            InternalIsDirtyChanged += tabItem_InternalIsDirtyChanged;
            InternalCanCloseChanged += tabItem_InternalCanCloseChanged;
            InternalCanSelectChanged += tabItem_InternalCanSelectChanged;
            InternalIconChanged += tabItem_InternalIconChanged;
            InternalContentChanged += tabItem_InternalContentChanged;
            InternalVisibilityChanged += tabItem_InternalVisibilityChanged;
            InternalShowIconChanged += tabItem_InternalShowIconChanged;
            InternalAllowDropChanged += tabItem_InternalAllowDropChanged;
            InternalToolTipChanged += tabItem_InternalToolTipChanged;
            InternalDisplayDirtyStateChanged += tabItem_InternalDisplayDirtyStateChanged;
            InternalTabBackgroundChanged += tabItem_InternalTabBackgroundChanged;
            InternalPaddingChanged += tabItem_InternalPaddingChanged;
        }

        #region Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TabItem),
            new PropertyMetadata("New Tab", new PropertyChangedCallback(OnInternalTitleChanged)));

        ///<summary>
        /// Get or set the title of this tab.
        ///</summary>
        [Category("Common")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalTitleChanged;

#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? TitleChanged;
#else
        public event DependencyPropertyChangedEventHandler TitleChanged;
#endif

        private static void OnInternalTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalTitleChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalTitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TitleChanged?.Invoke(this, e);
        }
        #endregion

        #region IsDirty

        public static readonly DependencyProperty IsDirtyProperty = DependencyProperty.Register("IsDirty", typeof(bool), typeof(TabItem),
            new PropertyMetadata(false, new PropertyChangedCallback(OnInternalIsDirtyChanged)));

        ///<summary>
        /// Get or set if the tab is dirty.
        ///</summary>
        [Category("Common")]
        public bool IsDirty
        {
            get { return (bool)GetValue(IsDirtyProperty); }
            set { SetValue(IsDirtyProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalIsDirtyChanged;

#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? IsDirtyChanged;
#else
        public event DependencyPropertyChangedEventHandler IsDirtyChanged;
#endif

        private static void OnInternalIsDirtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalIsDirtyChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalIsDirtyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsDirtyChanged?.Invoke(this, e);
        }
        #endregion

        #region DisplayDirtyState

        public static readonly DependencyProperty DisplayDirtyStateProperty = DependencyProperty.Register("DisplayDirtyState", typeof(bool), typeof(TabItem),
            new PropertyMetadata(true, new PropertyChangedCallback(OnInternalDisplayDirtyStateChanged)));

        ///<summary>
        /// Get or set if the tab should display the state of its IsDirty property. When set to true, an asterisk (*) will be added to the title when it is dirty.
        ///</summary>
        [Category("Common")]
        public bool DisplayDirtyState
        {
            get { return (bool)GetValue(DisplayDirtyStateProperty); }
            set { SetValue(DisplayDirtyStateProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalDisplayDirtyStateChanged;

#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? DisplayDirtyStateChanged;
#else
        public event DependencyPropertyChangedEventHandler DisplayDirtyStateChanged;
#endif

        private static void OnInternalDisplayDirtyStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalDisplayDirtyStateChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalDisplayDirtyStateChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DisplayDirtyStateChanged?.Invoke(this, e);
        }
        #endregion

        #region CanClose

        public static readonly DependencyProperty CanCloseProperty = DependencyProperty.Register("CanClose", typeof(bool), typeof(TabItem),
            new PropertyMetadata(true, new PropertyChangedCallback(OnInternalCanCloseChanged)));

        ///<summary>
        /// Get or set if this tab can be closed via the UI (i.e. the close button).
        ///</summary>
        ///<remarks>
        /// This property does not prevent this tab being closed via other means, such as <see cref="TabItem.Close()"/> or <see cref="SelectableCollection{TabItem}.Remove(TabItem)"/>.
        /// </remarks>
        [Category("Common")]
        public bool CanClose
        {
            get { return (bool)GetValue(CanCloseProperty); }
            set { SetValue(CanCloseProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalCanCloseChanged;

#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? CanCloseChanged;
#else
        public event DependencyPropertyChangedEventHandler CanCloseChanged;
#endif

        private static void OnInternalCanCloseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalCanCloseChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalCanCloseChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CanCloseChanged?.Invoke(this, e);
        }
        #endregion

        #region ShowIcon

        public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register("ShowIcon", typeof(bool), typeof(TabItem),
            new PropertyMetadata(true, new PropertyChangedCallback(OnInternalShowIconChanged)));

        ///<summary>
        /// Get or set if an icon is shown for this tab. The icon section of the tab will be visible if this is true, even if there is no icon set.
        ///</summary>
        [Category("Common")]
        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalShowIconChanged;

#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? ShowIconChanged;
#else
        public event DependencyPropertyChangedEventHandler ShowIconChanged;
#endif

        private static void OnInternalShowIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalShowIconChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalShowIconChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ShowIconChanged?.Invoke(this, e);
        }
        #endregion

        #region CanSelect

        public static readonly DependencyProperty CanSelectProperty = DependencyProperty.Register("CanSelect", typeof(bool), typeof(TabItem),
            new PropertyMetadata(true, new PropertyChangedCallback(OnInternalCanSelectChanged)));

        ///<summary>
        /// Get or set whether this tab can be selected via UI.
        ///</summary>
        [Category("Common")]
        public bool CanSelect
        {
            get { return (bool)GetValue(CanSelectProperty); }
            set { SetValue(CanSelectProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalCanSelectChanged;

#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? CanSelectChanged;
#else
        public event DependencyPropertyChangedEventHandler CanSelectChanged;
#endif

        private static void OnInternalCanSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalCanSelectChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalCanSelectChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CanSelectChanged?.Invoke(this, e);
        }
        #endregion

        #region Icon

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TabItem),
            new PropertyMetadata(null, new PropertyChangedCallback(OnInternalIconChanged)));

        ///<summary>
        /// Get or set the icon to display with this tab.
        ///</summary>
        [Category("Common")]
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalIconChanged;

        /// <summary>
        /// Raised when the tab's icon is changed.
        /// </summary>
#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? IconChanged;
#else
        public event DependencyPropertyChangedEventHandler IconChanged;
#endif

        private static void OnInternalIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalIconChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalIconChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IconChanged?.Invoke(this, e);
        }
        #endregion

        #region TabBackground

        public static readonly DependencyProperty TabBackgroundProperty = DependencyProperty.Register("TabBackground", typeof(Brush), typeof(TabItem),
            new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(OnInternalTabBackgroundChanged)));

        ///<summary>
        /// Get or set the brush displayed in the background of the tab. This will override the color of the TabControl itself, but transparency does allow the standard color to show through.
        ///</summary>
        [Category("Brush")]
        public Brush TabBackground
        {
            get { return (Brush)GetValue(TabBackgroundProperty); }
            set { SetValue(TabBackgroundProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalTabBackgroundChanged;

        /// <summary>
        /// Raised when the tab's background is changed.
        /// </summary>
#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? TabBackgroundChanged;
#else
        public event DependencyPropertyChangedEventHandler TabBackgroundChanged;
#endif

        private static void OnInternalTabBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalTabBackgroundChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalTabBackgroundChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TabBackgroundChanged?.Invoke(this, e);
        }
        #endregion

        #region Content

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(TabItem),
            new PropertyMetadata(null, new PropertyChangedCallback(OnInternalContentChanged)));

        ///<summary>
        /// Get or set the content to display when this tab is selected.
        ///</summary>
        [Category("Common")]
        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalContentChanged;

        /// <summary>
        /// Raised when the tab's content property is changed.
        /// </summary>
#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? ContentChanged;
#else
        public event DependencyPropertyChangedEventHandler ContentChanged;
#endif

        private static void OnInternalContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalContentChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalContentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ContentChanged?.Invoke(this, e);
        }
        #endregion

        #region Padding

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(TabItem),
            new PropertyMetadata(new Thickness(4,0,4,0), new PropertyChangedCallback(OnInternalPaddingChanged)));

        ///<summary>
        /// Get or set the padding (or space) applied around the tab's title and icon. (This does not set the padding for the content.)
        ///</summary>
        [Category("Common")]
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalPaddingChanged;

#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? PaddingChanged;
#else
        public event DependencyPropertyChangedEventHandler PaddingChanged;
#endif

        private static void OnInternalPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalPaddingChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalPaddingChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PaddingChanged?.Invoke(this, e);
        }

        #endregion

        #region Visibility

        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(TabItem),
            new PropertyMetadata(System.Windows.Visibility.Visible, new PropertyChangedCallback(OnInternalVisibilityChanged)));

        ///<summary>
        /// Get or set if this tab is visually displayed in the UI.
        ///</summary>
        [Category("Appearance")]
        public Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalVisibilityChanged;

        /// <summary>
        /// Raised when the tab's visibility property is changed.
        /// </summary>
#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? VisibilityChanged;
#else
        public event DependencyPropertyChangedEventHandler VisibilityChanged;
#endif

        private static void OnInternalVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalVisibilityChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VisibilityChanged?.Invoke(this, e);
        }
        #endregion

        #region TabContextMenu

        public static readonly DependencyProperty TabContextMenuProperty = DependencyProperty.Register("TabContextMenu", typeof(ContextMenu), typeof(TabItem),
            new PropertyMetadata(null));

#if NETCOREAPP
        /// <summary>
        /// Get or set the menu that appears when you right-click on the tab in the TabControl.
        /// </summary>
        [Category("Common")]
        public ContextMenu? TabContextMenu
        {
            get { return (ContextMenu)GetValue(TabContextMenuProperty); }
            set { SetValue(TabContextMenuProperty, value); }
        }
#else
        /// <summary>
        /// Get or set the menu that appears when you right-click on the tab in the TabControl.
        /// </summary>
        [Category("Common")]
        public ContextMenu TabContextMenu
        {
            get { return (ContextMenu)GetValue(TabContextMenuProperty); }
            set { SetValue(TabContextMenuProperty, value); }
        }
#endif

        #endregion

        #region AllowDrop

        public static readonly DependencyProperty AllowDropProperty = DependencyProperty.Register("AllowDrop", typeof(bool), typeof(TabItem),
            new PropertyMetadata(false, new PropertyChangedCallback(OnInternalAllowDropChanged)));

        ///<summary>
        /// Get or set if this tab can be used as a target in drag-and-drop operations.
        ///</summary>
        ///<remarks>
        /// Note that this only sets if this tab can recieve data dropped onto it in the tab bar. The elements inside the tab can set their own AllowDrop property if you want to drop items inside the tab's contents.
        /// This property does not need to be set to allow tab drag-and-drop on the TabControl (for rearranging tabs). Tab drag-and-drop is controlled via the <see cref="TabControl.AllowTabDragDrop"/> property.
        /// </remarks>
        [Category("Common")]
        public bool AllowDrop
        {
            get { return (bool)GetValue(AllowDropProperty); }
            set { SetValue(AllowDropProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalAllowDropChanged;

#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? AllowDropChanged;
#else
        public event DependencyPropertyChangedEventHandler AllowDropChanged;
#endif

        private static void OnInternalAllowDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalAllowDropChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalAllowDropChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AllowDropChanged?.Invoke(this, e);
        }
        #endregion

        #region Drag and drop events

        // The TabDisplayItem is the visual item that actually displays tabs in the TabControl
        // When a drag event occurs in the TabDisplayItem, it calls this RaiseDragEvent method so that the TabItem can raise the respective event.

        internal protected void RaiseDragEvent(string ev, DragEventArgs e)
        {
            switch (ev)
            {
                case "DragEnter":
                    DragEnter?.Invoke(this, e);
                    break;
                case "DragOver":
                    DragOver?.Invoke(this, e);
                    break;
                case "DragLeave":
                    DragLeave?.Invoke(this, e);
                    break;
                case "Drop":
                    Drop?.Invoke(this, e);
                    break;
                case "PreviewDragEnter":
                    PreviewDragEnter?.Invoke(this, e);
                    break;
                case "PreviewDragOver":
                    PreviewDragOver?.Invoke(this, e);
                    break;
                case "PreviewDragLeave":
                    PreviewDragLeave?.Invoke(this, e);
                    break;
                case "PreviewDrop":
                    PreviewDrop?.Invoke(this, e);
                    break;
                default:
                    break;
            }
        }

#if NETCOREAPP
        public event DragEventHandler? DragEnter;
        public event DragEventHandler? DragOver;
        public event DragEventHandler? DragLeave;
        public event DragEventHandler? Drop;

        public event DragEventHandler? PreviewDragEnter;
        public event DragEventHandler? PreviewDragOver;
        public event DragEventHandler? PreviewDragLeave;
        public event DragEventHandler? PreviewDrop;
#else
        public event DragEventHandler DragEnter;
        public event DragEventHandler DragOver;
        public event DragEventHandler DragLeave;
        public event DragEventHandler Drop;

        public event DragEventHandler PreviewDragEnter;
        public event DragEventHandler PreviewDragOver;
        public event DragEventHandler PreviewDragLeave;
        public event DragEventHandler PreviewDrop;
#endif

        #endregion

        #region ToolTip

        public static readonly DependencyProperty ToolTipProperty = DependencyProperty.Register("ToolTip", typeof(object), typeof(TabItem),
            new PropertyMetadata(null, new PropertyChangedCallback(OnInternalToolTipChanged)));

        ///<summary>
        /// Get or set the ToolTip to display when the mouse is hovering over this tab.
        ///</summary>
        [Category("Common")]
        public object ToolTip
        {
            get { return (object)GetValue(ToolTipProperty); }
            set { SetValue(ToolTipProperty, value); }
        }

        protected event DependencyPropertyChangedEventHandler InternalToolTipChanged;

        /// <summary>
        /// Raised when the tab's ToolTip property is changed.
        /// </summary>
#if NETCOREAPP
        public event DependencyPropertyChangedEventHandler? ToolTipChanged;
#else
        public event DependencyPropertyChangedEventHandler ToolTipChanged;
#endif

        private static void OnInternalToolTipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem s)
            {
                s.InternalToolTipChanged?.Invoke(s, e);
            }
        }
        private void tabItem_InternalToolTipChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ToolTipChanged?.Invoke(this, e);
        }
        #endregion

        /// <summary>
        /// Close this tab (and remove it from the TabControl), if it is currently in a TabControl.
        /// </summary>
        /// <remarks>
        /// This function can be run regardless of the state of the <c>CanClose</c> property.
        /// </remarks>
        public void Close()
        {
            InternalTabClosing?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Attempt to bring the current tab into view on the tab bar
        /// </summary>
        public void BringIntoView()
        {
            InternalBringIntoViewRequested?.Invoke(this, EventArgs.Empty);
        }

#if NETCOREAPP
        internal protected event EventHandler? InternalTabClosing;
        internal protected event EventHandler? InternalBringIntoViewRequested;
#else
        internal protected event EventHandler InternalTabClosing;
        internal protected event EventHandler InternalBringIntoViewRequested;
#endif

        ///<summary>
        /// Get or set if this tab is visible in the UI.
        ///</summary>
        [Category("Appearance")]
        public bool IsVisible
        {
            get
            {
                return Visibility == Visibility.Visible;
            }
            set
            {
                Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

    }
}
