﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SolidShineUi;

namespace SolidShineUi.PropertyList.Dialogs
{
    /// <summary>
    /// A dialog for the user to select a char or Rune via their Unicode code point. Primarily designed for the <see cref="PropertyEditors.CharEditor"/>, but can be used independently as well.
    /// </summary>
    public partial class CharInputDialog : FlatWindow
    {

        #region Window Actions

        /// <summary>
        /// Create a CharInputDialog.
        /// </summary>
        public CharInputDialog()
        {
            InitializeComponent();
            ColorScheme = new ColorScheme();
        }

        /// <summary>
        /// Create a CharInputDialog with a color scheme.
        /// </summary>
        /// <param name="cs">The color scheme to use for the window.</param>
        public CharInputDialog(ColorScheme cs)
        {
            InitializeComponent();
            ColorScheme = cs;
        }

        /// <summary>
        /// Create a CharInputDialog with prefilled values, with a char being entered in.
        /// </summary>
        /// <param name="cs">The color scheme to use for the window.</param>
        /// <param name="value">The value to preload into this dialog. The user is able to change the value though.</param>
        public CharInputDialog(ColorScheme cs, char value = 'a')
        {
            InitializeComponent();
            ColorScheme = cs;

            c = value;
            EnterValueIntoBoxes();
        }

#if NETCOREAPP
        /// <summary>
        /// Create a StringInputBox with prefilled values, with a Rune being entered in.
        /// </summary>
        /// <param name="cs">The color scheme to use for the window.</param>
        /// <param name="value">The value to preload into this dialog. The user is able to change the value though.</param>
        /// <remarks>Entering in a Rune value will put the dialog into "Rune mode", which can be used to enter in and display characters beyond what a single char can support.</remarks>
        public CharInputDialog(ColorScheme cs, Rune value)
        {
            InitializeComponent();
            ColorScheme = cs;

            r = value;
            runeMode = true;
            nudDec.MaxValue = 1114111; // 10FFFF - max valid Unicode code point
            txtHex.MaxValue = 1114111;
            EnterValueIntoBoxes();
        }

        /// <summary>
        /// Enable rune mode for this dialog, which will allow the display of characters beyond what a single char can support. This is meant to be used in situations where a Rune, not a char, is needed.
        /// </summary>
        /// <remarks>
        /// If you suppled a Rune in the constructor for this dialog, then the dialog is already in Rune mode.
        /// When using Rune mode, you should get the value entered in via using <see cref="ValueAsRune"/>.
        /// </remarks>
        public void EnterIntoRuneMode()
        {
            if (runeMode) return;

            r = new Rune(c);
            runeMode = true;
            nudDec.MaxValue = 1114111; // 10FFFF - max valid Unicode code point
            txtHex.MaxValue = 1114111;
            EnterValueIntoBoxes();
        }
#endif

        private void FlatWindow_Loaded(object sender, RoutedEventArgs e)
        {
            nudDec.MinValue = char.MinValue;
            nudDec.MaxValue = char.MaxValue;

            txtHex.MinValue = char.MinValue;
            txtHex.MaxValue = char.MaxValue;

            if (runeMode)
            {
                nudDec.MaxValue = 1114111; // 10FFFF - max valid Unicode code point
                txtHex.MaxValue = 1114111;
            }

            txtHex.Focus();
        }

        #endregion

        bool runeMode = false;
        char c = 'a';
#if NETCOREAPP
        Rune r = (Rune)'a';
#endif

        bool _internalAction = false;

        void EnterValueIntoBoxes()
        {
            _internalAction = true;
            UpdateHexBox();
            UpdateDecBox();
            UpdatePreview();
            _internalAction = false;
        }

        void UpdateHexBox()
        {
            if (runeMode)
            {
#if NETCOREAPP
                txtHex.Value = r.Value;
#else
                txtHex.Value = 0;
#endif
            }
            else
            {
                txtHex.Value = c;
            }
        }

        void UpdateDecBox()
        {
            if (runeMode)
            {
#if NETCOREAPP
                nudDec.Value = r.Value;
#else
                nudDec.Value = 0;
#endif
            }
            else
            {
                nudDec.Value = c;
            }
        }

        void UpdatePreview()
        {
            if (runeMode)
            {
#if NETCOREAPP
                txtPreview.Text = r.ToString();
#else
                txtPreview.Text = "(error - rune mode)";
#endif
            }
            else
            {
                if (char.IsSurrogate(c))
                {
                    txtPreview.Text = "(surrogate)";
                }
                else
                {
                    txtPreview.Text = new string(c, 1);
                }
            }
        }

        /// <summary>
        /// Get the Unicode character selected in this dialog, as a char.
        /// </summary>
        /// <remarks>
        /// Note that chars are explicitly UTF-16. Many code points (especially multilingual, extended, or symbol/emoji characters) cannot be represented as a single UTF-16 char, and instead will require two.
        /// This dialog only returns one char, but you can check if it is such a char by using <see cref="char.IsSurrogate(char)"/>.
        /// 
        /// If using .NET Core or .NET 5 or higher, you can use Runes instead.
        /// </remarks>
        public char ValueAsChar
        {
            get
            {
#if NETCOREAPP
                if (runeMode)
                {
                    return r.ToString()[0];
                }
                else
                {
                    return c;
                }
#else
            return c;
#endif
            }
            set
            {
#if NETCOREAPP

#else
            c = value;
            EnterValueIntoBoxes();
#endif
            }
        }

#if NETCOREAPP
        /// <summary>
        /// Get the Unicode character selected in this dialog, as a Rune.
        /// </summary>
        public Rune ValueAsRune
        {
            get
            {
                if (runeMode)
                {
                    return r;
                }
                else
                {
                    return (Rune)c;
                }
            }
        }
#endif

        /// <summary>
        /// Get the result of the dialog when it is closed. "False" refers to the user cancelling the operation, while "True" refers to the user confirming, by clicking "OK" or pressing the Enter key.
        /// </summary>
        public new bool DialogResult { get; private set; } = false;

        /// <summary>
        /// Get or set whether the Enter key can be used to confirm the dialog. If enabled, pressing down the Enter key will be treated as if the user pressed "OK".
        /// </summary>
        public bool EnterKeyConfirms { get; set; } = true;

        /// <summary>
        /// Get or set whether the Escape key can be used to cancel the dialog. If enabled, pressing down the Escape key will be treated as if the user pressed "Cancel".
        /// </summary>
        public bool EscapeKeyCancels { get; set; } = true;

        /// <summary>
        /// Get or set the description text to display above the text box. This text should describe what the user should enter into the text box.
        /// </summary>
        /// <remarks>Try to keep the description to about a sentence long. 
        /// Ideally, the overall design of the program should make it apparent what the user should enter into the text box without reading the description.</remarks>
        public string Description
        {
            get
            {
                return txtDesc.Text;
            }
            set
            {
                txtDesc.Text = value;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void txtName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && EnterKeyConfirms)
            {
                btnOK_Click(this, e);
            }
            else if (e.Key == System.Windows.Input.Key.Escape && EscapeKeyCancels)
            {
                btnCancel_Click(this, e);
            }
        }

        private static string UnicodeToChar(int code)
        {
            try
            {
                //int code = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
#if NETCOREAPP
                if (Rune.IsValid(code))
                {
                    Rune r = new Rune(code);
                    return r.ToString();
                }
                else
                {
                    return char.ConvertFromUtf32(0);
                    //if (runeMode)
                    //{
                    //    return char.ConvertFromUtf32(0);
                    //}
                    //else
                    //{
                    //    string unicodeString = char.ConvertFromUtf32(code);
                    //    return unicodeString;
                    //}
                }
#else
                string unicodeString = char.ConvertFromUtf32(code);
                return unicodeString;
#endif
            }
            catch (FormatException)
            {
                return char.ConvertFromUtf32(0);
            }
            catch (ArgumentOutOfRangeException)
            {
                return char.ConvertFromUtf32(0);
            }
        }

        private void nudDec_ValueChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_internalAction) return;

            if (runeMode)
            {
#if NETCOREAPP
                if (Rune.IsValid(nudDec.Value))
                {
                    r = new Rune(nudDec.Value);
                }
                else
                {
                    _internalAction = true;
                    nudDec.Value = 0;
                    r = new Rune(0);
                    _internalAction = false;
                }
#else
                // shrug?
#endif
            }
            else
            {
                try
                {
                    c = Convert.ToChar(nudDec.Value);
                }
                catch (FormatException)
                {
                    _internalAction = true;
                    nudDec.Value = 0;
                    c = Convert.ToChar(0);
                    _internalAction = false;
                }
                catch (ArgumentOutOfRangeException)
                {
                    _internalAction = true;
                    nudDec.Value = 0;
                    c = Convert.ToChar(0);
                    _internalAction = false;
                }
            }

            _internalAction = true;
            UpdateHexBox();
            UpdatePreview();
            _internalAction = false;
        }

        private void txtHex_ValueChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_internalAction) return;

            if (runeMode)
            {
#if NETCOREAPP
                if (Rune.IsValid(txtHex.Value))
                {
                    r = new Rune(txtHex.Value);
                }
                else
                {
                    _internalAction = true;
                    txtHex.Value = 0;
                    r = new Rune(0);
                    _internalAction = false;
                }
#else
                // shrug?
#endif
            }
            else
            {
                try
                {
                    c = Convert.ToChar(txtHex.Value);
                }
                catch (FormatException)
                {
                    _internalAction = true;
                    txtHex.Value = 0;
                    c = Convert.ToChar(0);
                    _internalAction = false;
                }
                catch (ArgumentOutOfRangeException)
                {
                    _internalAction = true;
                    txtHex.Value = 0;
                    c = Convert.ToChar(0);
                    _internalAction = false;
                }
            }

            _internalAction = true;
            UpdateDecBox();
            UpdatePreview();
            _internalAction = false;
        }
    }
}
