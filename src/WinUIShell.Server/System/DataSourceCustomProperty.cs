using Microsoft.UI.Xaml.Data;

namespace WinUIShell.Server;

internal sealed partial class DataSourceCustomProperty : ICustomProperty
{
    public bool CanRead { get; }
    public bool CanWrite { get; }
    public string Name { get; }
    public Type Type { get; }

    public DataSourceCustomProperty(string name, Type type)
    {
        CanRead = true;
        CanWrite = true;
        Name = name;
        Type = type;
    }

    public object GetIndexedValue(object target, object index)
    {
        throw new NotImplementedException();
    }

    public object GetValue(object target)
    {
        if (target is DataSource dataSource)
        {
            return dataSource.GetMember(Name)!;
        }
        else
        {
            return null!;
        }
    }

    public void SetIndexedValue(object target, object value, object index)
    {
        throw new NotImplementedException();
    }

    public void SetValue(object target, object value)
    {
        if (target is DataSource dataSource)
        {
            dataSource.SetMember(Name, value);
        }
    }
}
