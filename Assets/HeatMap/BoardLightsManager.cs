//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//namespace LightingExperiment
//{
//    namespace Object
//    {
//        public class BoardLightsManager : MonoBehaviour
//        {
//            public struct BoardLightsProperty
//            {
//                public int count;
//                public float intensity;
//                public Temperature temperature;
//            }

//            public List<BoardLight> boardLights;//三个黑板灯
//            [HideInInspector]
//            public List<BoardLight> OpenedBoardLights=new List<BoardLight>();
//            // Use this for initialization
//            private BoardLightsProperty currentProperty;
//            public UnityEvent OnBoardLightChange;//灯改变时触发事件


//            public BoardLightsProperty CurrentProperty
//            {
//                get
//                {
//                    currentProperty.count = OpenedBoardLights.Count;
//                    if(OpenedBoardLights.Count==0)
//                    {
//                        currentProperty.intensity = 0;
//                        currentProperty.temperature = Temperature._7500K;
//                    }
//                    else
//                    {
//                        currentProperty.intensity = boardLights[0].Intensity;
//                        currentProperty.temperature = boardLights[0].Temper;
//                    }
//                    return currentProperty;
//                }
//                set
//                {
//                    currentProperty = value;
//                    for(int i = 0; i < boardLights.Count; i++)
//                    {
//                        boardLights[i].Intensity = currentProperty.intensity;
//                        boardLights[i].Temper = currentProperty.temperature;
//                    }
//                    OnChange();
//                }
//            }

//            //打开/关闭一个灯
//            public void OpenLight(int index, bool state)
//            {
//                if(index<boardLights.Count)
//                {
//                    boardLights[index].Switch(state);
//                    if (state)
//                    {
//                        OpenedBoardLights.Add(boardLights[index]);
//                    }
//                    else
//                    {
//                        OpenedBoardLights.Remove(boardLights[index]);
//                    }
//                }

//                OnChange();
//            }

//            //灯光改变时触发事件
//            private void OnChange()
//            {
//                if (OnBoardLightChange != null)
//                {
//                    OnBoardLightChange.Invoke();
//                }
//            }
//        }
//    }
//}

