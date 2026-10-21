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
            Translate t = Translate.Create();
            UIPanel.RegisterPage(PageKey, "加长传送带", "Longer Belts")
                .AddCheckbox(LongerBeltsPlugin.LongerOnGrid, t.IfLongerOnGridZh, t.IfLongerOnGridEn)
                .AddSlider(LongerBeltsPlugin.LongitudeDistance, MinGrid, MaxGrid, t.LongitudeDistanceZh, t.LongitudeDistanceEn, 3)
                .AddSlider(LongerBeltsPlugin.LatitudeDistance, MinGrid, MaxGrid, t.LatitudeDistanceZh, t.LatitudeDistanceEn, 3)
                .AddSlider(LongerBeltsPlugin.CurrentDistance, MinDistance, MaxDistance, t.DistanceSettingZh, t.DistanceSettingEn, 6)
                .AddSwitch(LongerBeltsPlugin.DistanceUnits, t.DistanceUnitsZh, t.DistanceUnitsEn,
                    t.SwitchLeftZh, t.SwitchLeftEn, t.SwitchRightZh, t.SwitchRightEn)
                .AddSelection(LongerBeltsPlugin.IsometricSegmentation, t.PathModeZh, t.PathModeEn,
                    t.PathModeOptionsZh, t.PathModeOptionsEn)
                .AddLabel(t.WarningNoticeZh, t.WarningNoticeEn)
                .AddCheckbox(LongerBeltsPlugin.ShortestUnlimit, t.UnlimitInstructionZh, t.UnlimitInstructionEn)
                .AddFloatField(LongerBeltsPlugin.CurrentDistance, LooseMin, LooseMax, t.UnlimitSettingZh, t.UnlimitSettingEn, 6);

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
