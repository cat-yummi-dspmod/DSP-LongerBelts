using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CatAPI;
using CatUIPanel;
using HarmonyLib;

namespace LongerBelts
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(CatAPI.HarnessPlugin.PluginGuid)]
    [BepInDependency(UIPanel.Guid)]
    public class LongerBeltsPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "shisang_LongerBelts";
        public const string PluginName = "LongerBelts";
        public const string PluginVersion = "1.6.1";

        private const float MinGrid = 1.0f;
        private const float MaxGrid = 3.0f;
        private const float MinDistance = 0.400001f;
        private const float MaxDistance = 2.302172f;

        private static readonly Harmony harmony = new Harmony(PluginGuid);
        private static readonly CatAPI.PatchOptions HarnessOptions = new CatAPI.PatchOptions { ModName = PluginName };

        internal static ManualLogSource Log;
        internal static ConfigEntry<bool> LongerOnGrid;
        internal static ConfigEntry<int> IsometricSegmentation;
        internal static ConfigEntry<float> CurrentDistance;
        internal static ConfigEntry<float> LongitudeDistance;
        internal static ConfigEntry<float> LatitudeDistance;
        internal static ConfigEntry<int> DistanceUnits;
        internal static ConfigEntry<bool> ShortestUnlimit;

        private void Awake()
        {
            Instance = this;
            Log = Logger;
            BindConfig();
            LongerBeltsPanel.Register();
            CatAPI.Harness.PatchAll(harmony, typeof(LongerBeltsPatches), HarnessOptions);
            PushRuntimeFromConfig();
            Log.LogInfo($"{PluginName} v{PluginVersion} loaded (CatAPI + CatUIPanel).");
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

        internal static void BindConfig()
        {
            LongerOnGrid = Instance.Config.Bind("更改经纬线模式默认跨度/Change default span in gridline mode", "Enable", false);
            IsometricSegmentation = Instance.Config.Bind("斜坡带模式/Slope Path Mode", "Enable", 0,
                "0=原版升降，1=阿基米德螺线 / 0=vanilla elevation, 1=Archimedean spiral.");
            CurrentDistance = Instance.Config.Bind("最远距离/Maximum Spacing", "Distance", 1.9f);
            LongitudeDistance = Instance.Config.Bind("沿纬线最远距离/Maximum Longitude Distance", "Distance", 1.75f);
            LatitudeDistance = Instance.Config.Bind("沿经线最远距离/Maximum Latitude Distance", "Distance", 1.5f);
            DistanceUnits = Instance.Config.Bind("General", "DistanceUnits", 0,
                "0=meters, 1=grid (equator) / 0=米，1=格(赤道)");
            ShortestUnlimit = Instance.Config.Bind("General", "ShortestUnlimit", false);
        }

        internal static LongerBeltsPlugin Instance { get; private set; }

        internal static void PushRuntimeFromConfig()
        {
            LongerBeltsRuntime.longerOnGrid = LongerOnGrid.Value;
            LongerBeltsRuntime.isometricSegmentation = IsometricSegmentation.Value;
            LongerBeltsRuntime.currentDistance = CurrentDistance.Value;
            LongerBeltsRuntime.longitudeDistance = LongitudeDistance.Value;
            LongerBeltsRuntime.latitudeDistance = LatitudeDistance.Value;
            LongerBeltsRuntime.shortestUnlimit = ShortestUnlimit.Value;
        }
    }
}
