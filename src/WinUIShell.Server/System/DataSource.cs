using System.ComponentModel;
using System.Dynamic;

namespace WinUIShell.Server;

#pragma warning disable CA1515 // Consider making public types internal
public sealed partial class DataSource : DynamicObject, INotifyPropertyChanged
#pragma warning restore CA1515
{
    private readonly Dictionary<string, object?> _members = [];
    public event PropertyChangedEventHandler? PropertyChanged;

    public override bool TryGetMember(
        GetMemberBinder binder, out object? result)
    {
        ArgumentNullException.ThrowIfNull(binder);

        string memberName = binder.Name;
        if (_members.ContainsKey(memberName))
        {
            result = GetMember(memberName);
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

    public override bool TrySetMember(
        SetMemberBinder binder, object? value)
    {
        ArgumentNullException.ThrowIfNull(binder);
        SetMember(binder.Name, value);
        return true;
    }

    public object? GetMember(string memberName)
    {
        return _members[memberName];
    }

    public void SetMember(string memberName, object? value)
    {
        _members[memberName] = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
    }

    public override IEnumerable<string> GetDynamicMemberNames()
    {
        return _members.Keys.AsEnumerable();
    }
}
