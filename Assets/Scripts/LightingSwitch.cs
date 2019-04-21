using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSwitch : MonoBehaviour
{
	UIFollow m_ui;

	public void OpenLight ()
	{
		transform.GetComponent<Light> ().enabled = true;

	}
	public void CloseLight ()
	{
		transform.GetComponent<Light> ().enabled = false;
	}


}
