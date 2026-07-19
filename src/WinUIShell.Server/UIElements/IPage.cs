using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RpcUIShell.Core;

namespace WinUIShell.Server;

public interface IPage
{
    ObjectId Id { get; set; }

    static void Init<TPage>(TPage page) where TPage : Page, IPage
    {
        ArgumentNullException.ThrowIfNull(page);

        PageStore.Get().RegisterPageInstance(page);
        var pageProperty = PageStore.Get().GetPageProperty(page.GetType());

        page.NavigationCacheMode = pageProperty.NavigationCacheMode;
        page.Loaded += CreateOnLoaded(page);
    }

    static void Term<TPage>(TPage page) where TPage : IPage
    {
        ArgumentNullException.ThrowIfNull(page);

        var pageProperty = PageStore.Get().GetPageProperty(page.GetType());
        var queueId = EventCallback.s_binder.GetProcessingQueueId(
            pageProperty.OnLoadedCallbackRunspaceMode,
            pageProperty.OnLoadedCallbackMainRunspaceId);

        CommandClient.Get().DestroyObject(queueId, page.Id);
        page.Id = new();
    }

    private static RoutedEventHandler CreateOnLoaded<TPage>(TPage page) where TPage : Page, IPage
    {
        return async (object sender, RoutedEventArgs eventArgs) =>
        {
            var pageProperty = PageStore.Get().GetPageProperty(typeof(TPage));

            var temporaryQueueId = CommandClient.Get().CreateTemporaryQueueId();
            var processingQueueId = EventCallback.s_binder.GetProcessingQueueId(
                pageProperty.OnLoadedCallbackRunspaceMode,
                pageProperty.OnLoadedCallbackMainRunspaceId);

            page.Id = CommandClient.Get().CreateObjectWithId(
                temporaryQueueId,
                "WinUIShell.Microsoft.UI.Xaml.Controls.Page, WinUIShell",
                page);

            var eventArgsId = CommandClient.Get().CreateObjectWithId(
                temporaryQueueId,
                "WinUIShell.Microsoft.UI.Xaml.RoutedEventArgs, WinUIShell",
                eventArgs);

            var invokeTask = CommandClient.Get().InvokeStaticMethodWaitAsync(
                temporaryQueueId,
                "WinUIShell.PageStore, WinUIShell",
                "OnLoaded",
                pageProperty.Name,
                page.Id,
                eventArgsId);

            CommandClient.Get().ProcessTemporaryQueue(processingQueueId, temporaryQueueId);

            await EventCallback.s_binder.WaitEventCallbackAsync(pageProperty.OnLoadedCallbackRunspaceMode, invokeTask);

            CommandClient.Get().DestroyObject(processingQueueId, eventArgsId);
        };
    }
}
