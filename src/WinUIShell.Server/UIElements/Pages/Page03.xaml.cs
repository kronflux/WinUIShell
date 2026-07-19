using Microsoft.UI.Xaml.Controls;
using RpcUIShell.Core;

namespace WinUIShell.Server;

public partial class Page03 : Page, IPage
{
    public ObjectId Id { get; set; } = new();

    public Page03()
    {
        InitializeComponent();
        IPage.Init(this);
    }
}
