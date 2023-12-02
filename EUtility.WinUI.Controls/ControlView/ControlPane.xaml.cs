using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EUtility.WinUI.Controls.ControlView
{
    public class ControlOption : DependencyObject
    {
        public static DependencyProperty PathProperty
            = DependencyProperty.Register("Path", typeof(string), typeof(ControlOption), null);

        public static DependencyProperty DisplayNameProperty
            = DependencyProperty.Register("DisplayName", typeof(string), typeof(ControlOption), null);

        public string Path
        {
            get
            {
                return (string)GetValue(PathProperty);
            }
            set
            {
                SetValue(PathProperty, value);
            }
        }

        public string DisplayName
        {
            get
            {
                return (string)GetValue(DisplayNameProperty);
            }
            set
            {
                SetValue(DisplayNameProperty, value);
            }
        }
    }

    public sealed partial class ControlPane : UserControl
    {
        public ControlPane()
        {
            this.InitializeComponent();
        }

        public static DependencyProperty DisplayControlProperty =
            DependencyProperty.Register("DisplayControl", typeof(Control), typeof(ControlPane), new(new()));

        public Control DisplayControl
        {
            get
            {
                return (Control)GetValue(DisplayControlProperty);
            }
            set
            { 
                SetValue(DisplayControlProperty, value); 
            }
        }

        public DependencyProperty OptionsProperty =
            DependencyProperty.Register("Options", typeof(DependencyObjectCollection), typeof(ControlPane), new(new DependencyObjectCollection()));

        public DependencyObjectCollection Options
        {
            get
            {
                return (DependencyObjectCollection)GetValue(OptionsProperty);
            }
            set
            {
                SetValue(OptionsProperty, value);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Type[] Number = new[]
            {
                typeof(int),
                typeof(long),
                typeof(short),
                typeof(byte),
                typeof(uint),
                typeof(ulong),
                typeof(ushort),
                typeof(nint),
                typeof(nuint),
                typeof(double),
                typeof(float),
                typeof(decimal)
            };
                

            Control control = DisplayControl;
            ControlArea.Children.Add(DisplayControl);

            Type type = control.GetType();

            foreach(var option in Options)
            {
                ControlOption controlOption = option as ControlOption;
                StackPanel optionPreseter = new();

                Control optionSetter = default;
                DependencyProperty setterChange = default;

                if(type.GetProperty(controlOption.Path).PropertyType != typeof(bool))
                {
                    optionPreseter.Children.Add(new TextBlock() { Text = controlOption.DisplayName });
                }
                else
                {
                    CheckBox box = new();
                    box.Content = new TextBlock() { Text = controlOption.DisplayName };
                    setterChange = CheckBox.IsCheckedProperty;
                    optionSetter = box;
                }

                Type propertyType = type.GetProperty(controlOption.Path).PropertyType;

                if (Number.Contains(type.GetProperty(controlOption.Path).PropertyType))
                {
                    NumberBox box = new NumberBox();
                    box.Maximum = (double)type.GetProperty(controlOption.Path).PropertyType.GetField("MaxValue").GetRawConstantValue();
                    box.Minimum = (double)type.GetProperty(controlOption.Path).PropertyType.GetField("MinValue").GetRawConstantValue();
                    box.SmallChange = 1;
                    box.LargeChange = 1;
                    box.HorizontalAlignment = HorizontalAlignment.Stretch;
                    setterChange = NumberBox.ValueProperty;
                    optionSetter = box;
                }
                else if(propertyType == typeof(string) || propertyType == typeof(char))
                {
                    TextBox box = new();
                    box.MaxLength = propertyType == typeof(char) ? 1 : int.MaxValue;
                    box.HorizontalAlignment = HorizontalAlignment.Stretch;
                    setterChange = TextBox.TextProperty;
                    optionSetter = box;
                }
                else if (propertyType.IsEnum)
                {
                    ComboBox box = new();
                    var names = Enum.GetNames(propertyType);
                    foreach(var name in names)
                    {
                        box.Items.Add(new ComboBoxItem() { Content = name });
                    }

                    box.HorizontalAlignment = HorizontalAlignment.Stretch;
                    setterChange = ComboBox.SelectedIndexProperty;
                    optionSetter = box;
                }

                optionSetter.RegisterPropertyChangedCallback(setterChange, (sender, args) =>
                {
                    DisplayControl.GetType().GetProperty(controlOption.Path).SetValue(DisplayControl, sender.GetValue(args));
                });

                optionPreseter.Children.Add(optionSetter);
                optionPreseter.Spacing = 5.0;
                optionPreseter.HorizontalAlignment = HorizontalAlignment.Center;
                optionPreseter.VerticalAlignment = VerticalAlignment.Top;

                ListViewItem lvi = new();
                lvi.HorizontalContentAlignment = HorizontalAlignment.Left;
                lvi.Content = optionPreseter;

                OptionsList.Items.Add(lvi);
            }
        }
    }
}
