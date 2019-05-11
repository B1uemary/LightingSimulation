//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using AdvancedGizmo;

//namespace LightingExperiment
//{
//    namespace Object
//    {
//        /// <summary>
//        /// 天窗
//        /// </summary>
//        public class SideWindow : Window
//        {

//            //区分不同的状态
//            //    public GameObject Gizmo;
//            [Header("Gizmos物体引用")]
//            public GameObject crossSectionBox;
//            public GameObject clickBox;
//            public GameObject setVisibleX;
//            public GameObject setVisibleXN;
//            public GameObject setVisibleY;
//            public GameObject setVisibleYN;

//            [SerializeField]
//            private List<GameObject> stills;

//            #region 属性
//            /// <summary>
//            /// 天窗属性
//            /// </summary>
//            public new Property CurrentProperty
//            {
//                get
//                {
//                    Property SkylightPara = new Property();
//                    SkylightPara.position = transform.position;
//                    SkylightPara.width = crossSectionBox.transform.localScale.x;
//                    SkylightPara.height = crossSectionBox.transform.localScale.y;
//                    return SkylightPara;
//                }

//                set
//                {
//                    currentProperty = value;
//                    transform.position = value.position;
//                    crossSectionBox.transform.localScale = new Vector3(value.width, value.height, crossSectionBox.transform.localScale.z);
//                }
//            }
//            public new State CurrentState
//            {
//                get
//                {
//                    return base.CurrentState;
//                }
//                set
//                {
//                    base.CurrentState = value;
//                    switch (value)
//                    {
//                        case State.Preview:
//                            break;
//                        case State.Normal:


//                            //  CrossSectionBox.SetActive(false);
//                            setVisibleX.SetActive(false);
//                            setVisibleXN.SetActive(false);
//                            setVisibleY.SetActive(false);
//                            setVisibleYN.SetActive(false);


//                            clickBox.SetActive(true);
//                            break;
//                        case State.Choiced:
//                            clickBox.SetActive(false);
//                            break;
//                        case State.Translation:

//                            clickBox.SetActive(false);

//                            //   CrossSectionBox.SetActive(false);
//                            setVisibleX.SetActive(false);
//                            setVisibleXN.SetActive(false);
//                            setVisibleY.SetActive(false);
//                            setVisibleYN.SetActive(false);


//                            break;
//                        case State.Rotation:

//                            clickBox.SetActive(false);

//                            // CrossSectionBox.SetActive(false);
//                            setVisibleX.SetActive(false);
//                            setVisibleXN.SetActive(false);
//                            setVisibleY.SetActive(false);
//                            setVisibleYN.SetActive(false);


//                            break;
//                        case State.Scaling:


//                            //  CrossSectionBox.SetActive(true);
//                            setVisibleX.SetActive(true);
//                            setVisibleXN.SetActive(true);
//                            setVisibleY.SetActive(true);
//                            setVisibleYN.SetActive(true);

//                            clickBox.SetActive(false);
//                            break;

//                        default:
//                            break;
//                    }
//                }
//            }
//            #endregion

//            //点击物体
//            public override void Click()
//            {
//                //发送消息到管理器
//            }

//            public override void Delete()
//            {
//                Destroy(gameObject);
//                //发送消息到管理器
//            }

//            public void SetVisible()
//            {
//                for(int i = 0; i < stills.Count; i++)
//                {
//                    stills[i].SetActive(false);
//                }
//            }

//        }

//    }

//}



