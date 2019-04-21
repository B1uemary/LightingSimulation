using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ui一直跟随镜头

public class UIFollow : MonoBehaviour
{

	public GameObject m_follow;                 //跟随的对象
	public RectTransform m_rectTransform;       //UI在屏幕上显示的位置
	public Vector3 m_offsetPos;                 //偏移值


	[Header ("Dont Open It")]
	public bool m_word = false;

	// Use this for initialization
	void Start ()
	{
		m_offsetPos = new Vector3 (0, 0, 0);    //默认不偏移
	}

	// Update is called once per frame
	void Update ()
	{

		if (m_word == true) {
			Vector3 tarPos = m_follow.transform.position;
			Vector3 pos = Camera.main.WorldToScreenPoint (tarPos);

			if (pos.z > 0) {
				m_rectTransform.position = pos + m_offsetPos;
			}
		}
	}

	public void Show ()
	{
		m_word = true;
	}

	public void Close ()
	{
		if (m_word == true) {
			m_word = false;
			m_rectTransform.position = new Vector3 (-500, -500, 0);
		}

	}
}
