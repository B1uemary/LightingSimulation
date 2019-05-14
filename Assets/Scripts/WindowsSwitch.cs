using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsSwitch : MonoBehaviour
{
    public Transform go1;
    public Transform go2;

    void Start()
    {
        go1 = transform.GetChild(0);
        go2 = transform.GetChild(1);
    }


    public void Switch ()
	{
	
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
