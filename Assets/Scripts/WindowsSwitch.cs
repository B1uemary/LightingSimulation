using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsSwitch : MonoBehaviour
{

	public void Switch ()
	{
		Transform go1 = transform.GetChild (0);
		Transform go2 = transform.GetChild (1);
		if (go1 == null || go2 == null) { return; }
		if (go1.gameObject.activeSelf == false) {
			go1.gameObject.SetActive (true);
			go2.gameObject.SetActive (false);
		} else {
			go1.gameObject.SetActive (false);
			go2.gameObject.SetActive (true);
		}
	}
}
