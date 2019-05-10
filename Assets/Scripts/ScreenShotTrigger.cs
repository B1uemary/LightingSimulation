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
	}

	void HandleUnityAction ()
	{
		m_screenShot.GetComponent<ScreenShot> ().CaptureAllScene ();

	}


	public void AddButtonClick (string name, UnityAction callback)
	{
		Button button = GetControl<Button> (name);
		if (button) {
			button.onClick.AddListener (callback);
		} else {
			Debug.LogWarning (name + " Not Found");
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
