﻿using System;
using System.Windows.Media;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SolidShineUi
{


    /// <summary>
    /// Contains a collection of helper functions in relation to colors.
    /// </summary>
    public static class ColorsHelper
    {

        #region RGB/Hex Converters

        /// <summary>
        /// Create a color based upon RGB values.
        /// </summary>
        /// <param name="r">The red value of the color.</param>
        /// <param name="g">The green value of the color.</param>
        /// <param name="b">The blue value of the color.</param>
        /// <returns></returns>
        public static Color CreateFromRgb(byte r, byte g, byte b)
        {
            return Color.FromRgb(r, g, b);
        }

        /// <summary>
        /// Create a color based upon RGB values, with alpha transparency as well.
        /// </summary>
        /// <param name="a">The alpha value (transparency) of the color.</param>
        /// <param name="r">The red value of the color.</param>
        /// <param name="g">The green value of the color.</param>
        /// <param name="b">The blue value of the color.</param>
        /// <returns></returns>
        public static Color CreateFromArgb(byte a, byte r, byte g, byte b)
        {
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Create a color based upon a hex string.
        /// </summary>
        /// <param name="hex">The hex string. Should have 3, 6, or 8 characters (i.e. "FFF", "FF00FF", or "FF00FF00"). The starting hash is also allowed (such as "#FF00FF").</param>
        /// <exception cref="FormatException">Thrown if the string is not a hex string, or incorrectly formatted (including if the string is not of the length 3, 6, or 8).</exception>
        /// <remarks>
        /// Note that the expected format for the strings are RGB for 3-character and 6-character strings, and ARGB (with A first) for 8-character strings.
        /// RGBA strings will be read incorrectly, so verify the format that another location displays/exports hex strings if you're experiencing unexpected results.
        /// </remarks>
        public static Color CreateFromHex(string hex)
        {
            // first, remove the starting # symbol (change #890ABC to 890ABC)
            if (hex.StartsWith("#", StringComparison.Ordinal))
            {
                hex = hex.Substring(1);
            }

            switch (hex.Length)
            {
                case 6: // #890ABC
                    try
                    {
                        return Color.FromRgb(
                            byte.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier),
                            byte.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier),
                            byte.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier));
                    }
                    catch (FormatException ex)
                    {
                        throw new FormatException("Hex string is not in a correct format.", ex);
                    }

                case 8: // #FF890ABC
                    try
                    {
                        return Color.FromArgb(
                            byte.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier),
                            byte.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier),
                            byte.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier),
                            byte.Parse(hex.Substring(6, 2), NumberStyles.AllowHexSpecifier));
                    }
                    catch (FormatException ex)
                    {
                        throw new FormatException("Hex string is not in a correct format.", ex);
                    }

                case 3: // #890 (equivalent to #889900)
                    try
                    {
                        string r = $"{hex.Substring(0, 1)}{hex.Substring(0, 1)}";
                        string g = $"{hex.Substring(1, 1)}{hex.Substring(1, 1)}";
                        string b = $"{hex.Substring(2, 1)}{hex.Substring(2, 1)}";

                        return Color.FromRgb(
                            byte.Parse(r, NumberStyles.AllowHexSpecifier),
                            byte.Parse(g, NumberStyles.AllowHexSpecifier),
                            byte.Parse(b, NumberStyles.AllowHexSpecifier));
                    }
                    catch (FormatException ex)
                    {
                        throw new FormatException("Hex string is not in a correct format.", ex);
                    }

                    // I don't support the 4-digit case here, as there's some minor ambiguity as to what it may mean
                    // I can add 2-digit support at some point though (#45 is #454545)

                default:
                    //throw new ArgumentOutOfRangeException(nameof(hex), "The hex value must have a length of 3, 6, or 8, not including the '#' symbol.");
                    throw new FormatException("The hex value must have a length of 3, 6, or 8, not including the '#' symbol.");
            }
        }

        /// <summary>
        /// Create a color based upon an OLE color value.
        /// </summary>
        /// <param name="oleColor">The OLE color value to translate.</param>
        /// <returns>A color that is the translation of the OLE color value.</returns>
        /// <remarks>Most modern programs will not have much use or need for the OLE color value, but Microsoft Office does still use this in some areas/APIs.</remarks>
        public static Color CreateFromOle(int oleColor)
        {
            System.Drawing.Color c = ColorTranslator.FromOle(oleColor);
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        // TODO: perform benchmark to see if ToHexString or ToHexStringLegacy is faster

        //taken from http://www.cambiaresearch.com/articles/1/convert-dotnet-color-to-hex-string
        //written by Steve Lautenschlager

        //private static readonly char[] hexDigits = {
        // '0', '1', '2', '3', '4', '5', '6', '7',
        // '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

        ///// <summary>
        ///// Returns an RGB hex triplet string that corresponds to this color. Note that A (alpha) is dropped.
        ///// </summary>
        ///// <param name="color">The color to convert to a hex string.</param>
        //public static string ToHexStringLegacy(Color color)
        //{
        //    byte[] bytes = new byte[3];
        //    bytes[0] = color.R;
        //    bytes[1] = color.G;
        //    bytes[2] = color.B;
        //    char[] chars = new char[bytes.Length * 2];
        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        int b = bytes[i];
        //        chars[i * 2] = hexDigits[b >> 4];
        //        chars[i * 2 + 1] = hexDigits[b & 0xF];
        //    }
        //    return new string(chars);
        //}

        /// <summary>
        /// Returns an RGB hex triplet string that corresponds to this color. Note that A (alpha) is dropped, please use <see cref="ToHexStringWithAlpha(Color)"/> to keep the A value as well..
        /// </summary>
        /// <param name="color">The color to convert to a hex string.</param>
        public static string ToHexString(Color color)
        {
            return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        /// <summary>
        /// Returns an ARGB hex string that corresponds to this color. "A" refers to alpha, or transparency.
        /// </summary>
        /// <param name="color">The color to convert to a hex string.</param>
        /// <remarks>
        /// Note that this returns ARGB, which is the same format that <see cref="CreateFromHex(string)"/> supports.
        /// If using this string with another service, please make sure they import strings as ARGB, not RGBA.
        /// </remarks>
        public static string ToHexStringWithAlpha(Color color)
        {
            return color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        /// <summary>
        /// Get the OLE color value that translates to this color.
        /// </summary>
        /// <param name="color">The color to translate to an OLE color value.</param>
        /// <returns>A color that is the translation of the OLE color value.</returns>
        /// <remarks>Most modern programs will not have much use or need for the OLE color value, but Microsoft Office does still use this in some areas/APIs.</remarks>
        public static int ToOleColor(Color color)
        {
            return ColorTranslator.ToOle(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        #endregion

        #region Additional Functions

        /// <summary>Get a color by blending one color onto another color by a specified amount.</summary>
        /// <param name="color">The color to blend onto the background color.</param>
        /// <param name="backColor">The color that is the background or base (that is being blended onto).</param>
        /// <param name="amount">How much of <paramref name="color"/> to blend onto <paramref name="backColor"/>.
        /// Must be between 0 and 1: 0 leaves only the back color (new color blended in 0%), 0.5 is a perfect blend between the two colors, and 1 leaves only the new color (blended in 100%).</param>
        /// <returns>The color that is the result of blending the two colors together.</returns>
        /// <remarks>
        /// The alpha value of the new color will be 255 (opaque). No gamma correction is applied.
        /// </remarks>
        public static Color Blend(Color color, Color backColor, double amount)
        {
            // https://stackoverflow.com/a/3722337/2987285
            if (amount < 0) amount = 0;
            else if (amount > 1) amount = 1;

            byte r = (byte)(color.R * amount + backColor.R * amount);
            byte g = (byte)(color.G * amount + backColor.G * amount);
            byte b = (byte)(color.B * amount + backColor.B * amount);
            return Color.FromRgb(r, g, b);
        }

        #endregion

        #region HSV Math (used for color schemes)

        internal static Color AddValue(double h, double s, double v, double add)
        {
            if (v + add < 0)
            {
                v = 0;
            }
            else if (v + add > 1)
            {
                v = 1;
            }
            else
            {
                v += add;
            }

            return CreateFromHSV(h, s, v);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        static Color AddSatValue(double h, double s, double v, double addS, double addV)
        {
            if (v + addV < 0)
            {
                v = 0;
            }
            else if (v + addV > 1)
            {
                v = 1;
            }
            else
            {
                v += addV;
            }

            if (s + addS < 0)
            {
                s = 0;
            }
            else if (s + addS > 1)
            {
                s = 1;
            }
            else
            {
                s += addS;
            }

            return CreateFromHSV(h, s, v);
        }

        #endregion

        #region HSV conversion
        /// <summary>
        /// Get the HSV values for a particular color.
        /// </summary>
        /// <param name="color">The color to convert to HSV.</param>
        /// <param name="hue">The hue value of the color.</param>
        /// <param name="saturation">The saturation value of the color.</param>
        /// <param name="value">The value (also known as brightness) value of the color.</param>
        public static void ToHSV(Color color, out double hue, out double saturation, out double value)
        {
            // taken from http://www.rapidtables.com/convert/color/rgb-to-hsv.htm
            // backed up by https://en.wikipedia.org/wiki/HSL_and_HSV#Formal_derivation

            double r = Convert.ToDouble(color.R) / 255;
            double g = Convert.ToDouble(color.G) / 255;
            double b = Convert.ToDouble(color.B) / 255;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;
            if (delta == 0)
            {
                hue = 0;
            }
            else if (max == r)
            {
                if ((g - b) < 0)
                {
                    hue = 360 + (60 * (((g - b) / delta) % 6));
                }
                else
                {
                    hue = 60 * (((g - b) / delta) % 6);
                }
            }
            else if (max == g)
            {
                hue = 60 * (((b - r) / delta) + 2);
            }
            else // (max == b)
            {
                hue = 60 * (((r - g) / delta) + 4);
            }

            if (hue < 0)
            {
                Console.WriteLine("HUE UNDER 0: " + hue);
            }

            saturation = (Math.Abs(max) < double.Epsilon) ? 0 : delta / max;
            value = max;

        }

        /// <summary>
        /// Create a color based upon HSV values.
        /// </summary>
        /// <param name="hue">The hue of the color.</param>
        /// <param name="saturation">The saturation of the color.</param>
        /// <param name="value">The value of the color.</param>
        /// <returns>Return a color that corresponds to these HSV values.</returns>
        public static Color CreateFromHSV(double hue, double saturation, double value)
        {
            // taken from https://en.wikipedia.org/wiki/HSL_and_HSV#Converting_to_RGB

            double c = value * saturation; // chroma

            double h = hue / 60;

            double x = c * (1 - Math.Abs((h % 2) - 1));
            double m = value - c;

            double r1;
            double g1;
            double b1;

            if (0 <= h && h < 1)
            {
                r1 = c;
                g1 = x;
                b1 = 0;
            }
            else if (1 <= h && h < 2)
            {
                r1 = x;
                g1 = c;
                b1 = 0;
            }
            else if (2 <= h && h < 3)
            {
                r1 = 0;
                g1 = c;
                b1 = x;
            }
            else if (3 <= h && h < 4)
            {
                r1 = 0;
                g1 = x;
                b1 = c;
            }
            else if (4 <= h && h < 5)
            {
                r1 = x;
                g1 = 0;
                b1 = c;
            }
            else if (5 <= h && h < 6)
            {
                r1 = c;
                g1 = 0;
                b1 = x;
            }
            else
            {
                r1 = 0;
                g1 = 0;
                b1 = 0;
            }

            return Color.FromRgb(Convert.ToByte((r1 + m) * 255), Convert.ToByte((g1 + m) * 255), Convert.ToByte((b1 + m) * 255));
        }

        #endregion

        #region Color Resources

        /// <summary>A preselected color, good to use for a color scheme. Has hex string 168FE5.</summary>
        public static Color Blue { get; } = CreateFromHex("168FE5"); // CornflowerBlue may work too
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 00AEDB.</summary>
        public static Color Cyan { get; } = CreateFromHex("00AEDB");        
        /// <summary>A preselected color. Has hex string 5C8ED3.</summary>
        public static Color GrayBlue { get; } = CreateFromHex("5C8ED3");
        /// <summary>A preselected color, good to use for a color scheme. Has hex string FFBE00.</summary>
        public static Color Yellow { get; } = CreateFromHex("FAD400"); // Goldenrod or #FFC425 may work too
        /// <summary>A preselected color, good to use for a color scheme. Has hex string FF7000.</summary>
        public static Color Orange { get; } = CreateFromHex("FF7000"); // DarkOrange may work too
        /// <summary>A preselected color, good to use for a color scheme. Has hex string E34234.</summary>
        public static Color Red { get; } = CreateFromHex("E34234"); // Vermillion from "A Dictionary of Color"
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 93D2FA.</summary>
        public static Color SkyBlue { get; } = CreateFromHex("93D2FA"); // LightSkyBlue may also work
        /// <summary>A preselected color, good to use for a color scheme. Has hex string FDBCB9.</summary>
        public static Color Pink { get; } = CreateFromHex("FDBCB9"); // May need to be changed (probably to be a bit darker)
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 228B22.</summary>
        public static Color Green { get; } = CreateFromHex("228B22"); // ForestGreen from X11
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 21B17E.</summary>
        public static Color DullGreen { get; } = CreateFromHex("21B17E");
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 74C365.</summary>
        public static Color LightGreen { get; } = CreateFromHex("74C365"); // Mantis
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 6C7C59.</summary>
        public static Color GrayGreen { get; } = CreateFromHex("6C7C59"); // Reseda green
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 947CFB.</summary>
        public static Color LightViolet { get; } = CreateFromHex("947CFB"); // MediumPurple may also work
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 7F66AF.</summary>
        public static Color Violet { get; } = CreateFromHex("7F66AF"); // not shown in UI
        /// <summary>A preselected color, good to use for a color scheme. Has hex string B571C7.</summary>
        public static Color Purple { get; } = CreateFromHex("B571C7");
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 808080.</summary>
        public static Color Gray { get; } = CreateFromHex("808080");
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 954535.</summary>
        public static Color RedBrown { get; } = CreateFromHex("954535"); // not shown in UI / Used to be Brown, changed to RedBrown / Chestnut from "A Dictionary of Color"
        /// <summary>A preselected color, good to use for a color scheme. Has hex string D95E40.</summary>
        public static Color Salmon { get; } = CreateFromHex("d95e40");
        /// <summary>A preselected color, good to use for a color scheme. Has hex string B05010.</summary>
        public static Color Brown { get; } = CreateFromHex("b05010"); // new Brown / Medium brown
        /// <summary>A preselected color. Has hex string FFFFFF.</summary>
        public static Color White { get; } = CreateFromHex("FFF");
        /// <summary>A preselected color. Has hex string 000000.</summary>
        public static Color Black { get; } = CreateFromHex("000");

        // new colors
        /// <summary>A preselected color, good to use for a color scheme. Has hex string C0C0C0.</summary>
        public static Color LightGray { get; } = CreateFromHex("C0C0C0");
        /// <summary>A preselected color. Has hex string 808000.</summary>
        public static Color Olive { get; } = CreateFromHex("808000");
        /// <summary>A preselected color, good to use for a color scheme. Has hex string 666666.</summary>
        public static Color DarkGray { get; } = CreateFromHex("666666");

        /// <summary>A preselected color. Has hex string FFC22C.</summary>
        public static Color Gold { get; } = CreateFromHex("FFC22C");

        /// <summary>A color used in high contrast color schemes, which mirrors Windows's high contrast themes. Has hex string 1AEBFF.</summary>
        public static Color HighContrastLightBlue { get; } = CreateFromHex("1AEBFF"); //1AEBFF
        /// <summary>A color used in high contrast color schemes, which mirrors Windows's high contrast themes. Has hex string 37006E.</summary>
        public static Color HighContrastPurple { get; } = CreateFromHex("37006E");
        /// <summary>A color used in high contrast color schemes, which mirrors Windows's high contrast themes. Has hex string 9B7FB6.</summary>
        public static Color HighContrastLightPurple { get; } = CreateFromHex("9b7fb6");
        /// <summary>A color used in high contrast color schemes, which mirrors Windows's high contrast themes. Has hex string 00FF00.</summary>
        public static Color HighContrastGreen { get; } = CreateFromHex("00FF00"); // 00FF00
        /// <summary>A color used in high contrast color schemes, which mirrors Windows's high contrast themes. Has hex string 81FF81.</summary>
        public static Color HighContrastLightGreen { get; } = CreateFromHex("81ff81"); // 3FF23F
        /// <summary>A color used in high contrast color schemes, which mirrors Windows's high contrast themes. Has hex string 0000FF.</summary>
        public static Color HighContrastBlue { get; } = CreateFromHex("0000FF");
        /// <summary>A color used in high contrast color schemes, which mirrors Windows's high contrast themes. Has hex string C0C0C0.</summary>
        public static Color HighContrastGray { get; } = CreateFromHex("C0C0C0");
        /// <summary>A color used in high contrast color schemes, which mirrors Windows's high contrast themes. Has hex string 900000.</summary>
        public static Color HighContrastRed { get; } = CreateFromHex("900000"); // 600000

        /// <summary>
        /// Get a color from the X11 Color table (<c>System.Windows.Media.Colors</c>) with a particular name.
        /// </summary>
        /// <param name="colorName">The name of the color to get.</param>
        /// <returns>A Color that corresponds to that name.</returns>
        /// <exception cref="ArgumentException">Thrown if there is no X11 color with that name.</exception>
        /// <remarks>Make sure to enter the name as it appears in the class <c>System.Windows.Media.Colors</c>, with no spaces or alternate spellings.</remarks>
        public static Color GetX11Color(string colorName)
        {
            Type t = typeof(Colors);
            PropertyInfo[] propInfo = t.GetProperties();
            foreach (PropertyInfo p in propInfo)
            {
                if (p.PropertyType == typeof(Color))
                {
                    if (p.Name.ToLowerInvariant() == colorName.ToLowerInvariant())
                    {
                        var c = (Color?) p.GetValue(new Color(), BindingFlags.GetProperty, null, null, null);
                        if (c != null)
                        {
                            return c.Value;
                        }
                        else
                        {
                            throw new ArgumentException("There is no color with this name.", nameof(colorName));
                        }
                    }
                }
            }

            throw new ArgumentException("There is no color with this name.", nameof(colorName));
        }

//#if DEBUG
//        public static List<Color> ListOfColors =
//            new List<Color> { DarkBlue, Blue, Yellow, Orange, Red, SkyBlue, Pink, Green,
//            Cyan, LightGreen, GrayGreen, LightViolet, Violet, Purple,
//            Gray, RedBrown, Salmon, Brown, White, Black, LightGray, DarkGray, Olive};
//#endif

        #endregion

        #region Color Extensions

        /// <summary>
        /// Get a new SolidColorBrush with this color.
        /// </summary>
        public static SolidColorBrush ToBrush(this Color c)
        {
            return new SolidColorBrush(c);
        }

        /// <summary>
        /// Get a hex triplet string for this color. Does not include the hash symbol, nor the alpha value.
        /// </summary>
        public static string GetHexString(this Color c)
        {
            return ToHexString(c);
        }

        /// <summary>
        /// Get a hex string for this color, including the alpha value, as ARGB. Does not include the hash symbol.
        /// </summary>
        public static string GetHexStringWithAlpha(this Color c)
        {
            return ToHexStringWithAlpha(c);
        }

        /// <summary>
        /// Get the color that is the invert of this color. The A (alpha) value of the color is also kept.
        /// </summary>
        public static Color GetInversion(this Color c)
        {
            // based upon answers from https://stackoverflow.com/questions/4801366/convert-rgb-values-to-integer
            // and https://stackoverflow.com/questions/1165107/how-do-i-invert-a-colour
            int rgb = c.R;
            rgb = (rgb << 8) + c.G;
            rgb = (rgb << 8) + c.B;

            int irgb = 0xffffff ^ rgb;

            int r = (irgb >> 16) & 0xFF;
            int g = (irgb >> 8) & 0xFF;
            int b = irgb & 0xFF;

            return Color.FromArgb(c.A, (byte)r, (byte)g, (byte)b);
        }

        #endregion

    }
}
