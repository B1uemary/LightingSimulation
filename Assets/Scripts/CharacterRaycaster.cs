using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRaycaster : MonoBehaviour
{

	public LayerMask m_mask = ~0;

	public float m_clickDistance = 2;

	public string m_lightingParentName = "Lights";
	public string m_windowsParentName = "Windows";

	private Transform m_temoTran;

	// Update is called once per frame
	void Update ()
	{
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, m_clickDistance, m_mask)) {

			if (Input.GetMouseButtonDown (0)) {

				m_temoTran = hit.transform;

				//lighting
				if (m_temoTran.parent.name == m_lightingParentName) {
					LightingSwitch lightingSwitch = m_temoTran.GetComponent<LightingSwitch> ();
					if (m_temoTran.GetComponent<Light> ().enabled == false) {
						lightingSwitch.OpenLight ();
					} else {
						lightingSwitch.CloseLight ();
					}
				}

				//windows
				if (m_temoTran.parent.name == m_windowsParentName) {
					m_temoTran.GetComponent<WindowsSwitch> ().Switch ();
				}
			}

		}
		Debug.DrawLine (ray.origin, hit.point, Color.red);
	}



}
