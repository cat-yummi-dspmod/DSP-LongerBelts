using CatUIPanel;

namespace LongerBelts
{
    internal static class LongerBeltsPanel
    {
        public const string PageKey = "longer_belts";

        private const float MinGrid = 1.0f;
        private const float MaxGrid = 3.0f;
        private const float MinDistance = 0.400001f;
        private const float MaxDistance = 2.302172f;
        private const float LooseMin = 0.001f;
        private const float LooseMax = 999f;

        public static void Register()
        {
            Translate t = Translate.NewTexture();
            UIPanel.RegisterPage(PageKey, "加长传送带", "Longer Belts")
                .AddCheckbox(LongerBeltsPlugin.LongerOnGrid, t.ifLongerOnGrid, t.ifLongerOnGrid)
                .AddSlider(LongerBeltsPlugin.LongitudeDistance, MinGrid, MaxGrid, t.longitudeDistance, t.longitudeDistance, 3)
                .AddSlider(LongerBeltsPlugin.LatitudeDistance, MinGrid, MaxGrid, t.latitudeDistance, t.latitudeDistance, 3)
                .AddSlider(LongerBeltsPlugin.CurrentDistance, MinDistance, MaxDistance, t.distance_setting, t.distance_setting, 6)
                .AddSelection(LongerBeltsPlugin.DistanceUnits, t.distance_units, t.distance_units, t.distance_unitsStrings, t.distance_unitsStrings)
                .AddSelection(LongerBeltsPlugin.IsometricSegmentation, t.pathMode, t.pathMode, t.pathModeStrings, t.pathModeStrings)
                .AddLabel(t.WarningNotice, t.WarningNotice)
                .AddCheckbox(LongerBeltsPlugin.ShortestUnlimit, t.unlimit_distance_instruction, t.unlimit_distance_instruction)
                .AddFloatField(LongerBeltsPlugin.CurrentDistance, LooseMin, LooseMax, t.unlimit_distance_setting, t.unlimit_distance_setting, 6);

            HookConfigSync();
        }

        private static void HookConfigSync()
        {
            LongerBeltsPlugin.LongerOnGrid.SettingChanged += (_, __) => LongerBeltsPlugin.PushRuntimeFromConfig();
            LongerBeltsPlugin.IsometricSegmentation.SettingChanged += (_, __) => LongerBeltsPlugin.PushRuntimeFromConfig();
            LongerBeltsPlugin.CurrentDistance.SettingChanged += (_, __) => LongerBeltsPlugin.PushRuntimeFromConfig();
            LongerBeltsPlugin.LongitudeDistance.SettingChanged += (_, __) => LongerBeltsPlugin.PushRuntimeFromConfig();
            LongerBeltsPlugin.LatitudeDistance.SettingChanged += (_, __) => LongerBeltsPlugin.PushRuntimeFromConfig();
            LongerBeltsPlugin.ShortestUnlimit.SettingChanged += (_, __) => LongerBeltsPlugin.PushRuntimeFromConfig();
            LongerBeltsPlugin.DistanceUnits.SettingChanged += (_, __) => LongerBeltsPlugin.PushRuntimeFromConfig();
        }
    }
}
