using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using LightingExperiment.Chart;
using LightingExperiment.Object;
using UnityEngine.UI;

namespace LightingExperiment
{
    /// <summary>
    /// 将窗户和其他的数据输出到表格当中
    /// </summary>
    public class DataOutput : MonoBehaviour
    {

        //public SkylightManager skylightManager;
        //public SideWindowManager sideWindowManager;
        public IlluminatedPlane illuminatedPlane;
        //public LightManager lightManager;
        //public BoardLightsManager boardLightsManager;

        [Header("数据输出")]
       // public PieChartSetting pieChartSetting;
        //public ChartSetting chartSetting;
        public TableSet tableSet;
       // public CeilLighttableSet ceilLighttableSet;
        //public BoardLighttableSet boardLighttableSet;
        //平均照度/采光系数
        public Text meanText;
        //参考值
        public Text referenceText;
        private float SkylightGroundHeight = 0;
        private int sideWindowsCount = 1;

        //区别照度图上是照度还是采光系数
        private bool isIllu = true;

        public void UpdateData()
        {
            //OutputPie();

            OutputTable(tableSet);

            //OutputCeilLightsTable(ceilLighttableSet);

            //OutputBoardLightsTable(boardLighttableSet);

            OutputMean();
        }

        ////输出饼状图
        //public void OutputPie()
        //{
        //    pieChartSetting.SetValue(illuminatedPlane.PercentPart1, illuminatedPlane.PercentPart2);
        //}

        //通过tableSet来更新table
        public void OutputTable(TableSet tableSet)
        {
            //SideWindowsProperty sideWindowsProperty = sideWindowManager.CurrentProperty;
            //SkylightsProperty skylightsProperty = skylightManager.CurrentProperty;

            tableSet.RCR = LightOutput.GetRCR(SkylightGroundHeight, LightOutput.RoomWidth, LightOutput.RoomLength);
            tableSet.CCR = LightOutput.GetCCR(LightOutput.CeilingHeight - SkylightGroundHeight, LightOutput.RoomWidth, LightOutput.RoomLength);
            //tableSet.CCR = LightOutput.GetCCR();

            //侧窗属性设置
            tableSet.SideWindowsNumber = sideWindowsCount;
            if (sideWindowsCount <= 1)
            {
                tableSet.SideWindowsDistance = 0;
            }
            else
            {
                tableSet.SideWindowsDistance = LightOutput.RoomWidth / sideWindowsCount;
            }

            tableSet.SideWindowsArea = illuminatedPlane.sideWindowWidthArray[0] * illuminatedPlane.sideWindowHeightArray[0];
            tableSet.SideWindowsWidth = illuminatedPlane.sideWindowWidthArray[0];
            tableSet.SideWindowsHeight = illuminatedPlane.sideWindowHeightArray[0];

            //灯光属性设置
            tableSet.LightCount = illuminatedPlane.openedLight;

            //天窗属性设置
            //tableSet.SkylightsNumber = skylightsProperty.Count;
            //if (skylightsProperty.RowCount == 0 || skylightsProperty.Count / skylightsProperty.RowCount <= 1)
            //{
            //    tableSet.SkylightsDistance = 0;
            //}
            //else
            //{
            //    tableSet.SkylightsDistance = LightOutput.RoomWidth / skylightsProperty.Count * skylightsProperty.RowCount;
            //}

            //if (skylightsProperty.RowCount == 1)
            //{
            //    tableSet.SkylightWallDistance = (skylightsProperty.WallDistance0 * 1000).ToString("f0");
            //}
            //else if (skylightsProperty.RowCount == 2)
            //{
            //    tableSet.SkylightWallDistance = ((skylightsProperty.WallDistance0 * 1000).ToString("f0")) + "," + ((skylightsProperty.WallDistance1 * 1000).ToString("f0"));
            //}
            //tableSet.SkylightsArea = skylightsProperty.Width * skylightsProperty.Height;
            //tableSet.SkylightsWidth = skylightsProperty.Width;
            //tableSet.SkylightsHeight = skylightsProperty.Height;

            //计算指标
            tableSet.WindowGroundRadio = (sideWindowsCount * illuminatedPlane.sideWindowWidthArray[0] * illuminatedPlane.sideWindowHeightArray[0] + 0) / (LightOutput.RoomLength * LightOutput.RoomWidth); //0是天窗相关数据：天窗数*天窗高度*天窗宽度
            tableSet.DepthRatio = LightOutput.GetDepthRatio(LightOutput.RoomWidth, illuminatedPlane.sideWindowWidthArray[0]);
            tableSet.DaylightFactor = illuminatedPlane.DaylightFactorMean;
            tableSet.IndoorIllumination = illuminatedPlane.illuminationMean;
            tableSet.LightingUniformity = (illuminatedPlane.DaylightFactorMin + 0.01f) / (illuminatedPlane.DaylightFactorMean + 0.01f);
        }

        //输出到顶灯的table
        //public void OutputCeilLightsTable(CeilLighttableSet ceilLighttableSet)
        //{
        //    ceilLighttableSet.ceilLightsCount = lightManager.CurrentProperty.count;
        //    ceilLighttableSet.ceilLightsIntensity = lightManager.CurrentProperty.intensity;
        //    ceilLighttableSet.ceilLightTemperature = lightManager.CurrentProperty.temperature;
        //}

        //输出到灯的table
        //public void OutputBoardLightsTable(BoardLighttableSet boardLighttableSet)
        //{
        //    boardLighttableSet.boardLightsCount = boardLightsManager.CurrentProperty.count;
        //    boardLighttableSet.boardLightsIntensity = boardLightsManager.CurrentProperty.intensity;
        //    boardLighttableSet.boardLightTemperature = boardLightsManager.CurrentProperty.temperature;
        //}

        //改变照度图上的平均值
        public void OutputMean()
        {
            if (isIllu)
            {
                meanText.text = (illuminatedPlane.illuminationMean).ToString("#0.##");
                //Debug.Log(illuminatedPlane.illuminationMean);
            }
            else
            {
                meanText.text = illuminatedPlane.DaylightFactorMean.ToString("#0.##");
            }
        }

        //切换照度、采光系数
        public void SwitchMean(bool isIllumination)
        {
            isIllu = isIllumination;
            //Debug.Log(isIllu);
            if (isIllumination)
            {
                meanText.text = (illuminatedPlane.illuminationMean).ToString("#0.##");
                referenceText.text = LightOutput.referenceIllumination.ToString();
            }
            else
            {
                meanText.text = illuminatedPlane.DaylightFactorMean.ToString("#0.###");
                referenceText.text = LightOutput.referenceDaylightFactor.ToString();
            }
        }

        ////清空折线图
        //public void ClearStaticGragh()
        //{
        //    chartSetting.ClearLine();
        //}

        ////更新折线图
        //public void OutputStaticGragh(int[] values, string line)
        //{
        //    if (line == "1" || line == "2" || line == "3" || line == "4")
        //    {
        //        chartSetting.UpdateValues(values, line);
        //    }
        //}

    }
}

