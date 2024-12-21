using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.StartScreen;

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
            DependencyProperty.Register("DisplayControl", typeof(UIElement), typeof(ControlPane), new(new()));

        public UIElement DisplayControl
        {
            get
            {
                return (UIElement)GetValue(DisplayControlProperty);
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


            UIElement control = DisplayControl;
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
                    box.IsChecked = (bool)type.GetProperty(controlOption.Path).GetValue(DisplayControl);
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
                    box.Text = (string)type.GetProperty(controlOption.Path).GetValue(DisplayControl);
                    setterChange = NumberBox.ValueProperty;
                    optionSetter = box;
                }
                else if(propertyType == typeof(string) || propertyType == typeof(char))
                {
                    TextBox box = new();
                    box.MaxLength = propertyType == typeof(char) ? 1 : int.MaxValue;
                    box.HorizontalAlignment = HorizontalAlignment.Stretch;
                    box.Text = (string)type.GetProperty(controlOption.Path).GetValue(DisplayControl);
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

                bool canChange = false;
                var timer = DispatcherQueue.CreateTimer();

                optionSetter.RegisterPropertyChangedCallback(setterChange, (sender, args) =>
                {
                    try
                    {
                        if (setterChange == TextBox.TextProperty || setterChange == NumberBox.TextProperty)
                        {
                            if (!timer.IsRunning)
                            {
                                timer.Tick += Elapsed(sender, args, controlOption, propertyType, timer);
                                timer.Interval = TimeSpan.FromMilliseconds(800);
                                timer.Start();
                            }
                            else
                            {
                                timer.Stop();
                                timer.Start();
                            }
                            return;
                        }

                        OnChange(sender, args, controlOption, propertyType);

                        void OnChange(DependencyObject sender, DependencyProperty args, ControlOption controlOption, Type propertyType)
                        {
                            if (propertyType.IsEnum)
                            {
                                try
                                {
                                    DisplayControl.GetType().GetProperty(controlOption.Path).SetValue(DisplayControl, Enum.Parse(propertyType, ((sender as ComboBox).Items[(int)sender.GetValue(args)] as ComboBoxItem).Content as string));
                                }
                                catch { }
                                return;
                            }
                            try
                            {
                                DisplayControl.GetType().GetProperty(controlOption.Path).SetValue(DisplayControl, sender.GetValue(args));
                            }
                            catch (Exception e)
                            {
#if DEBUG
                                Debug.WriteLine(e.Message);
                                Debug.WriteLine(e.StackTrace);
                                Debug.WriteLine(e.Source);
#endif
                            }
                        }
                        TypedEventHandler<DispatcherQueueTimer, object> Elapsed(DependencyObject sender, DependencyProperty args, ControlOption controlOption, Type propertyType, DispatcherQueueTimer watch)
                        {
                            return (s, a) =>
                            {
                                OnChange(sender, args, controlOption, propertyType);
                                watch.Stop();
                                watch.Tick -= Elapsed(sender, args, controlOption, propertyType, watch);
                            };
                        }
                    }
                    catch { }
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
