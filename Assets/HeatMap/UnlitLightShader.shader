Shader "Unlit/UnlitLightShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

		_BlueColor("BlueColor", Color) = (0,0,1,1)

		_GreenColor("GreenColor", Color) = (0,1,0,1)

		_RedColor("RedColor", Color) = (1,0,0,1)

		_Range("Range" ,Range(0,2)) = 0.05

		_Intensity("Intensity" ,float) = 8.0

		_LightIntensityFactor("LightIntensityFactor",float) = 1

		E("E",float) = 0.5

	    _Mode("Mode",int) = 0


	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"



			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : WORLDPOSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;


			float Position;


			fixed4 _BlueColor;
			fixed4 _GreenColor;
			fixed4 _RedColor;

			//侧窗的参数
			float4 _WindowPositionArray[10];
			float _WindowWidthArray[10];
			float _WindowHeightArray[10];
			int _WindowsCount;
			
			//天窗的参数
			float4 _SkylightPositionArray[10];
			float _SkylightWidthArray[10];
			float _SkylightHeightArray[10];
			int _SkylightsCount;

			//顶灯的参数
			int _LampCount;
			float _LampIntensity[100];
			float _LampSpotAngle[100];
			float3 _LampAngleArray[100];
			float3 _LampPositionArray[100];
			int _FixedAxis;

			float _Range;
			float _Intensity;
			float _LightIntensityFactor;

			float _PointLightFactor;
			float E;
			int _Mode;




			float GetSideWindowFactor(float3 PointPosition, float3 WindowPosition, float WindowWidth, float WindowHeight)
			{

				float ElevationAngle_1 = atan(((WindowPosition.y + 0.5f * WindowHeight) - PointPosition.y) / (WindowPosition.z - PointPosition.z));

				float ElevationAngle_2 = atan(((WindowPosition.y - 0.5f * WindowHeight) - PointPosition.y) / (WindowPosition.z - PointPosition.z));


				float DirectionAngle_1 = atan(((WindowPosition.x + 0.5f * WindowWidth) - PointPosition.x) / (WindowPosition.z - PointPosition.z));

				float DirectionAngle_2 = atan(((WindowPosition.x - 0.5f * WindowWidth) - PointPosition.x) / (WindowPosition.z - PointPosition.z));

				float Sub = 4.0 * pow(sin(ElevationAngle_2), 3) - 3.0 * pow(cos(ElevationAngle_2), 2) - 4.0 * pow(sin(ElevationAngle_1), 3) + 3.0 * pow(cos(ElevationAngle_1), 2);
				//采光系数
				float DaylightFactor = (DirectionAngle_2 - DirectionAngle_1)*Sub / (14.0* 3.1416);


				//这个点的照度
				return  DaylightFactor;

			}

			float EOutDoor = 15000;
			float Length = 0.1;
			float LTop = 15000;

			float GetSkylightIllumination(float3 PointPosition, float3 WindowPosition, float WindowWidth, float WindowHeight)
			{

				float E = 0;
				//float Y = WindowPosition.y - PointPosition.y;
				//float A = Length * Length;
				//float3 firstPosition = float3(WindowPosition.x - 0.5 * WindowWidth, WindowPosition.y, WindowPosition.z - 0.5 * WindowHeight);
				//int widthCount = floor(WindowWidth / Length);
				//int heightCount = floor(WindowHeight / Length);

				//for (int i = 0; i < widthCount; i++)
				//{
				//	for (int j = 0; j < heightCount; j++)
				//	{
				//		float3 WindowDotPosition = float3((firstPosition.x + i * Length), (firstPosition.y), (firstPosition.z + j * Length));
				//        float R = distance(WindowDotPosition, PointPosition);
				//		float cos = Y / R;
				//		float L = 0.3333 * (1 + 2 * cos) * LTop;
				//		E = E + LTop * A * cos * cos / (R * R) ;
				//	}
				//}

				float3 firstPosition = float3(WindowPosition.x - 0.5 * WindowWidth, WindowPosition.y, WindowPosition.z);
				fixed widthCount = WindowWidth / Length;
				fixed i = 0;
				for (i = 0; i < widthCount; i++)
				{
				//	float3 WindowDotPosition = float3((firstPosition.x + i * Length), firstPosition.y, firstPosition.z);
				//	float R = distance(WindowDotPosition, PointPosition);
				//    float cos = (WindowPosition.y - PointPosition.y) / R;
				//    float L = 0.3333 * (1 + 2 * cos) * LTop;
			    	E = E + 0.1 ;
				}



				return E ;
			}


			float GetAngle(float a, float b, float c)
			{
				float CosC = (a * a + b * b - c * c) / (2 * a * b);
				return acos(CosC);
			}

			float GetSkylightFactor2(float3 PointPosition, float3 WindowPosition, float WindowWidth, float WindowHeight)
			{
				if (WindowWidth <= 0)
				{
					return 0;
				}
				float3 A = float3((WindowPosition.x - 0.5 * WindowWidth), WindowPosition.y, (WindowPosition.z - 0.5*WindowHeight));
				float3 B = float3((WindowPosition.x + 0.5 * WindowWidth), WindowPosition.y, (WindowPosition.z - 0.5*WindowHeight));
				float3 C = float3((WindowPosition.x + 0.5 * WindowWidth), WindowPosition.y, (WindowPosition.z + 0.5*WindowHeight));
				float3 D = float3((WindowPosition.x - 0.5 * WindowWidth), WindowPosition.y, (WindowPosition.z + 0.5*WindowHeight));

				//分别计算9条棱的长度;
				float OA = distance(PointPosition, A);
				float OB = distance(PointPosition, B);
				float OC = distance(PointPosition, C);
				float OD = distance(PointPosition, D);
				float AB = distance(A, B);
				float BC = distance(B, C);
				float CD = distance(C, D);
				float AD = distance(A, D);
				float AC = distance(A, C);

				float AOB = GetAngle(OA, OB, AB);
				float BOC = GetAngle(OB, OC, BC);
				float AOC = GetAngle(OA, OC, AC);

				float AOD = GetAngle(OA, OD, AD);
				float COD = GetAngle(OC, OD, CD);

				float s1 = 0.5 * (AOB + BOC + AOC);
				float s2 = 0.5 * (AOD + COD + AOC);
				float Omiga1 = atan(sqrt(tan(s1 / 2)*tan(s1 / 2 - AOB / 2)*tan(s1 / 2 - BOC / 2)*tan(s1 / 2 - AOC / 2))) * 4;
				float Omiga2 = atan(sqrt(tan(s2 / 2)*tan(s2 / 2 - AOD / 2)*tan(s2 / 2 - COD / 2)*tan(s2 / 2 - AOC / 2))) * 4;
				float Omiga = Omiga1 + Omiga2;

				float LTop = 200;
				float Y = WindowPosition.y - PointPosition.y;
				float R = distance(WindowPosition, PointPosition);
				float cos = Y / R;
				float L = 0.3333 * (1 + 2 * cos) * LTop;

				return 0.3*Omiga*cos;

			}

			//灯光的点照度计算
			float GetLightIllumination(float3 PointPosition, float3 LampPosition, float3 LampAngle, float LampIntensity, float LampSpotAngle, int FixedAxis)
			{

				float r = distance(PointPosition, LampPosition);
				//得到shader上任一点与灯具两点间向量
				float3 length = PointPosition - LampPosition;
				//计算内积
				float dotProduct = dot(length, LampAngle);
				//得到余弦值
				float lampanglecos = dotProduct / r;
				//若两向量间余弦值为负值，置0				
				if (lampanglecos < 0) {
					lampanglecos = 0;
				}
				//根据光束角修改灯具不同角度上的衰减系数，角度每减小一度，衰减系数自乘一次
				float LampSpotAngleCos = cos(radians(LampSpotAngle));




				float currentlampanglecos = cos((acos(lampanglecos)) + (radians(90 - (LampSpotAngle / 1.5))));
				if (currentlampanglecos < 0) {
					currentlampanglecos = 0;
				}
				//计算照度公式中所使用的余弦系数
				float  cos1 = (LampPosition.y - PointPosition.y) / r;
				if (FixedAxis == 0) {
					cos1 = (LampPosition.x - PointPosition.x) / r;
				}
				else if (FixedAxis == 1) {
					cos1 = (LampPosition.y - PointPosition.y) / r;
				}
				else if (FixedAxis == 2) {
					cos1 = (LampPosition.z - PointPosition.z) / r;
				}
				cos1 = abs(cos1);

				//照度
				float DaylightFactor = (LampIntensity*cos1)*currentlampanglecos / (r*r);
				//这个点的照度
				return  DaylightFactor;

			}


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
		
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				//col(_Color, 1);
				Position = i.vertex;
				fixed Factor = 0;
				fixed Offset = 0;
				int Index = 0;
				for (Index = 0; Index < _WindowsCount; Index++)
				{
					Factor = Factor + GetSideWindowFactor(i.worldPos, _WindowPositionArray[Index], _WindowWidthArray[Index], _WindowHeightArray[Index]);
				}

				for (Index = 0; Index < _SkylightsCount; Index++)
				{
					Factor = Factor + GetSkylightFactor2(i.worldPos, _SkylightPositionArray[Index], _SkylightWidthArray[Index], _SkylightHeightArray[Index]);
				}

				for (Index = 0; Index < _LampCount; Index++)
				{
					Factor = Factor + GetLightIllumination(i.worldPos, _LampPositionArray[Index], _LampAngleArray[Index], _LampIntensity[Index], _LampSpotAngle[Index], _FixedAxis);
				}

				//得到等系数线
				if (_Range != 0)
				{					
					// 后处理
					if (_Mode == 0)//线性增强
					{
						Offset = fmod(Factor, _Range);


						Factor = Factor - Offset;
					}
					else if (_Mode == 1)
					{
						Factor = pow(Factor, E);
						Offset = fmod(Factor, _Range);


						Factor = Factor - Offset;


					}
					else if (_Mode == 2)
					{
						Offset = fmod(Factor, _Range);
						Factor = Factor - Offset;
						Factor = clamp(Factor, 0, 1);
						Factor = pow(Factor, E);
					}
					else if (_Mode = 3)
					{
						Factor = pow(Factor, E);
						Offset = fmod(Factor, _Range);
						Factor = Factor - Offset;
						Factor = clamp(Factor, 0, 1);
						Factor = pow(Factor, 1.0/E);
					}

				}
				
				fixed4 ColorPoint; //= lerp(_ColdColor, _Color, clamp(Factor, 0, 1.5)* _Intensity);

				fixed lerpFactor = clamp(Factor * _Intensity, 0, 1);

				if (lerpFactor < 0.5) 
				{
					ColorPoint = lerp(_BlueColor, _GreenColor, 2 * lerpFactor);
				}
				else
				{
					ColorPoint = lerp(_GreenColor, _RedColor, 2 * lerpFactor - 1);
				}

				return ColorPoint;
			}
			
			ENDCG
		}
	}
}
