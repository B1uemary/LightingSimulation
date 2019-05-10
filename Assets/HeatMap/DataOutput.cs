//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using LightingExperiment.Chart;
//using LightingExperiment.Object;
//using UnityEngine.UI;

//namespace LightingExperiment
//{
//    /// <summary>
//    /// 将窗户和其他的数据输出到表格当中
//    /// </summary>
//    public class DataOutput : MonoBehaviour
//    {

//        public SkylightManager skylightManager;
//        public SideWindowManager sideWindowManager;
//        public IlluminatedPlane illuminatedPlane;
//        public LightManager lightManager;
//        public BoardLightsManager boardLightsManager;

//        [Header("数据输出")]
//        public PieChartSetting pieChartSetting;
//        public ChartSetting chartSetting;
//        public TableSetting tableSetting;
//        public CeilLightTableSetting ceilLightTableSetting;
//        public BoardLightTableSetting boardLightTableSetting;
//        //平均照度/采光系数
//        public Text meanText;
//        //参考值
//        public Text referenceText;

//        //区别照度图上是照度还是采光系数
//        private bool isIllu = true;

//        public void UpdateData()
//        {
//            OutputPie();

//            OutputTable(tableSetting);

//            OutputCeilLightsTable(ceilLightTableSetting);

//            OutputBoardLightsTable(boardLightTableSetting);

//            OutputMean();
//        }

//        //输出饼状图
//        public void OutputPie()
//        {
//            pieChartSetting.SetValue(illuminatedPlane.PercentPart1,illuminatedPlane.PercentPart2);
//        }

//        //通过tableSetting来更新table
//        public void OutputTable(TableSetting tableSetting)
//        {
//            SideWindowsProperty sideWindowsProperty = sideWindowManager.CurrentProperty;
//            SkylightsProperty skylightsProperty = skylightManager.CurrentProperty;

//            tableSetting.RCR = LightOutput.GetRCR(skylightsProperty.GroundHeight, LightOutput.RoomWidth, LightOutput.RoomLength);
//            tableSetting.CCR = LightOutput.GetCCR(LightOutput.CeilingHeight - skylightsProperty.GroundHeight, LightOutput.RoomWidth, LightOutput.RoomLength);
//            //tableSetting.CCR = LightOutput.GetCCR();

//            //侧窗属性设置
//            tableSetting.SideWindowsNumber = sideWindowsProperty.Count;
//            if(sideWindowsProperty.Count<=1)
//            {
//                tableSetting.SideWindowsDistance = 0;
//            }
//            else
//            {
//                tableSetting.SideWindowsDistance = LightOutput.RoomWidth / sideWindowsProperty.Count;
//            }

//            tableSetting.SideWindowsArea = sideWindowsProperty.Width * sideWindowsProperty.Height;
//            tableSetting.SideWindowsWidth = sideWindowsProperty.Width;
//            tableSetting.SideWindowsHeight = sideWindowsProperty.Height;
//            //天窗属性设置
//            tableSetting.SkylightsNumber = skylightsProperty.Count;
//            if (skylightsProperty.RowCount == 0||skylightsProperty.Count/ skylightsProperty.RowCount <= 1)
//            {
//                tableSetting.SkylightsDistance = 0;
//            }
//            else
//            {
//                tableSetting.SkylightsDistance = LightOutput.RoomWidth / skylightsProperty.Count * skylightsProperty.RowCount;
//            }

//            if(skylightsProperty.RowCount == 1)
//            {
//                tableSetting.SkylightWallDistance = (skylightsProperty.WallDistance0*1000).ToString("f0");
//            }
//            else if(skylightsProperty.RowCount == 2)
//            {
//                tableSetting.SkylightWallDistance = ((skylightsProperty.WallDistance0 * 1000).ToString("f0")) + "," + ((skylightsProperty.WallDistance1 * 1000).ToString("f0"));
//            }
//            tableSetting.SkylightsArea = skylightsProperty.Width * skylightsProperty.Height;
//            tableSetting.SkylightsWidth = skylightsProperty.Width;
//            tableSetting.SkylightsHeight = skylightsProperty.Height;

//            //计算指标
//            tableSetting.WindowGroundRadio = (sideWindowsProperty.Count * sideWindowsProperty.Width * sideWindowsProperty.Height + skylightsProperty.Count * skylightsProperty.Width * skylightsProperty.Height) / (LightOutput.RoomLength * LightOutput.RoomWidth);
//            tableSetting.DepthRatio = LightOutput.GetDepthRatio(LightOutput.RoomWidth, sideWindowsProperty.Width);
//            tableSetting.DaylightFactor = illuminatedPlane.DaylightFactorMean;
//            tableSetting.IndoorIllumination = illuminatedPlane.illuminationMean;
//            tableSetting.LightingUniformity = (illuminatedPlane.DaylightFactorMin + 0.01f) / (illuminatedPlane.DaylightFactorMean + 0.01f);
//        }

//        //输出到顶灯的table
//        public void OutputCeilLightsTable(CeilLightTableSetting ceilLightTableSetting)
//        {
//            ceilLightTableSetting.ceilLightsCount = lightManager.CurrentProperty.count;
//            ceilLightTableSetting.ceilLightsIntensity = lightManager.CurrentProperty.intensity;
//            ceilLightTableSetting.ceilLightTemperature = lightManager.CurrentProperty.temperature;
//        }

//        //输出到黑板灯的table
//        public void OutputBoardLightsTable(BoardLightTableSetting boardLightTableSetting)
//        {
//            boardLightTableSetting.boardLightsCount = boardLightsManager.CurrentProperty.count;
//            boardLightTableSetting.boardLightsIntensity = boardLightsManager.CurrentProperty.intensity;
//            boardLightTableSetting.boardLightTemperature = boardLightsManager.CurrentProperty.temperature;
//        }

//        //改变照度图上的平均值
//        public void OutputMean()
//        {
//            if (isIllu)
//            {
//                meanText.text = (illuminatedPlane.illuminationMean).ToString("#0.##");
//            }
//            else
//            {
//                meanText.text = illuminatedPlane.DaylightFactorMean.ToString("#0.##");
//            }
//        }

//        //切换照度、采光系数
//        public void SwitchMean(bool isIllumination)
//        {
//            isIllu = isIllumination;
//            if(isIllumination)
//            {
//                meanText.text = (illuminatedPlane.illuminationMean).ToString("#0.##");
//                referenceText.text = LightOutput.referenceIllumination.ToString();
//            }
//            else
//            {
//                meanText.text = illuminatedPlane.DaylightFactorMean.ToString("#0.###");
//                referenceText.text = LightOutput.referenceDaylightFactor.ToString();
//            }
//        }

//        //清空折线图
//        public void ClearStaticGragh()
//        {
//            chartSetting.ClearLine();
//        }

//        //更新折线图
//        public void OutputStaticGragh(int[] values,string line)
//        {
//            if(line=="1"||line=="2"||line=="3"||line=="4")
//            {
//                chartSetting.UpdateValues(values, line);
//            }
//        }

//    }
//}

