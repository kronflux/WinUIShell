using RpcUIShell.Core;

namespace WinUIShell.Server;

internal static partial class TypeMappingInitializer
{
    public static void Init()
    {
        ObjectTypeMapping.Get().Init(
            ObjectTypeMapping.MappingDirection.ServerToClient,
            "WinUIShell");

        InitEnumTypeMapping();
        InitObjectTypeMapping();
    }
}
