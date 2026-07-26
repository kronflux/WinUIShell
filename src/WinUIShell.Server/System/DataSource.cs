using System.ComponentModel;
using Microsoft.UI.Xaml.Data;

namespace WinUIShell.Server;

#pragma warning disable CA1515 // Consider making public types internal
public sealed partial class DataSource : INotifyPropertyChanged, ICustomPropertyProvider
#pragma warning restore CA1515
{
    // Make binding case-insensitive as variable names in PowerShell are case-insensitive.
    private readonly Dictionary<string, object?> _members = new(StringComparer.OrdinalIgnoreCase);

    public event PropertyChangedEventHandler? PropertyChanged;
    public Type Type { get => GetType(); }

    public object? GetMember(string memberName)
    {
        ArgumentNullException.ThrowIfNull(memberName);
        if (_members.TryGetValue(memberName, out object? value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public void SetMember(string memberName, object? value)
    {
        ArgumentNullException.ThrowIfNull(memberName);
        _members[memberName] = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
    }

    public ICustomProperty GetCustomProperty(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (_members.TryGetValue(name, out object? value))
        {
            return new DataSourceCustomProperty(name, value?.GetType() ?? typeof(object));
        }
        else
        {
            return new DataSourceCustomProperty(name, typeof(object));
        }
    }

    public ICustomProperty GetIndexedProperty(string name, Type type)
    {
        return null!;
    }

    public string GetStringRepresentation()
    {
        return ToString()!;
    }
}
