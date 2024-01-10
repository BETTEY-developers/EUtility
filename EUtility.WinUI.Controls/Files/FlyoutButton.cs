using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EUtility.WinUI.Controls.Files;

public sealed class FlyoutButton : ButtonBase
{
    public FlyoutButton()
    {
        this.DefaultStyleKey = typeof(FlyoutButton);
    }

    public FlyoutBase Flyout
    {
        get { return (FlyoutBase)GetValue(FlyoutProperty); }
        set { SetValue(FlyoutProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Flyout.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FlyoutProperty =
        DependencyProperty.Register("Flyout", typeof(FlyoutBase), typeof(FlyoutButton), new PropertyMetadata(new()));

    protected override void OnPointerEntered(PointerRoutedEventArgs e)
    {
        VisualStateManager.GoToState(this, "PointerEntered", true);
        base.OnPointerEntered(e);
    }

    protected override void OnPointerExited(PointerRoutedEventArgs e)
    {
        VisualStateManager.GoToState(this, "PointerExited", true);
        base.OnPointerExited(e);
    }

    protected override void OnPointerPressed(PointerRoutedEventArgs e)
    {
        VisualStateManager.GoToState(this, "PointerPressed", true);
        base.OnPointerPressed(e);
    }

    protected override void OnPointerReleased(PointerRoutedEventArgs e)
    {
        VisualStateManager.GoToState(this, "PointerReleased", true);
        base.OnPointerReleased(e);
    }
}
