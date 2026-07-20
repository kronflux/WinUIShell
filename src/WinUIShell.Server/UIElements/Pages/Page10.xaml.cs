using Microsoft.UI.Xaml.Controls;
using RpcUIShell.Core;

namespace WinUIShell.Server;

public partial class Page10 : Page, IPage
{
    public ObjectId Id { get; set; } = new();

    public Page10()
    {
        InitializeComponent();
        IPage.Init(this);
    }
}
