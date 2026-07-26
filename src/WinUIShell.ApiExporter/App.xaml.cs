using Microsoft.UI.Xaml;

namespace WinUIShell.ApiExporter;

#pragma warning disable CA1515 // Consider making public types internal
public partial class App : Application
{
#pragma warning restore CA1515
    public App()
    {
        Export();
    }

    private void Export()
    {
        string[] arguments = Environment.GetCommandLineArgs();
        if (arguments.Length != 2)
        {
            throw new ArgumentException("Specify a path to the output Api.xml file.");
        }

        string apiFilePath = arguments[1];
        var exporter = new RpcUIShell.Core.ApiExporter(
            "WinUIShell.ApiExporter",
            "WinUIShell.Server");

        var api = exporter.Api;

        api.ModuleName = "WinUIShell";
        api.ServerName = "WinUIShell.Server";

        api.UnsupportedNamespaces =
        [
            "System.Linq.Expressions",
        ];

        api.UnsupportedTypes =
        [
            "System.IntPtr",
            "WinRT.IWinRTObject",
            "WinRT.IObjectReference",
            "WinRT.ObjectReference",
        ];

        api.SupportedGlobalSystemInterfaces =
        [
            "System.IDisposable",
            "System.Collections.Generic.ICollection",
            "System.Collections.Generic.IList",
            "System.Collections.IEnumerable",
            "System.Collections.Generic.IEnumerable",
            "System.Collections.IEnumerator",
            "System.Collections.Generic.IEnumerator",
            "System.Collections.Generic.IReadOnlyList",
            "System.Collections.Generic.IReadOnlyCollection",
        ];

        api.EmulatedSystemInterfaces =
        [
            "System.Collections.Generic.IDictionary",
            "System.Collections.IComparer",
            "System.Collections.IList",
            "System.Collections.ICollection",
        ];

        api.UnsupportedMethodNames =
        [
            "Equals",
            "GetHashCode",
            "GetType",
        ];

        exporter.AddTypesInAssembly(typeof(Microsoft.UI.Xaml.Controls.BackgroundSizing)); // Microsoft.WinUI
        exporter.AddTypesInAssembly(typeof(Microsoft.UI.Windowing.CompactOverlaySize)); // Microsoft.InteractiveExperiences.Projection
        exporter.AddTypesInAssembly(typeof(Windows.UI.Text.FontStretch)); // Microsoft.Windows.SDK.NET
        exporter.AddTypesInAssembly(typeof(Microsoft.Windows.Storage.Pickers.FileOpenPicker)); // Microsoft.Windows.Storage.Pickers.Projection
        exporter.AddTypeMapping(typeof(Server.DataSource));
        exporter.Export(apiFilePath);

        Exit();
    }
}
