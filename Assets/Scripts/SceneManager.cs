using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

	public string m_lightingParentName = "Light";
	public float m_clickDistance = 2;

	private string m_perfabName = "LightingUI";

	private GameObject m_lightParent;
	private GameObject m_ligthUIParent;
	private Transform m_character;
	private GameObject m_perfab;

	List<UIFollow> m_lightTransforms;

	// Use this for initialization
	void Start ()
	{

		m_lightParent = GameObject.Find (m_lightingParentName);
		m_ligthUIParent = GameObject.Find ("Canvas/LightingUI");
		m_character = GameObject.Find ("CharacterController").transform;
		Debug.Log (m_character);

		m_character.GetComponentInChildren<CharacterRaycaster> ().m_lightingParentName = m_lightingParentName;
		m_character.GetComponentInChildren<CharacterRaycaster> ().m_clickDistance = m_clickDistance;

		m_lightTransforms = new List<UIFollow> ();
		int childCount = m_lightParent.transform.childCount;
		for (int i = 0; i < childCount; i++) {
			InitItemUI (m_lightParent.transform.GetChild (i).gameObject);

		}


	}

	void InitItemUI (GameObject go)
	{

		if (m_perfab == null) {
			m_perfab = Resources.Load<GameObject> (m_perfabName);
		}


		GameObject UIObject = Instantiate (m_perfab);
		UIObject.name = "UI" + go.name;

		UIFollow uiFollow = go.AddComponent<UIFollow> ();
		Debug.Log (go);

		uiFollow.m_follow = go;

		uiFollow.m_rectTransform = UIObject.GetComponent<RectTransform> ();
		uiFollow.m_offsetPos = new Vector3 (0, 0, 0);

		uiFollow.Show ();
		m_lightTransforms.Add (uiFollow);
		UIObject.transform.SetParent (m_ligthUIParent.transform);

	}

	void Update ()
	{
		CheckLightUISwitch ();
	}

	void CheckLightUISwitch ()
	{
		foreach (UIFollow i in m_lightTransforms) {
			Transform f = i.transform;
			//whether distance between character with every light is longer with max distance
			if ((f.position.x - m_character.position.x) *
			(f.position.x - m_character.position.x) +
				(f.position.y - m_character.position.y) *
					(f.position.y - m_character.position.y) > (m_clickDistance * m_clickDistance)) {
				i.Close ();
			} else {
				i.Show ();
			}
		}
	}
}
