using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EUtility.WinUI.Controls.Files.Triggers;

public sealed class PointerOverTrigger : StateTriggerBase
{
    private UIElement _target;
    public UIElement Target
    {
        get { return _target; }
        set
        {
            _target = value;
            _target.PointerEntered += target_PointerEntered;
            _target.PointerExited += target_PointerExited;
        }
    }

    private void target_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        SetActive(false);
    }

    private void target_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        SetActive(true);
    }
}
