namespace LongerBelts
{
    /// <summary>猫猫面板用中英双语文案（构建 UI 时由 CatUIPanel.L10n 按游戏语言选取）。</summary>
    internal sealed class Translate
    {
        public string IfLongerOnGridZh;
        public string IfLongerOnGridEn;
        public string LongitudeDistanceZh;
        public string LongitudeDistanceEn;
        public string LatitudeDistanceZh;
        public string LatitudeDistanceEn;
        public string DistanceSettingZh;
        public string DistanceSettingEn;
        public string DistanceUnitsZh;
        public string DistanceUnitsEn;
        public string SwitchLeftZh;
        public string SwitchLeftEn;
        public string SwitchRightZh;
        public string SwitchRightEn;
        public string PathModeZh;
        public string PathModeEn;
        public string[] PathModeOptionsZh;
        public string[] PathModeOptionsEn;
        public string WarningNoticeZh;
        public string WarningNoticeEn;
        public string UnlimitInstructionZh;
        public string UnlimitInstructionEn;
        public string UnlimitSettingZh;
        public string UnlimitSettingEn;

        public static Translate Create()
        {
            return new Translate
            {
                IfLongerOnGridZh = "更改经纬线模式默认跨度",
                IfLongerOnGridEn = "Change default span in gridline mode",
                LongitudeDistanceZh = "沿经线时跨度",
                LongitudeDistanceEn = "Latitude Span",
                LatitudeDistanceZh = "沿纬线时跨度",
                LatitudeDistanceEn = "Longitude Span",
                DistanceSettingZh = "最大间距设置",
                DistanceSettingEn = "Maximum Spacing (unit: meters, not grid)",
                DistanceUnitsZh = "单位:",
                DistanceUnitsEn = "Units:",
                SwitchLeftZh = "米",
                SwitchLeftEn = "meters",
                SwitchRightZh = "格",
                SwitchRightEn = "Grid",
                PathModeZh = "传送带路径",
                PathModeEn = "Belt Path Mode",
                PathModeOptionsZh = new[]
                {
                    "原版升降逻辑(水平移动一格后每节升降0.5层至目标高度)",
                    "垂直等距分割(端点不水平，常规游戏需先在两端拉好水平带\n建议配合如建筑铺设无条件等放宽传送带铺设条件的功能使用)"
                },
                PathModeOptionsEn = new[]
                {
                    "Original elevating logic (move one grid horizontally, then 0.5 elevation per segment)",
                    "Vertical equidistant (needs horizontal belts at both ends; may need relaxed belt placement mods)"
                },
                WarningNoticeZh = "下列功能慎用!",
                WarningNoticeEn = "Exercise caution when modifying the following settings!",
                UnlimitInstructionZh =
                    "勾选以启用弱约束间距输入框(常规游戏中会出现传送带过短或过长等错误\n即使使用无条件铺设也可能触发特殊bug,造出的蓝图也未必能用)",
                UnlimitInstructionEn =
                    "Check to enable the input below (some values cause belts too short/long in vanilla; blueprints may break)",
                UnlimitSettingZh = "弱约束间距输入框(配合铺设无条件作弊、测试用):",
                UnlimitSettingEn = "Maximum Spacing (less constraints)"
            };
        }
    }
}
