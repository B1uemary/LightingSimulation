using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingExperiment.Object;

namespace LightingExperiment
{
    /// <summary>
    /// 采光计算的各种公式
    /// </summary>
    public static class LightOutput
    {
        //室外无遮挡照度
        public const float OutdoorIllumination = 15000;
        //放侧窗的一边
        public const float RoomWidth = 9;
        public const float RoomLength = 9;
        //顶棚高度
        public const float CeilingHeight = 6;

        //米转为mm
        public const int mmUnit = 1000;

        //发光强度系数
        public const float intensityFactor = 1000;

        //天空顶部亮度
        const float LTop = 5000;

        //顶棚反射比（按PPT上的给）
        const float RC = 0.8f;

        //墙面反射比
        const float RW = 0.5f;

        //地面反射比
        const float RD = 0.2f;

        //顶棚有效光反射比（未解决）
        const float RCC = 0.7f;

        public const float referenceIllumination = 700;
        public const float referenceDaylightFactor = 0.05f;

        /// <summary>
        /// 得到室空间比
        /// </summary>
        /// <param name="h">天窗下沿据参考平面的高度</param>
        /// <param name="l">房间宽度</param>
        /// <param name="b">房间进深</param>
        /// <returns></returns>
        public static float GetRCR(float h, float l, float b)
        {
            return 5 * h * (l + b) / (l * b);
        }

        /// <summary>
        /// 得到顶棚空间比
        /// </summary>
        /// <param name="h">天窗下沿据顶棚的高度</param>
        /// <param name="l">房间长度</param>
        /// <param name="b">房间进深</param>
        /// <returns></returns>
        public static float GetCCR(float h, float l, float b)
        {
            return 5 * h * (l + b) / (l * b);
        }

        //窗地比
        public static float GetWindowGroundRatio(float WindowArea, float GroundArea)
        {
            return WindowArea / GroundArea;
        }

        //采光有效进深, 地面宽度除以窗高？
        public static float GetDepthRatio(float GroundWidth, float WindowHeight)
        {
            if(WindowHeight!=0)
            {
                return GroundWidth / WindowHeight;
            }
            else
            {
                return 0;
            }
        }

        //采光系数
        public static float GetDaylightFactor(float Illumination, float IlluminationOut)
        {
            return Illumination / IlluminationOut;
        }

        //得到某一点的采光系数(侧窗)
        public static float GetPointDaylightFactor(Vector3 PointPosition, Vector3 WindowPosition, float WindowWidth, float WindowHeight)
        {

            float ElevationAngle_1 = Mathf.Atan(((WindowPosition.y + 0.5f * WindowHeight) - PointPosition.y) / (WindowPosition.z - PointPosition.z));

            float ElevationAngle_2 = Mathf.Atan(((WindowPosition.y - 0.5f * WindowHeight) - PointPosition.y) / (WindowPosition.z - PointPosition.z));

            float DirectionAngle_1 = Mathf.Atan(((WindowPosition.x + 0.5f * WindowWidth) - PointPosition.x) / (WindowPosition.z - PointPosition.z));

            float DirectionAngle_2 = Mathf.Atan(((WindowPosition.x - 0.5f * WindowWidth) - PointPosition.x) / (WindowPosition.z - PointPosition.z));

            float Sub = 4.0f * Mathf.Pow(Mathf.Sin(ElevationAngle_2), 3) - 3.0f * Mathf.Pow(Mathf.Cos(ElevationAngle_2), 2) - 4.0f * Mathf.Pow(Mathf.Sin(ElevationAngle_1), 3) + 3.0f * Mathf.Pow(Mathf.Cos(ElevationAngle_1), 2);
            //采光系数
            float DaylightFactor = (DirectionAngle_2 - DirectionAngle_1) * Sub / (14.0f * 3.1416f);

            //这个点的照度
            return DaylightFactor;
        }

        //得到平均采光系数(侧窗)
        public static float GetDaylightFactorAV(float SildWindowArea)
        {
            //常量:
            //采光材料透射比（取的是普通白玻）
            const float a = 0.89f;

            //窗结构挡光折减系数（取的是单层木窗）
            const float b = 0.7f;

            //窗玻璃污染折减系数（一般垂直）
            const float c = 0.75f;

            //没有遮挡物，角度取0.5 PI
            const float d = 1.57f;

            //室内表面总面积
            const float e = 138;

            //室内反射比的加权平均值，需要计算
            const float f = 0.5f;

            return (SildWindowArea * a * b * c * d) / (e * (1 - (f * f)));
        }

        //得到平均采光系数(天窗)
        public static float GetSkylightMean(float SkylightArea)
        {
            //常量
            //采光材料透射比（取的是普通白玻）
            const float a = 0.89f;

            //窗结构挡光折减系数（取的是单层木窗）
            const float b = 0.7f;

            //窗玻璃污染折减系数（一般水平）
            const float c = 0.45f;

            //窗户的总透射比，同上
            float d = a * b * c;
            //利用系数,需要计算
            const float e = 0.8f;

            //地面面积
            const float GroundArea = 48;

            return d * e * (SkylightArea / GroundArea);
        }

        //天窗采光系数的计算，没有设计好
        public static  float GetSkylightPointFactor(Vector3 PointPosition, Vector3 SkylightPosition, float SkylightWidth, float SkylightHeight)
        {
            float ElevationAngle_1 = Mathf.Atan(((SkylightPosition.z + 0.5f * SkylightHeight) - PointPosition.z) / (SkylightPosition.y - PointPosition.y));

            float ElevationAngle_2 = Mathf.Atan(((SkylightPosition.z - 0.5f * SkylightHeight) - PointPosition.z) / (SkylightPosition.y - PointPosition.y));

            float DirectionAngle_1 = Mathf.Atan(((SkylightPosition.x + 0.5f * SkylightWidth) - PointPosition.x) / (SkylightPosition.y - PointPosition.y));

            float DirectionAngle_2 = Mathf.Atan(((SkylightPosition.x - 0.5f * SkylightWidth) - PointPosition.x) / (SkylightPosition.y - PointPosition.y));

            float Sub = 4.0f * Mathf.Pow(Mathf.Sin(ElevationAngle_2), 3) - 3.0f * Mathf.Pow(Mathf.Cos(ElevationAngle_2), 2) - 4.0f * Mathf.Pow(Mathf.Sin(ElevationAngle_1), 3) + 3.0f * Mathf.Pow(Mathf.Cos(ElevationAngle_1), 2);
            //采光系数
            float DaylightFactor = (DirectionAngle_2 - DirectionAngle_1) * Sub / (14.0f * 3.1416f);

            //这个点的照度
            return Mathf.Max(DaylightFactor,0);
        }

        /// <summary>
        /// 天窗逐点采光计算
        /// </summary>
        /// <param name="PointPosition"></param>
        /// <param name="SkylightPosition"></param>
        /// <param name="SkylightWidth"></param>
        /// <param name="SkylightHeight"></param>
        /// <returns></returns>
        public static float GetSkylightPointFactor1(Vector3 PointPosition, Vector3 SkylightPosition, float SkylightWidth, float SkylightHeight)
        {
            float E = 0;
            float R = 0;
            float Y = PointPosition.y;
            float Length = 0.1f;
            float dA = Length * Length;
            float LTop = 233;
            float L = 12;
            Vector3 firstPosition = new Vector3(SkylightPosition.x - 0.5f * SkylightWidth, Y, SkylightPosition.z - 0.5f * SkylightHeight);
            int widthCount = (int)(SkylightWidth / 0.1f);
            int heightCount = (int)(SkylightHeight / 0.1f);
            for(int i = 0; i < widthCount; i++)
            {
                for(int j = 0; j < heightCount; j++)
                {
                    R = Vector3.Distance(PointPosition, new Vector3((firstPosition.x + Length * i), firstPosition.y, firstPosition.z + Length * j));
                    float cos = Y / R;
                    L = (1 + 2 * cos) / 3 * LTop;
                    E = E + L * dA * cos / (R * R) * cos; 
                }
            }

            //这个点的照度
            return E/ OutdoorIllumination;
        }

        public static float GetAngle(float a, float b, float c)
        {
            float CosC = (a * a + b * b - c * c) / (2 * a * b);
            return Mathf.Acos(CosC);
        }

        public static float GetSkylightPointFactor2(Vector3 PointPosition, Vector3 WindowPosition, float WindowWidth, float WindowHeight)
        {
            //分别计算9条棱的长度;
            Vector3 A = new Vector3((WindowPosition.x - 0.5f * WindowWidth), WindowPosition.y, (WindowPosition.z - 0.5f * WindowHeight));
            Vector3 B = new Vector3((WindowPosition.x + 0.5f * WindowWidth), WindowPosition.y, (WindowPosition.z - 0.5f * WindowHeight));
            Vector3 C = new Vector3((WindowPosition.x + 0.5f * WindowWidth), WindowPosition.y, (WindowPosition.z + 0.5f * WindowHeight));
            Vector3 D = new Vector3((WindowPosition.x - 0.5f * WindowWidth), WindowPosition.y, (WindowPosition.z + 0.5f * WindowHeight));

            float OA = Vector3.Distance(PointPosition, A);
            float OB = Vector3.Distance(PointPosition, B);
            float OC = Vector3.Distance(PointPosition, C);
            float OD = Vector3.Distance(PointPosition, D);
            float AB = Vector3.Distance(A, B);
            float BC = Vector3.Distance(B, C);
            float CD = Vector3.Distance(C, D);
            float AD = Vector3.Distance(A, D);
            float AC = Vector3.Distance(A, C);

            float AOB = GetAngle(OA, OB, AB);
            float BOC = GetAngle(OB, OC, BC);
            float AOC = GetAngle(OA, OC, AC);

            float AOD = GetAngle(OA, OD, AD);
            float COD = GetAngle(OC, OD, CD);

            float s1 = 0.5f * (AOB + BOC + AOC);
            float s2 = 0.5f * (AOD + COD + AOC);
            float Omiga1 = Mathf.Atan(Mathf.Sqrt(Mathf.Tan(s1 / 2) * Mathf.Tan(s1 / 2 - AOB / 2) * Mathf.Tan(s1 / 2 - BOC / 2) * Mathf.Tan(s1 / 2 - AOC / 2))) * 4;
            float Omiga2 = Mathf.Atan(Mathf.Sqrt(Mathf.Tan(s2 / 2) * Mathf.Tan(s2 / 2 - AOD / 2) * Mathf.Tan(s2 / 2 - COD / 2) * Mathf.Tan(s2 / 2 - AOC / 2))) * 4;
            float Omiga = Omiga1 + Omiga2;

            float Y = WindowPosition.y - PointPosition.y;
            float R = Vector3.Distance(WindowPosition, PointPosition);
            float cos = Y / R;
            float L = 0.3333f * (1 + 2 * cos) * LTop;
            return L * Omiga * cos/ OutdoorIllumination;
        }

        /// <summary>
        /// 采光均匀度
        /// </summary>
        /// <param name="Min">最小采光系数</param>
        /// <param name="Mean">最大采光系数</param>
        /// <returns></returns>
        public static float GetUniformity(float Min, float Mean)
        {
            return Min / Mean;
        }


        //计算一个灯光对一点的照度
        public static float GetPointIllumination(Vector3 pointPosition, Vector3 LampPosition, Vector3 LampAngle, float LampIntensity, float LampSpotAngle, char fixedAxis)
        {
            float cos1 = 0;
            float currentlampanglecos = 0;
            float r = Vector3.Distance(pointPosition, LampPosition);
            Vector3 length = pointPosition - (Vector3)(LampPosition);
            //得到内积
            float dotProduct = Vector3.Dot(length, LampAngle);
            //得到余弦值
            float lampanglecos = dotProduct / r;
            if (lampanglecos < 0)
            {
                lampanglecos = 0;
            }
            currentlampanglecos = Mathf.Cos((Mathf.Acos(lampanglecos)) + ((90 - (LampSpotAngle / 1.5f)) / 180) * Mathf.PI);
            if (currentlampanglecos < 0)
            {
                currentlampanglecos = 0;
            }

            //currentlampanglecos = lampanglecos * lampanglecos * lampanglecos * lampanglecos;
            //得到光束角余弦值
            //float LampSpotAngleCos = Mathf.Cos((LampSpotAngle[i] / 180) * Mathf.PI);
            //LampSpotAngleCos = LampSpotAngleCos * LampSpotAngleCos * LampSpotAngleCos * LampSpotAngleCos;
            //if (lampanglecos > (Mathf.Cos((LampSpotAngle[i] / 360) * Mathf.PI)))
            //{
            //    currentlampanglecos = lampanglecos;
            //}
            //公式所需余弦值
            if (fixedAxis == 'x')
            {
                cos1 = (LampPosition.x - pointPosition.x) / r;
            }
            else if (fixedAxis == 'y')
            {
                cos1 = (LampPosition.y - pointPosition.y) / r;
            }
            else if (fixedAxis == 'z')
            {
                cos1 = (LampPosition.z - pointPosition.z) / r;
            }
            //灯的坐标可能小于展品坐标，导致得到负值，从而需要取绝对值
            cos1 = Mathf.Abs(cos1);

            //照度
            //currentIntensity = LampIntensity[i] * 2;
            //float DaylightFactor = (LampIntensity[i] * cos1 * currentlampanglecos) / (r * r* LampSpotAngleCos);
            float DaylightFactor = (LampIntensity * cos1 * currentlampanglecos) / (r * r);
            //返回这个点的照度
            return DaylightFactor;
        }
    }

}
