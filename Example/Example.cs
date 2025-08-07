using System.Drawing;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using RMenu;
using RMenu.Enums;
using RMenu.Extensions;

namespace Example;

public class Example : BasePlugin
{
    public override string ModuleName => "Example";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        AddCommand("css_test", "", CommandTest);
    }

    private void CommandTest(CCSPlayerController? player, CommandInfo info)
    {
        if (player is null || !player.IsValid)
        {
            return;
        }

        MenuValue header = ["new", new MenuObject("header", new MenuFormat(Color.Green))];
        MenuValue footer = new("kzg", new Color().Strobe(Color.Red, Color.Purple));
        MenuOptions options = new() { BlockMovement = true, DisplayItemsInHeader = true };

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
            ) in GetSampleMaps()
        )
        {
            MenuValue item = AppendMap(mapName, isLinear, segments, tier, rating);
            menu.Items.Add(new(MenuItemType.Button, item, data: mapId));
        }

        menu.Items.Add(new(MenuItemType.Choice, values: values));
        menu.Items.Add(new(MenuItemType.Spacer));

        foreach (
            (
                string mapName,
                string mapId,
                bool isLinear,
                int segments,
                int tier,
                float rating
            ) in GetSampleMaps()
        )
        {
            MenuValue item = AppendMap(mapName, isLinear, segments, tier, rating);
            menu.Items.Add(new(MenuItemType.Button, item, data: mapId));
        }

        Menu.Display(
            player,
            menu,
            (menu, menuAction) => Console.WriteLine($"{menu.Player.PlayerName} -> {menuAction}")
        );
    }

    private MenuValue AppendMap(string mapName, bool isLinear, int segments, int tier, float rating)
    {
        MenuValue item =
        [
            $"{mapName} ",
            new MenuObject(
                $"{(isLinear ? "L" : $"S{segments}")} ",
                new MenuFormat(isLinear ? Color.DarkOrange : Color.Yellow)
            ),
            new MenuObject($"T{tier} ", new MenuFormat(TierToColor(tier))),
            $"{rating:0.0}/5",
        ];

        /*
        List<MenuObject> item =
        [
            $"{mapName} ",
            new(
                $"{(isLinear ? "L" : $"S{segments}")} ",
                isLinear ? Color.DarkOrange : Color.Yellow
            ),
            new($"T{tier} ", TierToColor(tier)),
            $"{rating:0.0}/5",
        ];
        */

        return item;
    }

    private static List<(
        string mapName,
        string mapId,
        bool isLinear,
        int segments,
        int tier,
        float rating
    )> GetSampleMaps() =>
        [
            ("surf_longhop2", "map_001", true, 5, 1, 4.2f),
            ("surf_skrillcake_r", "map_002", false, 8, 2, 3.8f),
            ("surf_comp_hopblocks", "map_003", true, 3, 1, 4.5f),
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
