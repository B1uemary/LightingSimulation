using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenShotTrigger : MonoBehaviour
{

	public GameObject m_screenShot;


	private void Start ()
	{
		AddButtonClick ("ScreenShotButton", HandleUnityAction);
        Debug.Log("相机启动");
	}

	void HandleUnityAction ()
	{
        Debug.Log("寻找相机");
        m_screenShot.GetComponent<ScreenShot> ().CaptureAllScene ();
    }


	public void AddButtonClick (string name, UnityAction callback)
	{
		Button button = GetControl<Button> (name);
		if (button) {
			button.onClick.AddListener (callback);
            Debug.Log("找到按钮");
        } else {
			Debug.LogWarning (name + " Not Found");
            Debug.Log("没找到按钮");
        }
        
    }


	public T GetControl<T> (string name)
	{
		var tran = transform.Find (name);
		if (tran) {
			return tran.GetComponent<T> ();
		}

		return default (T);
	}
}
