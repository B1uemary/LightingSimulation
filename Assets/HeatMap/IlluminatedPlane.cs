using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using LightingExperiment.Chart;
using UnityEngine.Events;


namespace LightingExperiment
{
	namespace Object
	{
		public class IlluminatedPlane : MonoBehaviour
		{
			//用于不同网格显示
			[System.Serializable]
			public struct GridDisplay
			{
				public int RowCount;
				public int ColumnCount;
				public float GridDistance;
				public Vector3 FirstPointPosition;
				GridDisplay (int rowCount = 5, int columnCount = 5, float gridDistance = 1, Vector3 firstPointPosition = new Vector3 ())
				{
					RowCount = rowCount;
					ColumnCount = columnCount;
					GridDistance = gridDistance;
					FirstPointPosition = firstPointPosition;
				}
			}
			//侧窗的数组,这种形式有利于放入shader中
			[HideInInspector]
			public Vector4 [] sideWindowPositionArray = new Vector4 [10];
			[HideInInspector]
			public float [] sideWindowWidthArray = new float [10];
			[HideInInspector]
			public float [] sideWindowHeightArray = new float [10];
			[HideInInspector]
			public int sideWindowsCount = 0;

			////天窗的数组
			//[HideInInspector]
			//public Vector4[] skylightPositionArray = new Vector4[10];
			//[HideInInspector]
			//public float[] skylightWidthArray = new float[10];
			//[HideInInspector]
			//public float[] skylightHeightArray = new float[10];
			//[HideInInspector]
			//public int skylightsCount = 0;

			//[Header("ceilLight 管理器")]
			//public LightManager lightManager;
			[Header ("BoardLight List")]
			public List<Light> boardLights;
			//定义改变shader的所需属性
			//自身偏转角向量
			private Vector4 [] LampAngleArray = new Vector4 [300];
			//灯具位置
			private Vector4 [] LampPositionArray = new Vector4 [300];
			//灯光亮度
			private float [] LampIntensity = new float [300];
			//光束角
			private float [] LampSpotAngle = new float [300];
			private int LampCount;


			[Header ("网格显示照度")]
			public GameObject LabelPerfab;
			public List<GridDisplay> gridDisplays;
			public Dropdown dropdown;
			private List<GameObject> Labels = new List<GameObject> ();
			private int RowCount;
			private int ColumnCount;
			private Vector3 FirstPointPosition;
			private float GridDistance = 1;

			[Space (10)]
			[HideInInspector]
			public float DaylightFactorMean = 0;//采光系数平均值

			public float illuminationMean = 0;//照度平均值
			[HideInInspector]
			public float DaylightFactorMin = 0;//采光系数最小值

			[Header ("立面图表输出")]
			[HideInInspector]
			public int [] StaticGraphValues = new int [10];
			public Vector3 firstPosition;
			public float StepLength = 0.4f;

			[Header ("数据输出")]
			//public DataOutput dataOutput;
			//public ChartUpdate chartUpdate;
			[HideInInspector]
			public float PercentPart1 = 0;
			[HideInInspector]
			public float PercentPart2 = 0;

			public UnityEvent onValueChange;//照度图改变时触发事件

			//平均照度
			private Material PlaneMaterial;
			//照度值或者采光系数
			private bool firstMode = true;
			// Use this for initialization
			void Start ()
			{
				PlaneMaterial = this.transform.GetComponent<Renderer> ().material;
				//初始化照度图上的标签
				SwitchLable ();
				InitialLabels ();
			}

			//更新参数接口
			public void UpdatePlanePara ()
			{
				if (PlaneMaterial != null) {
					//侧窗
					PlaneMaterial.SetVectorArray ("_WindowPositionArray", sideWindowPositionArray);
					PlaneMaterial.SetFloatArray ("_WindowWidthArray", sideWindowWidthArray);
					PlaneMaterial.SetFloatArray ("_WindowHeightArray", sideWindowHeightArray);
					PlaneMaterial.SetInt ("_WindowsCount", sideWindowsCount);
					//天窗
					//PlaneMaterial.SetVectorArray("_SkylightPositionArray", skylightPositionArray);
					//PlaneMaterial.SetFloatArray("_SkylightWidthArray", skylightWidthArray);
					//PlaneMaterial.SetFloatArray("_SkylightHeightArray", skylightHeightArray);
					//PlaneMaterial.SetInt("_SkylightsCount", skylightsCount);

					if (firstMode) {
						////顶灯
						//List<ceilingLight> ceilingLight = lightManager.ceilingLights;
						//LampCount = ceilingLight.Count;
						//MyDebug.Add("顶灯的灯光个数：" + LampCount);
						////得到顶灯的各属性
						//for (int i = 0; i < ceilingLight.Count; i++)
						//{
						//    LampAngleArray[i] = ceilingLight[i].transform.GetChild(0).transform.forward.normalized;
						//    LampPositionArray[i] = ceilingLight[i].transform.position;
						//    //shader中的公式是计算照度的，除以这个系数就可以转换为对应的采光系数，因为shader是根据采光系数显示的。
						//    LampIntensity[i] = ceilingLight[i].Intensity/LightOutput.OutdoorIllumination;
						//    LampSpotAngle[i] = ceilingLight[i].transform.GetChild(0).GetComponent<Light>().spotAngle;
						//}

						//黑板灯

						for (int i = 0; i < boardLights.Count; i++) {
							LampAngleArray [LampCount + i] = boardLights [i].transform.forward.normalized;
							LampPositionArray [LampCount + i] = boardLights [i].transform.position;
							LampIntensity [LampCount + i] = boardLights [i].intensity / LightOutput.OutdoorIllumination;
							LampSpotAngle [LampCount + i] = boardLights [i].transform.GetComponent<Light> ().spotAngle;
						}
						//LampCount = ceilingLight.Count + boardLights.Count;
					} else {
						LampCount = 0;
					}

					PlaneMaterial.SetVectorArray ("_LampAngleArray", LampAngleArray);
					PlaneMaterial.SetVectorArray ("_LampPositionArray", LampPositionArray);
					PlaneMaterial.SetFloatArray ("_LampIntensity", LampIntensity);//传入的变量是spotlight的intensity乘以了一个系数
					PlaneMaterial.SetFloatArray ("_LampSpotAngle", LampSpotAngle);
					PlaneMaterial.SetInt ("_LampCount", LampCount);
					PlaneMaterial.SetInt ("_FixedAxis", 1);
				}


				UpdateLabels ();
				//dataOutpuut.UpdateData ();
			}

			//更换标签 2*2或其它
			public void SwitchLable ()
			{
				switch (dropdown.value) {
				case 0:
					RowCount = gridDisplays [0].RowCount;
					ColumnCount = gridDisplays [0].ColumnCount;
					GridDistance = gridDisplays [0].GridDistance;
					FirstPointPosition = gridDisplays [0].FirstPointPosition;
					break;
				case 1:
					RowCount = gridDisplays [1].RowCount;
					ColumnCount = gridDisplays [1].ColumnCount;
					GridDistance = gridDisplays [1].GridDistance;
					FirstPointPosition = gridDisplays [1].FirstPointPosition;
					break;
				case 2:
					RowCount = gridDisplays [2].RowCount;
					ColumnCount = gridDisplays [2].ColumnCount;
					GridDistance = gridDisplays [2].GridDistance;
					FirstPointPosition = gridDisplays [2].FirstPointPosition;
					break;
				default:
					break;
				}
				InitialLabels ();
				UpdateLabels ();
			}

			//更改照度值或者采光系数
			public void SwitchMode (bool mode)
			{
				firstMode = mode;
				UpdatePlanePara ();
			}

			//更新标签数值
			public void UpdateLabels ()
			{
				GameObject CurrentLabel;
				float Illumination = 0;
				//各种照度计数
				int TotalCount = 0;
				int CountPart1 = 0;
				int CountPart2 = 0;

				DaylightFactorMean = 0;
				illuminationMean = 0;
				DaylightFactorMin = 15000;
				float Factor = 0;
				int count = 0;

				//网络
				for (int i = 0; i < RowCount; i++) {
					for (int j = 0; j < ColumnCount; j++) {
						Factor = 0;//采光系数
						Illumination = 0;//照度值
						CurrentLabel = Labels [i * ColumnCount + j];
						for (int Index = 0; Index < sideWindowsCount; Index++) {
							Factor = Factor + LightOutput.GetPointDaylightFactor (CurrentLabel.transform.position, sideWindowPositionArray [Index], sideWindowWidthArray [Index], sideWindowHeightArray [Index]);
						}
						//for (int Index = 0; Index < skylightsCount; Index++)
						//{
						//    Factor = Factor + LightOutput.GetSkylightPointFactor2(CurrentLabel.transform.position, skylightPositionArray[Index], skylightWidthArray[Index], skylightHeightArray[Index]);
						//}
						for (int Index = 0; Index < LampCount; Index++) {
							//LampIntensity的数组是不对的，因为shader限制除了一个室外照度来模仿采光系数，需要乘以室外照度
							Illumination += LightOutput.GetPointIllumination (CurrentLabel.transform.position, LampPositionArray [Index], LampAngleArray [Index], LampIntensity [Index] * LightOutput.OutdoorIllumination, LampSpotAngle [Index], 'y');
						}

						Illumination += Factor * LightOutput.OutdoorIllumination;//总照度等于灯的照度加上采光照度 
						count++;
						//DaylightFactorMean = 0.5f * (DaylightFactorMean + Factor);//计算平均采光系数
						DaylightFactorMean = DaylightFactorMean + (Factor - DaylightFactorMean) / count;
						//illuminationMean = 0.5f * (illuminationMean + Illumination);//计算平均照度
						illuminationMean = illuminationMean + (Illumination - illuminationMean) / count;
						if (Factor < DaylightFactorMin) {
							DaylightFactorMin = Factor;//计算最小照度
						}

						//计算各个部分占的百分比
						TotalCount++;
						if (Illumination < 200) {
							CountPart1++;
						} else if (Illumination < 500) {
							CountPart2++;
						}
						if (firstMode) {
							CurrentLabel.GetComponentInChildren<Text> ().text = ((int)Illumination).ToString ();
						} else {
							CurrentLabel.GetComponentInChildren<Text> ().text = Factor.ToString ("#0.##");
						}

					}

				}

				PercentPart1 = CountPart1 * 1.0f / TotalCount * 100;
				PercentPart2 = CountPart2 * 1.0f / TotalCount * 100;

				Illumination = 0;
				//折线图
				//dataOutput.ClearStaticGragh ();
				//chartUpdate.ClearStaticGragh ();//清空第二张折线图
				for (int j = 0; j < sideWindowsCount; j++) {
					for (int i = 0; i < 25; i++) {
						Factor = 0;
						for (int Index = 0; Index < sideWindowsCount; Index++) {
							Factor = Factor + LightOutput.GetPointDaylightFactor (new Vector3 (sideWindowPositionArray [j].x, firstPosition.y, firstPosition.z) + new Vector3 (0, 0, i * StepLength), sideWindowPositionArray [Index], sideWindowWidthArray [Index], sideWindowHeightArray [Index]);
						}
						//for (int Index = 0; Index < skylightsCount; Index++)
						//{
						//    Factor = Factor + LightOutput.GetSkylightPointFactor2(new Vector3(sideWindowPositionArray[j].x, firstPosition.y, firstPosition.z) + new Vector3(0, 0, i * StepLength), skylightPositionArray[Index], skylightWidthArray[Index], skylightHeightArray[Index]);
						//}
						for (int Index = 0; Index < LampCount; Index++) {
							Illumination += LightOutput.GetPointIllumination (new Vector3 (sideWindowPositionArray [j].x, firstPosition.y, firstPosition.z) + new Vector3 (0, 0, i * StepLength), LampPositionArray [Index] * LightOutput.OutdoorIllumination, LampAngleArray [Index], LampIntensity [Index], LampSpotAngle [Index], 'y');
						}
						Illumination = Factor * LightOutput.OutdoorIllumination;
						StaticGraphValues [i] = (int)Illumination;
					}
					//dataOutput.OutputStaticGragh (StaticGraphValues, (j + 1).ToString ());
					//chartUpdate.OutputStaticGragh (StaticGraphValues, (j + 1).ToString ()); ;//更新第二张折线图
				}

			}


			//初始化显示照度的标签
			private void InitialLabels ()
			{
				for (int i = 0; i < Labels.Count; i++) {
					Destroy (Labels [i]);
				}
				Labels.Clear ();

				for (int i = 0; i < RowCount; i++) {
					for (int j = 0; j < ColumnCount; j++) {
						//Labels[i * RowCount + j] = GameObject.CreatePrimitive();
						//transform.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(FollowedTransform.position);
						GameObject Label = GameObject.Instantiate (LabelPerfab);
						Label.transform.SetParent (transform);
						Label.transform.position = new Vector3 (FirstPointPosition.x + i * GridDistance, FirstPointPosition.y, FirstPointPosition.z + j * GridDistance);
						Labels.Add (Label);
					}
				}
			}

		}
	}
}
