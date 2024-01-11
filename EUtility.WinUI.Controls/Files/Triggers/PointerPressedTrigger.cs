using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EUtility.WinUI.Controls.Files.Triggers;

public sealed class PointerPressedTrigger : StateTriggerBase
{
    private UIElement _target;
    public UIElement Target
    {
        get { return _target; }
        set
        {
            _target = value;
            _target.PointerPressed += target_PointerPressed; ;
            _target.PointerReleased += target_PointerReleased;
        }
    }

    private void target_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        SetActive(false);
    }

    private void target_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        SetActive(true);
    }
}
