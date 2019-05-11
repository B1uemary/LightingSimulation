//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using LightingExperiment.Menu;
//using UnityEngine.Events;

//namespace LightingExperiment
//{
//    namespace Object
//    {
//        //所有侧窗的整体属性
//        [System.Serializable]
//        public struct SideWindowsProperty
//        {
//            public int Count;
//            public float Width;//窗户的宽度，有不相同的时候为-1
//            public float Height;
//            public float SillWidth;
//            public float GroundHeight;
//            public float Area;//所有窗户的面积
//        }

//        /// <summary>
//        /// 侧窗管理
//        /// </summary>
//        public class SideWindowManager : MonoBehaviour
//        {

//            [Header("侧窗预制体")]
//            public GameObject sideWindowPrefab;
//            [Header("光照面")]
//            public GameObject lightPlane;
//            [Header("放窗户的墙")]
//            public GameObject wall;

//            [Header("用来初始化窗户的位置")]
//            public float sillWidth = 0.2f;
//            public Vector3 liftPosiition;
//            public float wallLenght = 8.0f;
//            //地面的Y轴高度
//            public const float groundY = -2.8f;

//            [Header("侧窗改变时触发事件")]
//            public UnityEvent onSideWindowsChange;

//            //窗户的参数，用于挖洞
//            private Vector4[] sectionDirXs = new Vector4[10];
//            private Vector4[] sectionDirYs = new Vector4[10];
//            private Vector4[] sectionDirZs = new Vector4[10];
//            private Vector4[] sectionCentres = new Vector4[10];
//            private Vector4[] sectionScales = new Vector4[10];

//            //保存窗户的动态数组
//            private List<GameObject> sideWindows = new List<GameObject>();

//            //面板属性
//            private SideWindowsProperty currentProperty;
//            private Transform sectionBox;
//            private IlluminatedPlane planeScript;
//            private Material wallMaterial;
//            private Window.Property sideWindowProperty = new Window.Property();


//            #region 属性
//            public SideWindowsProperty CurrentProperty
//            {
//                get
//                {
//                    return currentProperty;
//                }
//                set
//                {
//                    currentProperty = value;
//                    for (int i = 0; i < sideWindows.Count; i++)
//                    {
//                        sideWindowProperty.type = Window.Type.SildWindow;
//                        sideWindowProperty.position.Set(sideWindows[i].transform.position.x, currentProperty.GroundHeight + groundY, sideWindows[i].transform.position.z);
//                        sideWindowProperty.width = currentProperty.Width;
//                        sideWindowProperty.height = currentProperty.Height;
//                        sideWindows[i].GetComponent<SideWindow>().CurrentProperty = sideWindowProperty;
//                    }
//                    //窗户改变时，触发对应的事件
//                    onSideWindowsChange.Invoke();
//                }
//            }

//            public int ButtonIndex { get; set; }
//            #endregion


//            // Use this for initialization
//            void Start()
//            {
//                planeScript = lightPlane.GetComponent<IlluminatedPlane>();
//                wallMaterial = wall.GetComponent<Renderer>().material;
//            }

//            //改变窗户的形式——个数
//            public void SwitchWindowForm(int form)
//            {
//                ButtonIndex = form;//记录按下的按钮
//                if (currentProperty.Count != form)
//                {
//                    //销毁窗户
//                    for (int i = 0; i < sideWindows.Count; i++)
//                    {
//                        GameObject.Destroy(sideWindows[i]);
//                    }
//                    sideWindows.Clear();

//                    //创建相应的窗户
//                    for (int i = 0; i < form; i++)
//                    {
//                        GameObject CreatedWindow = GameObject.Instantiate(sideWindowPrefab);
//                        Debug.Log(liftPosiition);
//                        CreatedWindow.transform.position = new Vector3(liftPosiition.x + (i + 0.5f) * wallLenght / form, liftPosiition.y, liftPosiition.z);
//                        Debug.Log(CreatedWindow.transform.position);
//                        sideWindows.Add(CreatedWindow);
//                    }

//                    //更新面板上的属性值
//                    UpdateWindowProperty();
//                }
//            }

//            //添加侧窗
//            public void AddChild(GameObject child)
//            {
//                if(sideWindows.Count<=6)
//                {
//                    sideWindows.Add(child);
//                    UpdateWindowProperty();
//                }
//            }

//            //删掉侧窗
//            public void DestroyChild(GameObject child)
//            {
//                sideWindows.Remove(child);
//                UpdateWindowProperty();
//            }

//            /// <summary>
//            /// 读档
//            /// </summary>
//            /// <param name="savedSideWindowForm"></param>
//            /// <param name="savedSideWindowsProperty"></param>
//            public void Load(int savedSideWindowForm, SideWindowsProperty savedSideWindowsProperty)
//            {
//                SwitchWindowForm(savedSideWindowForm);
//                CurrentProperty = savedSideWindowsProperty;
//            }

//            //更新属性值并传到面板上，只在切换窗户时调用 ——————解决bug：传到面板后，面板值更新，又会设置窗户，窗户又更新（不能设置面板，否则无限循环）
//            public void UpdateWindowProperty()
//            {
//                if (sideWindows.Count > 0)
//                {
//                    for(int i=0;i<sideWindows.Count;i++)
//                    {

//                    }
//                    Window.Property windowProperty = sideWindows[0].GetComponent<SideWindow>().CurrentProperty;
//                    currentProperty.Count = sideWindows.Count;             
//                    currentProperty.Width = windowProperty.width;
//                    currentProperty.Height = windowProperty.height;
//                    currentProperty.SillWidth = sillWidth;
//                    currentProperty.GroundHeight = windowProperty.position.y - groundY;
//                }
//                else
//                {
//                    currentProperty.Count = 0;
//                    currentProperty.Width = 0;
//                    currentProperty.Height = 0;
//                    currentProperty.SillWidth = 0;
//                    currentProperty.GroundHeight = 0;
//                }
//                CurrentProperty = currentProperty;
//            }

//            //更新光照面材质的参数和横截面参数——问题出现在天空球上，需要搞明白天空球的消失与否（定向光的问题，已解决）
//            public void UpdateShaderPara()
//            {
//                for (int i = 0; i < sideWindows.Count; i++)
//                {
//                    //横截面
//                    sectionBox = sideWindows[i].GetComponent<SideWindow>().crossSectionBox.transform;
//                    sectionDirXs[i] = sectionBox.right;
//                    sectionDirYs[i] = sectionBox.up;
//                    sectionDirZs[i] = sectionBox.forward;
//                    sectionCentres[i] = sectionBox.position;
//                    sectionScales[i] = sectionBox.localScale;

//                    //光照面
//                    planeScript.sideWindowPositionArray[i] = sectionBox.position;
//                    planeScript.sideWindowWidthArray[i] = sectionBox.localScale.x;
//                    planeScript.sideWindowHeightArray[i] = sectionBox.localScale.y;
//                }
//                planeScript.sideWindowsCount = sideWindows.Count;

//                planeScript.UpdatePlanePara();//使用接口更新，避免update()
//                                              //将窗户参数组传入shader中
//                wallMaterial.SetVectorArray("_SectionDirX", sectionDirXs);
//                wallMaterial.SetVectorArray("_SectionDirY", sectionDirYs);
//                wallMaterial.SetVectorArray("_SectionDirZ", sectionDirZs);
//                wallMaterial.SetVectorArray("_SectionCentre", sectionCentres);
//                wallMaterial.SetVectorArray("_SectionScale", sectionScales);
//                wallMaterial.SetInt("_Count", sideWindows.Count);
//               // Debug.Log("chuanghude shuliang"+sideWindows.Count);
//            }
//        }
//    }
//} 