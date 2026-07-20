using RpcUIShell.Core;

namespace WinUIShell;

internal static partial class TypeMappingInitializer
{
    public static void Init()
    {
        ObjectTypeMapping.Get().Init(
            ObjectTypeMapping.MappingDirection.ClientToServer,
            "WinUIShell");

        InitEnumTypeMapping();
        InitObjectTypeMapping();
    }
}
