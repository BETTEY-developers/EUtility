using EUtility.WinUI.Controls.Files;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EUtility.WinUI.Controls.Helpers;

public partial class EnumRadio<T> : DependencyObject
    where T : Enum
{

    private static List<DependencyObject> _inited = new();

    public static DependencyProperty TargetProperty =
        DependencyProperty.RegisterAttached("Target", typeof(T), typeof(RadioMenuFlyoutItem), new(new(), OnPropertyChanged));

    public static DependencyProperty TriggerValueProperty =
        DependencyProperty.RegisterAttached("TriggerValue", typeof(int), typeof(RadioMenuFlyoutItem), new(new()));
    public static void SetTarget(DependencyObject obj, T target)
    {
        if(_inited.Contains(obj))
        {
            if (GetTarget(obj).Equals(target))
            {
                return;
            }
        }
        else
        {
            _inited.Add(obj);
        }
        obj.SetValue(TargetProperty, target);
    }

    public static T GetTarget(DependencyObject obj)
    {
        return (T)obj.GetValue(TargetProperty);
    }

    public static void SetTriggerValue(DependencyObject obj, int triggerValue)
    {
        obj.SetValue(TriggerValueProperty, triggerValue);

        (obj as RadioMenuFlyoutItem).RegisterPropertyChangedCallback(RadioMenuFlyoutItem.IsCheckedProperty, (s, e) =>
        {
            var r = s as RadioMenuFlyoutItem;
            if ((bool)r.Tag && !r.IsChecked )
            {
                (s as Control).Tag = false;
                return;
            }
            SetTarget(s, (T)Enum.Parse(typeof(T), Enum.GetName(typeof(T), triggerValue)));
        });
    }

    public static int GetTriggerValue(DependencyObject obj)
    {
        return (int)obj.GetValue(TriggerValueProperty);
    }

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        RadioMenuFlyoutItem item = d as RadioMenuFlyoutItem;
        item.Tag = true;
        item.IsChecked = Convert.ToInt32(GetTarget(item)) == GetTriggerValue(item);
    }
}