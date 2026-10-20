namespace LongerBelts
{
    /// <summary>运行时状态，与 BepInEx 配置同步（与原 LongerBelts 静态字段一致）。</summary>
    public static class LongerBeltsRuntime
    {
        public static bool shortestUnlimit;
        public static float currentDistance = 1.9f;
        public static float longitudeDistance = 1.75f;
        public static float latitudeDistance = 1.5f;
        public static int isometricSegmentation;
        public static bool longerOnGrid;
        public static float minimum_distance = 0.400001f;
        public static float test;
    }
}
