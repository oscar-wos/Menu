using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace RMenu.Helpers;

public static class GameRules
{
    private static CCSGameRules? _gameRules;

    public static void Get()
    {
        _gameRules = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").FirstOrDefault()?.GameRules;
    }

    public static void OnTick()
    {
        if (_gameRules is null)
            return;

        Console.WriteLine(_gameRules?.WarmupPeriod);
    }
}