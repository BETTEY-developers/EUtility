using EUtility.Foundation;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EUtility.WinUI.Helpers;

public struct ResourceItem
{
    public string Name { get; set; }
    public TypedDelegate<bool, ResourceItem, string> CheckProc { get; set; }
    public TypedDelegate<Void, ResourceItem, string> Fallback { get; set; }
}

public class ResourceChecker
{
    private Dictionary<string, ResourceItem> _resourceDictionary = new();

    public Dictionary<string, ResourceItem> ResourceDictionary
    {
        get => _resourceDictionary;
        set => _resourceDictionary = value;
    }

    public void CheckResources()
    {
        foreach(var item in _resourceDictionary)
        {
            if(!item.Value.CheckProc(item.Value, item.Key))
                item.Value.Fallback(item.Value, item.Key);
        }
    }

    public void AddResource(
        string str, 
        TypedDelegate<bool, ResourceItem, string> CheckProc,
        TypedDelegate<Void, ResourceItem, string> Fallback)
    {
        _resourceDictionary.Add(str, new ResourceItem {  Name = str, CheckProc = CheckProc, Fallback = Fallback });
    }

    public void RemoveResource(string str)
    {
        _resourceDictionary.Remove(str);
    }
}
