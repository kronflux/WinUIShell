using RpcUIShell.Core;

namespace WinUIShell.Server;

internal sealed class DisabledControlsHolder : IDisabledControlsHolder
{
    private readonly List<Microsoft.UI.Xaml.Controls.Control>? _controls;

    public static IDisabledControlsHolder Create(object?[]? controls)
    {
        return new DisabledControlsHolder(controls);
    }

    private DisabledControlsHolder(object?[]? controls)
    {
        if (controls is null)
            return;

        _controls = [];
        foreach (var obj in controls)
        {
            if (obj is Microsoft.UI.Xaml.Controls.Control control)
            {
                if (control.IsEnabled)
                {
                    _controls.Add(control);
                }
            }
        }
    }

    public void Disable()
    {
        if (_controls is null)
            return;

        foreach (var control in _controls)
        {
            control.IsEnabled = false;
        }
    }

    public void Enable()
    {
        if (_controls is null)
            return;

        foreach (var control in _controls)
        {
            control.IsEnabled = true;
        }
    }
}
