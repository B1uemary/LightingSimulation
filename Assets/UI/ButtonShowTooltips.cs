using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

//当鼠标悬浮时触发提升，场景需要有Canvas和Tips
public class ButtonShowTooltips : MonoBehaviour
{
	//public string DisplayName;
	//public Canvas canvas;//所在的画布
	//private GameObject ToolTipsUI;
	//private Vector2 pos;

	//void Start()
	//{
	//    canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
	//    ToolTipsUI = canvas.transform.Find("Tips").gameObject;
	//}

	////当鼠标悬浮在这个UI上触发
	//public void OnPointerEnter(PointerEventData eventData)
	//{
	//    if (ToolTipsUI != null)
	//    {
	//        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, transform.position, canvas.GetComponent<Camera>(), out pos);
	//        ToolTipsUI.GetComponentInChildren<Text>().text = DisplayName;
	//        ToolTipsUI.GetComponent<RectTransform>().anchoredPosition = pos;
	//        ToolTipsUI.GetComponent<RectTransform>().SetAsLastSibling();
	//        ToolTipsUI.SetActive(true);
	//    }
	//    else
	//    {
	//        Debug.LogWarning("TooltipsUI is null");
	//    }
	//}

	//public void OnPointerExit(PointerEventData eventData)
	//{
	//    if (ToolTipsUI != null)
	//    {
	//        ToolTipsUI.SetActive(false);
	//    }
	//}
}
