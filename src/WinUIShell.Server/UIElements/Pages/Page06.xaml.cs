using Microsoft.UI.Xaml.Controls;
using RpcUIShell.Core;

namespace WinUIShell.Server;

public partial class Page06 : Page, IPage
{
    public ObjectId Id { get; set; } = new();

    public Page06()
    {
        InitializeComponent();
        IPage.Init(this);
    }
}
