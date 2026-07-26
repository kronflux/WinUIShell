# This example is based on the WinUI Basic data binding tutorial.
# https://learn.microsoft.com/en-us/windows/apps/develop/data-binding/data-binding-overview

using namespace WinUIShell
using namespace WinUIShell.Microsoft.UI.Windowing
using namespace WinUIShell.Microsoft.UI.Xaml
using namespace WinUIShell.Microsoft.UI.Xaml.Controls
using namespace WinUIShell.Microsoft.UI.Xaml.Markup

if (-not (Get-Module WinUIShell)) {
    Import-Module WinUIShell
}

$xamlString = @'
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d">

    <Grid RowDefinitions="*,Auto">
        <ListView x:Name="ListView"
            ItemsSource="{Binding Recordings}"
            Grid.Row="0"
            HorizontalAlignment="Center"
            VerticalAlignment="Center">

            <ListView.ItemTemplate>
                <DataTemplate>
                    <StackPanel Orientation="Horizontal" Margin="6">
                        <SymbolIcon Symbol="Audio" Margin="0,0,12,0"/>
                        <StackPanel>
                            <TextBlock Text="{Binding ArtistName}" FontWeight="Bold"/>
                            <TextBlock Text="{Binding CompositionName}"/>
                        </StackPanel>
                    </StackPanel>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
        <Button x:Name="Button"
            Grid.Row="1"
            Margin="4"
            HorizontalAlignment="Stretch"
            HorizontalContentAlignment="Center"
            Style="{StaticResource AccentButtonStyle}"
            Content="Remove" />
    </Grid>

</Window>
'@

$win = [XamlReader]::Load($xamlString)
$win.AppWindow.ResizeClient(600, 400)

$rootGrid = $win.Content
$listView = $rootGrid.FindName('ListView')
$button = $rootGrid.FindName('Button')

# DataSource is the only type that supports data binding.
$data = [DataSource]::new()
# Use ObservableCollection to reflect the item removal or addition to the UI (Two-way binding).
$data.Recordings = [WinUIShell.System.Collections.ObjectModel.ObservableCollection[DataSource]]::new()

# DataSource supports dynamic property generation similar to PSCustomObject.
$items = @(
    [DataSource]@{
        ArtistName = 'Johann Sebastian Bach'
        CompositionName = 'Mass in B minor'
    },
    [DataSource]@{
        ArtistName = 'Ludwig van Beethoven'
        CompositionName = 'Third Symphony'
    },
    [DataSource]@{
        ArtistName = 'George Frideric Handel'
        CompositionName = 'Serse'
    }
)

$items | ForEach-Object {
    $data.Recordings.Add($_)
}

# Bind data.
$rootGrid.DataContext = $data

$button.AddClick({
        $data.Recordings.Remove($listView.SelectedItem)
    })

$win.Activate()
$win.WaitForClosed()
