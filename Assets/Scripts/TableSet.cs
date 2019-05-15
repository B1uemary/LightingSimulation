using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LightingExperiment
{
    /// <summary>
    /// 设置结果表格中的数据
    /// </summary>
    public class TableSet : MonoBehaviour
    {
        [Header("房间指标")]
        public InputField RCRInputField;
        public InputField CCRInputField;
        public InputField RCCInputField;

        [Header("设计指标")]
        public InputField sideWindowsLocationInputField;//侧窗的属性
        public InputField sideWindowsNumberInputField;
        public InputField sideWindowsDistanceInputField;
        public InputField sideWindowsAeraInputField;
        public InputField sideWindowsWidthInputField;
        public InputField sideWindowsHeightInputField;

        public InputField skylightsLocationInputField;//天窗的属性
        public InputField skylightsNumberInputField;
        public InputField skylightWallDistanceInputField;
        public InputField skylightsDistanceInputField;
        public InputField skylightsAeraInputField;
        public InputField skylightsWidthInputField;
        public InputField skylightsHeightInputField;

        public InputField LightCountInputField;


        [Header("计算指标")]
        public InputField windowGroundRatioInputField;
        public InputField depthRatioInputField;
        public InputField daylightFactorInputField;
        public InputField indoorIlluminationInputField;
        public InputField lightingUniformityInputField;

        #region 属性
        //室空间比
        public float RCR
        {
            //get { return 2; }
            set
            {
                RCRInputField.text = value.ToString("#0.##");
            }
        }

        //顶棚空间比
        public float CCR
        {
            set
            {
                CCRInputField.text = value.ToString("#0.##");
            }
        }

        public float RCC
        {
            set
            {
                RCCInputField.text = value.ToString("#0.##");
            }
        }

        //侧窗位置
        public float SideWindowsLocation
        {
            set
            {
                sideWindowsLocationInputField.text = value.ToString("#0.##");
            }
        }

        //侧窗个数
        public float SideWindowsNumber
        {
            set
            {
                sideWindowsNumberInputField.text = value.ToString();
            }
        }

        //侧窗间距
        public float SideWindowsDistance
        {
            set
            {
                sideWindowsDistanceInputField.text = (1000 * value).ToString();
            }
        }

        //侧窗面积
        public float SideWindowsArea
        {
            set
            {
                sideWindowsAeraInputField.text = value.ToString("#0.##");
            }
        }

        //侧窗宽度
        public float SideWindowsWidth
        {
            set
            {
                sideWindowsWidthInputField.text = (1000 * value).ToString();
            }
        }

        //侧窗高度
        public float SideWindowsHeight
        {
            set
            {
                sideWindowsHeightInputField.text = (1000 * value).ToString();
            }
        }

        public float LightCount
        {
            set
            {
                LightCountInputField.text = value.ToString("#0.##");
            }
        }

        //天窗位置
        public float SkylightsLocation
        {
            set
            {
                sideWindowsLocationInputField.text = value.ToString("#0.##");
            }
        }

        //天窗个数
        public float SkylightsNumber
        {
            set
            {
                skylightsNumberInputField.text = value.ToString();
            }
        }

        //天窗距离墙的位置
        public string SkylightWallDistance
        {
            set
            {
                skylightWallDistanceInputField.text = value;
            }
        }
        //天窗面积
        public float SkylightsArea
        {
            set
            {
                skylightsAeraInputField.text = value.ToString("#0.##");
            }
        }

        //天窗宽度
        public float SkylightsWidth
        {
            set
            {
                skylightsWidthInputField.text = (1000 * value).ToString();
            }
        }

        //天窗高度
        public float SkylightsHeight
        {
            set
            {
                skylightsHeightInputField.text = (1000 * value).ToString();
            }
        }

        //天窗间距
        public float SkylightsDistance
        {
            set
            {
                skylightsDistanceInputField.text = (1000 * value).ToString();
            }
        }

        //窗地比
        public float WindowGroundRadio
        {
            set
            {
                windowGroundRatioInputField.text = value.ToString("#0.##");
            }
        }

        //有效进深
        public float DepthRatio
        {
            set
            {
                depthRatioInputField.text = value.ToString("#0.##");
            }
        }

        //采光系数
        public float DaylightFactor
        {
            set
            {
                daylightFactorInputField.text = value.ToString("#0.##");
            }
        }

        //室内照度
        public float IndoorIllumination
        {
            set
            {
                indoorIlluminationInputField.text = value.ToString("#0.##");
            }
        }

        //采光均匀度
        public float LightingUniformity
        {
            set
            {
                lightingUniformityInputField.text = value.ToString("#0.##");
            }
        }
        
        #endregion

        public Dictionary<string,string> GetTableData()
        {
            Dictionary<string, string> tableData = new Dictionary<string, string>();
            //房间数据
            tableData.Add("RCR", RCRInputField.text);
            tableData.Add("CCR", CCRInputField.text);
            tableData.Add("RCC", RCCInputField.text);
            //侧窗数据
            tableData.Add("sideWindowsLocation", sideWindowsLocationInputField.text);
            tableData.Add("sideWindowsNumber", sideWindowsNumberInputField.text);
            tableData.Add("sideWindowsDistance", sideWindowsDistanceInputField.text);
            tableData.Add("sideWindowsAera", sideWindowsAeraInputField.text);
            tableData.Add("sideWindowsWidth", sideWindowsWidthInputField.text);
            tableData.Add("sideWindowsHeight", sideWindowsHeightInputField.text);
            //灯光数据
            tableData.Add("LightCount", LightCountInputField.text);
            //天窗数据
            //tableData.Add("skylightsLocation", skylightsLocationInputField.text);
            //tableData.Add("skylightsNumber", skylightsNumberInputField.text);
            //tableData.Add("skylightWallDistance", skylightWallDistanceInputField.text);
            //tableData.Add("skylightsDistance", skylightsDistanceInputField.text);
            //tableData.Add("skylightsAeraInput", skylightsAeraInputField.text);
            //tableData.Add("skylightsWidth", skylightsWidthInputField.text);
            //tableData.Add("skylightsHeight", skylightsHeightInputField.text);
            //采光计算数据
            tableData.Add("windowGroundRatio", windowGroundRatioInputField.text);
            tableData.Add("depthRatio", depthRatioInputField.text);
            tableData.Add("daylightFactor", daylightFactorInputField.text);
            tableData.Add("indoorIllumination", indoorIlluminationInputField.text);
            tableData.Add("lightingUniformity", lightingUniformityInputField.text);

            return tableData;
        }
    }
}

