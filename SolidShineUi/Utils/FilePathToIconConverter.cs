﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SolidShineUi.Utils
{
    public class FilePathToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
#if NETCOREAPP
            if (value is IEnumerable<string> ie)
            {
                string first = ie.FirstOrDefault() ?? "";
                return GetImageFromFilePath(first);
            }
            else if (value is string s)
            {
                return GetImageFromFilePath(s);
            }
            else
            {
                return null!;
            }
#else
            if (value is IEnumerable<string> ie)
            {
                string first = ie.FirstOrDefault() ?? "";
                return GetImageFromFilePath(first);
            }
            else if (value is string s)
            {
                return GetImageFromFilePath(s);
            }
            else
            {
                return null;
            }
#endif
        }

        private static BitmapSource GetImageFromFilePath(string path)
        {
            return NativeMethods.GetSmallIcon(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
