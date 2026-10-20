using UnityEngine;

namespace LongerBelts
{
    public static class LongerBeltsLogic
    {        public static int LongerSnapLineNonAlloc(PlanetGrid planetGrid , Vector3 begin, Vector3 end, int path, Vector3[] snaps, out int swerveIndex)
        {
			if (LongerBeltsRuntime.longerOnGrid)
			{
                swerveIndex = 0;
                int num1 = snaps.Length - 10;//snaps.Length在某个地方初始化的时候是160,不知道这里过程中有没有改,即snaps最多生成160个,num1=150
                if (num1 <= 0)
                    return 0;
                begin.Normalize();
                end.Normalize();
                float f1 = Mathf.Asin(begin.y);//起始点纬度90°S~90°N对应[-π/2,π/2]
                float num2 = Mathf.Atan2(begin.x, -begin.z);//起始点经度arctan(-x/z),范围[-π,π]
                float num3 = Mathf.Asin(end.y);//终止点纬度
                float num4 = Mathf.Atan2(end.x, -end.z);//终止点经度
                float f2 = Mathf.Repeat(num4 - num2, 6.283185f);//f2 = num4-num2±2kπ,f2∈[0,2π]，始末端点的经度跨度
                float heigherLatitude = Mathf.Max(Mathf.Abs(f1), Mathf.Abs(num3));
                Vector3 beginNormalized = begin.normalized;
                Vector3 endNormalized = end.normalized;
                int count = 0;
                snaps[count++] = beginNormalized;
                if ((double)f2 > 3.14159274101257)
                    f2 -= 6.283185f;//f2 = num4-num2±2kπ,f2∈[-π,π]
                if (path == 1)//先沿纬线走,再沿经线走
                {
                    float longitudeSegmentCount = (float)PlanetGrid.DetermineLongitudeSegmentCount(Mathf.FloorToInt(Mathf.Max(0.0f, Mathf.Abs(f1 / 6.283185f * (float)planetGrid.segment) - 0.1f)), planetGrid.segment);//起始点纬度的经线分割数/5
                    if ((Mathf.Abs(f2) - 1.57079637f) * longitudeSegmentCount > (1.57079637f - heigherLatitude) * 200)
				    {
                        if (f2 > 0)
                            f2 -= 3.141593f;//f2∈[-π,π]
					    else
                            f2 += 3.141593f;//f2∈[-π,π]
                        num3 = (double)num3 < 0.0 ? -3.141593f - num3 : 3.141593f - num3;//num3∈[-π,-π/2]∪[π/2,π]意义:2*π/2-num3,意思就是对着90°的极点翻转到另一端
                    }
                    float longitude1 = num2 + f2;//目标点的假想经度
                    float f3 = num3 - f1;//目标点纬度跨度
                    int longitudeSnaps = (int)(Mathf.Abs(f2) / 1.2566370614357f * longitudeSegmentCount / LongerBeltsRuntime.latitudeDistance) + 1;//沿纬线总步数
                    if(Mathf.Abs(f2) < 1e-4)
					{
                        longitudeSnaps = 0;
					}
                    LongerBeltsRuntime.test = longitudeSegmentCount;
                    int latitudeSnaps = (int)(Mathf.Abs(f3) / 0.0062831853f / LongerBeltsRuntime.longitudeDistance) + 1;//沿经线总步数
                    float num8 = Mathf.Cos(f1);//起始点纬线的半径
                    for (int index = 1; index <= longitudeSnaps; ++index)//沿纬线走,根据经度生成坐标
                    {
                        float t = f2 * (float)index / (float)longitudeSnaps;
                        snaps[count++] = new Vector3(num8 * Mathf.Sin(num2 + t), Mathf.Sin(f1), -num8 * Mathf.Cos(num2 + t));
                    }
                    swerveIndex = count;
                    Vector3 midNormalized = new Vector3(num8 * Mathf.Sin(longitude1), Mathf.Sin(f1), -num8 * Mathf.Cos(longitude1));//中间点
                    if((endNormalized- midNormalized).magnitude < 0.00251f)//跨度小于0.4格
				    {
                        return count;
                    }
                    for (int index = 1; index <= latitudeSnaps; ++index)//沿经线走,直接插值
                    {
                        float t = (float)index / (float)latitudeSnaps;
                        snaps[count++] = Vector3.Slerp(midNormalized, endNormalized, t).normalized;
                    }
                    return count;
                }
                else//先沿经线走,再沿纬线走
                {
                    float longitudeSegmentCount = (float)PlanetGrid.DetermineLongitudeSegmentCount(Mathf.FloorToInt(Mathf.Max(0.0f, Mathf.Abs(num3 / 6.283185f * (float)planetGrid.segment) - 0.1f)), planetGrid.segment);//终止点维度的经线分割数/5
                    if ((Mathf.Abs(f2) - 1.57079637f) * longitudeSegmentCount > (1.57079637f - heigherLatitude) * 200)
                    {
                        if (f2 > 0)
                        {
                            f2 -= 3.141593f;//f2∈[2-π,2]
                        }
                        else
                        {
                            f2 += 3.141593f;//f2∈[2-π,2]
                        }
                        num3 = (double)num3 < 0.0 ? -3.141593f - num3 : 3.141593f - num3;//num3∈[-π,π]意义:2*π/2-num3,意思就是对着90°的极点翻转到另一端
                    }
                    float f3 = num3 - f1;//目标点纬度跨度
                    int longitudeSnaps = (int)(Mathf.Abs(f2) / 1.2566370614357f * longitudeSegmentCount / LongerBeltsRuntime.latitudeDistance) + 1;//沿纬线总步数
                    int latitudeSnaps = (int)(Mathf.Abs(f3) / 0.0062831853f / LongerBeltsRuntime.longitudeDistance) + 1;//沿经线总步数
                    if (Mathf.Abs(f3) < 1e-4)
                    {
                        latitudeSnaps = 0;
                    }
                    float num8 = Mathf.Cos(num3);//目标点纬线的半径(这里实际上是负的)
                    Vector3 midNormalized = new Vector3(num8 * Mathf.Sin(num2), Mathf.Sin(num3), -num8 * Mathf.Cos(num2));//中间点
                    for (int index = 1; index <= latitudeSnaps; ++index)//沿经线走,直接插值
                    {
                        float t = (float)index / (float)latitudeSnaps;
                        snaps[count++] = Vector3.Slerp(beginNormalized, midNormalized, t).normalized;
                    }
                    swerveIndex = count;
                    if (Mathf.Abs(f2) / 1.2566370614357f * longitudeSegmentCount < 0.4f)//跨度小于0.4格
                    {
                        return count;
                    }
                    for (int index = 1; index <= longitudeSnaps; ++index)//沿纬线走,根据经度生成坐标
                    {
                        float t = num2 + ((float)index / (float)longitudeSnaps) * f2;
                        snaps[count++] = new Vector3(num8 * Mathf.Sin(t), Mathf.Sin(num3), -num8 * Mathf.Cos(t));//num3可能>90°,此时num8是负数,sin(num3) = sin(180°-num3)
                    }
                    return count;
                }
			}
			else
			{
                return planetGrid.SnapLineNonAlloc(begin, end, path, snaps, out swerveIndex);
			}
        }
        public static bool LongerGeodesic(Vector3 begin, Vector3 end, ref int counts, Vector3[] snaps)
        {
            int maxSnapsLength = 150;
            float max_radius = begin.magnitude > end.magnitude ? begin.magnitude : end.magnitude;
            Vector3 beginNormalized = begin.normalized;
            Vector3 endNormalized = end.normalized;
            float nodes_counts;
            float geodesic_distance = Mathf.Acos(Mathf.Clamp(Vector3.Dot(beginNormalized, endNormalized), -1, 1)) * max_radius;
            int num10;
            if (LongerBeltsRuntime.isometricSegmentation == 1)//阿基米德螺线模式
            {
                float beginMagnitude = begin.magnitude;
                float endMagnitude = end.magnitude;
                float delta_height = Mathf.Abs(endMagnitude - beginMagnitude);//起止点高度差
                nodes_counts = Mathf.Sqrt(geodesic_distance * geodesic_distance + delta_height * delta_height) / LongerBeltsRuntime.currentDistance;
                if (!LongerBeltsRuntime.shortestUnlimit && Mathf.Sqrt(geodesic_distance * geodesic_distance + delta_height * delta_height) / ((int)nodes_counts + 1) < LongerBeltsRuntime.minimum_distance)//实际建造时传送带距离过短判定是<=0.4m
                {
                    nodes_counts = Mathf.Sqrt(geodesic_distance * geodesic_distance + delta_height * delta_height) / LongerBeltsRuntime.minimum_distance - 1;
                }
                num10 = nodes_counts > 0.1 ? (int)nodes_counts + 1 : 0;
                if (num10 == 0)
                {
                    snaps[counts++] = beginNormalized;
                    if ((double)Mathf.Abs(beginMagnitude - endMagnitude) > 1.0 / 1000.0)
                    {
                        snaps[counts++] = snaps[0] * endMagnitude;
                        return false;
                    }
                }
                else
                {
                    for (int index = 0; index <= num10 && counts < maxSnapsLength; ++index)
                    {
                        float t = (float)index / (float)num10;
                        snaps[counts++] = Vector3.Slerp(beginNormalized, endNormalized, t).normalized;
                    }
                }
                for (int index = 0; index < counts; ++index)
                {
                    snaps[index] *= beginMagnitude * (float)(counts - 1 - index) / (float)(counts - 1) + endMagnitude * (float)index / (float)(counts - 1);
                }
                return false;
            }
            nodes_counts = geodesic_distance / LongerBeltsRuntime.currentDistance;//类似原版逻辑(只是间隔变长了)
            if (!LongerBeltsRuntime.shortestUnlimit && Mathf.Acos(Vector3.Dot(beginNormalized, endNormalized)) * (begin.magnitude < max_radius ? begin.magnitude : end.magnitude) / ((int)nodes_counts + 1) < LongerBeltsRuntime.minimum_distance)//实际建造时传送带距离过短判定是<=0.4m
            {
                nodes_counts = (Mathf.Acos(Vector3.Dot(beginNormalized, endNormalized)) * (begin.magnitude < max_radius ? begin.magnitude : end.magnitude) / LongerBeltsRuntime.minimum_distance) - 1;
            }
            num10 = nodes_counts > 0.1 ? (int)nodes_counts + 1 : 0;
            if (num10 == 0)
            {
                snaps[counts++] = beginNormalized;
            }
            else
            {
                for (int index = 0; index <= num10 && counts < maxSnapsLength; ++index)
                {
                    float t = (float)index / (float)num10;
                    snaps[counts++] = Vector3.Slerp(beginNormalized, endNormalized, t).normalized;
                }
            }
            return true;
        }
		public static void SwitchBeltsPath(BuildTool_Path path)
        {
            if (path.pathSuggest > 0)
                path.geodesic = false;
			if (path.pathAlternative == 1)
                path.pathAlternative = 2;
			else
			{
                path.pathAlternative = 1;
                path.geodesic = false;
			}
        }

        public static void SlantVerticalBelts(Vector3 begin,ref Vector3 end)
		{
            if ((begin.normalized - end.normalized).magnitude < 0.0001f)
            {
                // 新模式垂直带水平距离过近调整,保证垂直带坡度<1000,使垂直传送带美观
                Vector3 littleOffset = new Vector3(0, 0.001f, 0);
                if (Mathf.Abs(Vector3.Dot(begin.normalized, littleOffset.normalized)) > 0.99f)//如果接近极点就改对赤道上一点叉乘
                {
                    littleOffset = new Vector3(0.001f, 0, 0);
                }
                littleOffset = Vector3.Cross(end - begin, littleOffset);//横向偏置=纵向跨度叉乘一个模长为0.001的向量(通常指向北极),相当于保证坡度不大于1000
                end += littleOffset;
            }
        }    }
}
