﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

namespace SolidShineUi.PropertyList
{
    /// <summary>
    /// Interaction logic for PropertyEditorItem.xaml
    /// </summary>
    public partial class PropertyEditorItem : UserControl
    {
        public PropertyEditorItem()
        {
            InitializeComponent();
        }

#if NETCOREAPP
        public PropertyInfo? PropertyInfo { get; set; } = null;

        object? _value = null;
        public object? PropertyValue { get => _value; set { _value = value; txtValue.Text = (value ?? "(null)").ToString(); } }

        public IPropertyEditor? PropertyEditorControl { get; set; }
#else
        public PropertyInfo PropertyInfo { get; set; } = null;

        object _value = null;
        public object PropertyValue { get => _value; set { _value = value; txtValue.Text = (value ?? "(null)").ToString(); } }

        public IPropertyEditor PropertyEditorControl { get; set; }
#endif

        public string PropertyName { get => txtName.Text; set => txtName.Text = value; }
        public string PropertyType { get => txtType.Text; set => txtType.Text = value; }

#if NETCOREAPP
        public void LoadProperty(PropertyInfo property, object? value, IPropertyEditor? editor)
#else
        public void LoadProperty(PropertyInfo property, object value, IPropertyEditor editor)
#endif
        {
            PropertyInfo = property;
            PropertyName = property.Name;
            PropertyType = property.PropertyType.ToString();
            PropertyValue = value;
            if (editor != null)
            {
                PropertyEditorControl = editor;
                editor.LoadValue(value, property.PropertyType);
                txtValue.Visibility = Visibility.Collapsed;
                UIElement uie = PropertyEditorControl.GetUiElement();
                grdValue.Children.Add(uie);
            }
        }

        public void ApplyPropertyValue(object targetObject)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject), "Targetted object cannot be null");
            if (PropertyInfo == null) throw new InvalidOperationException("The property to set on the object hasn't been defined. Please set the " + nameof(PropertyInfo) + " property.");

            PropertyInfo.SetValue(targetObject, PropertyValue);
        }
    }
}