using System.Drawing;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;
using RMenu.Extensions;

namespace Example;

public partial class Example
{
    private static readonly List<(
        string mapName,
        string mapId,
        bool isLinear,
        int segments,
        int tier,
        float rating
    )> _example6Data =
    [
        ("surf_longhop2_r_ljt_abc", "map_001", true, 5, 1, 4.2f),
        ("surf_skrillcake_r", "map_002", false, 8, 2, 3.8f),
        ("surf_comp_hopblocks_r_ljt_abc", "map_003", true, 3, 1, 4.5f),
        ("surf_synergy_x", "map_004", false, 12, 4, 4.7f),
        ("surf_aztectemple", "map_005", true, 7, 3, 3.9f),
        ("surf_minimountain", "map_006", false, 6, 2, 4.1f),
        ("surf_embrace", "map_007", true, 4, 1, 4.3f),
        ("surf_frozen_go", "map_008", false, 15, 5, 4.8f),
        ("surf_hopbhop", "map_009", true, 2, 1, 3.7f),
        ("surf_matrix_v2", "map_010", false, 10, 3, 4.4f),
        ("surf_nightmare", "map_011", true, 9, 4, 4.6f),
        ("surf_colors", "map_012", false, 5, 2, 3.6f),
        ("surf_factory", "map_013", true, 6, 2, 4.0f),
        ("surf_megabhop", "map_014", false, 20, 6, 4.9f),
        ("surf_toxic", "map_015", true, 8, 3, 4.2f),
    ];

    private void Example6Menu(CCSPlayerController? player, CommandInfo info)
    {
        if (player is null || !player.IsValid)
        {
            return;
        }

        List<MenuObject> header =
        [
            "new",
            new MenuObject("header", new MenuFormat(color: Color.Green)),
        ];

        MenuValue footer = new("kzg", new MenuFormat(new Color().Strobe(Color.Red, Color.Purple)));
        MenuOptions options = new()
        {
            BlockMovement = true,
            DisplayItemsInHeader = true,
            Highlight = new MenuFormat { Color = Color.Green, Style = MenuStyle.Bold },
        };

        MenuBase menu = new(header: header, footer: footer, options: options);

        List<MenuValue> values = [];

        foreach (
            (
                string mapName,
                string mapId,
                bool isLinear,
                int segments,
                int tier,
                float rating
            ) in _example6Data
        )
        {
            MenuValue value = new(mapName);
            List<MenuObject> objects = AppendTail(isLinear, segments, tier, rating);

            value.Objects.AddRange(objects);
            value.Data = mapId;

            values.Add(value);
        }

        menu.Items.Add(new MenuItem(MenuItemType.Choice, values: values));
        menu.Items.Add(new MenuItem(MenuItemType.Spacer));

        foreach (
            (
                string mapName,
                string mapId,
                bool isLinear,
                int segments,
                int tier,
                float rating
            ) in _example6Data
        )
        {
            menu.Items.Add(
                new MenuItem(
                    type: MenuItemType.Button,
                    head: new(mapName),
                    tail: AppendTail(isLinear, segments, tier, rating),
                    data: mapId,
                    options: new MenuItemOptions() { Trim = MenuTrim.Head }
                )
            );
        }

        Menu.Display(player, menu, Example6MenuCallback);
    }

    private void Example6MenuCallback(MenuBase menu, MenuAction menuAction)
    {
        CCSPlayerController player = menu.Player;

        switch (menuAction)
        {
            case MenuAction.Start:
                player.PrintToChat("Menu Start");
                break;

            case MenuAction.Exit:
                player.PrintToChat("Menu Exit");
                break;

            case MenuAction.Select:
                MenuItem? selectedItem = menu.SelectedItem?.Item;

                if (selectedItem is not null)
                {
                    switch (selectedItem.Type)
                    {
                        case MenuItemType.Button:
                            player.PrintToChat($"Selected Map: {selectedItem.Data}");
                            break;

                        case MenuItemType.Choice:
                            MenuValue? selectedValue = selectedItem.SelectedValue?.Value;

                            if (selectedValue is not null)
                            {
                                player.PrintToChat($"Selected Map: {selectedValue.Data}");
                            }

                            break;
                    }

                    if (selectedItem.Type == MenuItemType.Button)
                    {
                        Menu.Clear(player);
                    }
                }

                break;
        }
    }

    private static List<MenuObject> AppendTail(bool isLinear, int segments, int tier, float rating)
    {
        List<MenuObject> objects =
        [
            " ",
            new MenuObject(
                $"{(isLinear ? "L" : $"S{segments}")} ",
                new MenuFormat(isLinear ? Color.DarkOrange : Color.Yellow, canHighlight: false)
            ),
            new MenuObject($"T{tier} ", new MenuFormat(TierToColor(tier), canHighlight: false)),
            new MenuObject(
                $"{rating:0.0}/5",
                new MenuFormat(style: MenuStyle.Italic, canHighlight: false)
            ),
        ];

        return objects;
    }

    private static Color TierToColor(int tier) =>
        tier switch
        {
            1 => new Color().Strobe(Color.LightBlue, Color.Aqua),
            2 => new Color().Strobe(Color.Blue, Color.DodgerBlue),
            3 => new Color().Strobe(Color.Purple, Color.MediumPurple),
            4 => new Color().Strobe(Color.Pink, Color.Magenta),
            5 => new Color().Strobe(Color.DarkRed, Color.Crimson),
            6 => new Color().Strobe(Color.Yellow, Color.Orange),
            _ => Color.White,
        };
}
