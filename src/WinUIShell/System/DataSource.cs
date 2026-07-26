using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using RpcUIShell.Core;

namespace WinUIShell;

#pragma warning disable CA1710 // Identifiers should have correct suffix
public class DataSource : DynamicObject, IDictionary<string, object?>, IWinUIShellObject
#pragma warning restore CA1710
{
    private readonly HashSet<string> _memberNames = new(StringComparer.OrdinalIgnoreCase);

    public ObjectId WinUIShellObjectId { get; protected set; } = new();

    public ICollection<string> Keys
    {
        get => [.. _memberNames];
    }

    public ICollection<object?> Values
    {
        get
        {
            List<object?> values = new(_memberNames.Count);
            foreach (string name in _memberNames)
            {
                var value = GetMember(name);
                values.Add(value);
            }
            return values;
        }
    }

    public int Count
    {
        get => _memberNames.Count;
    }

    public bool IsReadOnly
    {
        get => false;
    }

    public object? this[string key]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(key);
            return GetMember(key);
        }
        set => SetMember(key, value);
    }

    public DataSource()
    {
        WinUIShellObjectId = CommandClient.Get().CreateObject(
            "WinUIShell.Server.DataSource, WinUIShell.Server",
            this);
    }

    public DataSource(Hashtable hashtable)
        : this()
    {
        ArgumentNullException.ThrowIfNull(hashtable);

        foreach (DictionaryEntry keyValue in hashtable)
        {
            SetMember((string)keyValue.Key, keyValue.Value);
        }
    }

    public override IEnumerable<string> GetDynamicMemberNames()
    {
        return _memberNames.AsEnumerable();
    }

    public override bool TryGetMember(
        GetMemberBinder binder, out object? result)
    {
        ArgumentNullException.ThrowIfNull(binder);
        return TryGetMember(binder.Name, out result);
    }

    private bool TryGetMember(string memberName, out object? result)
    {
        if (_memberNames.Contains(memberName))
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

    private object? GetMember(string memberName)
    {
        object? result = CommandClient.Get().InvokeMethodAndGetResult<object?>(
            WinUIShellObjectId,
            "WinUIShell.Server.DataSource, WinUIShell.Server",
            "GetMember",
            memberName);

        return result;
    }

    private void SetMember(string memberName, object? value)
    {
        _ = _memberNames.Add(memberName);

        CommandClient.Get().InvokeMethod(
            WinUIShellObjectId,
            "WinUIShell.Server.DataSource, WinUIShell.Server",
            "SetMember",
            memberName,
            value);
    }

    public void Add(string key, object? value)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (ContainsKey(key))
        {
            throw new ArgumentException($"An element with the key '{key}' already exists in the DataSource.");
        }

        SetMember(key, value);
    }

    public bool ContainsKey(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        string memberName = key.ToUpperInvariant();
        return _memberNames.Contains(memberName);
    }

    public bool Remove(string key)
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
    {
        ArgumentNullException.ThrowIfNull(key);
        return TryGetMember(key, out value);
    }

    public void Add(KeyValuePair<string, object?> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(KeyValuePair<string, object?> item)
    {
        if (TryGetValue(item.Key, out object? value))
        {
            if (value is null)
            {
                return item.Value is null;
            }
            return value.Equals(item.Value);
        }
        return false;
    }

    public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
    {
        GetKeyValueList().CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<string, object?> item)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return GetKeyValueList().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private List<KeyValuePair<string, object?>> GetKeyValueList()
    {
        List<KeyValuePair<string, object?>> list = new(_memberNames.Count);
        foreach (string memberName in _memberNames)
        {
            _ = TryGetMember(memberName, out object? value);
            list.Add(new KeyValuePair<string, object?>(memberName, value));
        }
        return list;
    }
}
