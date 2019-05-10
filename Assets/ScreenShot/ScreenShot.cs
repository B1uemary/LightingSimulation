using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{


	[System.Serializable]
	public struct ImageSlot
	{
		public Image image;
		public bool HaveImage;
		public Sprite OriginSprit;
	}

	[Header ("截图的位置")]
	public ImageSlot [] imageSlots;

	public Sprite ImageOriginSprite;

    public Camera sceneShotCamera; //用来截场景中的三张图片
    public Camera sceneShotCamera2;
    public Camera sceneShotCamera3;

    public List<Transform> transformRefs;//用来截图的三个位置


	private void Start ()
	{
		sceneShotCamera.enabled = false;

		transformRefs = new List<Transform> ();
		for (int i = 0; i < transform.parent.childCount - 1; i++) {
			transformRefs.Add (transform.parent.GetChild (i + 1));
		}
	}
	//按下截图
	public void ScreenShotPress ()
	{
		bool Success = false;
		for (int i = 0; i < imageSlots.Length; i++) {
			if (!imageSlots [i].HaveImage) {
				Texture2D ScreenShot = CaptureCamera (Camera.main, transform.parent.GetComponent<Canvas> ().pixelRect);
				Sprite ScreenShotSprite = Sprite.Create (ScreenShot, new Rect (0, 0, ScreenShot.width, ScreenShot.height), new Vector2 (0, 0));
				imageSlots [i].image.sprite = ScreenShotSprite;
				imageSlots [i].HaveImage = true;
				Success = true;
				break;
			}
		}
		if (!Success) {
			Texture2D ScreenShot = CaptureCamera (Camera.main, transform.parent.GetComponent<Canvas> ().pixelRect);
			Sprite ScreenShotSprite = Sprite.Create (ScreenShot, new Rect (0, 0, ScreenShot.width, ScreenShot.height), new Vector2 (0, 0));
			imageSlots [2].image.sprite = ScreenShotSprite;
			imageSlots [2].HaveImage = true;
			Debug.Log ("截图位置满了");
		}
	}


	//关闭截图
	public void CloseImageSlot (int SlotNumber)
	{
		switch (SlotNumber) {
		case 0:
			if (imageSlots [0].HaveImage) {
				imageSlots [0].image.sprite = ImageOriginSprite;
				imageSlots [0].HaveImage = false;
			}
			break;
		case 1:
			if (imageSlots [1].HaveImage) {
				imageSlots [1].image.sprite = ImageOriginSprite;
				imageSlots [1].HaveImage = false;
			}
			break;
		case 2:
			if (imageSlots [2].HaveImage) {
				imageSlots [2].image.sprite = ImageOriginSprite;
				imageSlots [2].HaveImage = false;
			}
			break;
		default:
			break;
		}
	}



	/// <summary>  
	/// 对相机截图。   
	/// </summary>  
	/// <returns>The screenshot2.</returns>  
	/// <param name="camera">Camera.要被截屏的相机</param>  
	/// <param name="rect">Rect.截屏的区域</param>  
	Texture2D CaptureCamera (Camera camera, Rect rect)
	{
		// 创建一个RenderTexture对象  
		RenderTexture rt = new RenderTexture ((int)rect.width, (int)rect.height, 0);
		// 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
		camera.targetTexture = rt;
		camera.Render ();
		//ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
		//ps: camera2.targetTexture = rt;  
		//ps: camera2.Render();  
		//ps: -------------------------------------------------------------------  

		// 激活这个rt, 并从中中读取像素。  
		RenderTexture.active = rt;
		Texture2D screenShot = new Texture2D ((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);
		screenShot.ReadPixels (rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
		screenShot.Apply ();

		// 重置相关参数，以使用camera继续在屏幕上显示  
		camera.targetTexture = null;
		//ps: camera2.targetTexture = null;  
		RenderTexture.active = null; // JC: added to avoid errors  
		GameObject.Destroy (rt);
		// 最后将这些纹理数据，成一个png图片文件  
		//byte[] bytes = screenShot.EncodeToPNG();
		// string filename = Application.dataPath + "/Screenshot.png";
		//System.IO.File.WriteAllBytes(filename, bytes);
		//Debug.Log(string.Format("截屏了一张照片: {0}", filename));
		return screenShot;
	}

	//获得三张图片
	public List<Texture2D> CaptureAllScene ()
	{
		sceneShotCamera.enabled = true;
        sceneShotCamera2.enabled = true;
        sceneShotCamera3.enabled = true;

        List<Camera> cameraList = new List<Camera>();
		List<Texture2D> sceneImages = new List<Texture2D> ();

		for (int i = 0; i < transformRefs.Count; i++) {
            sceneShotCamera = cameraList[i];
			Texture2D texture = CaptureScene ();
			sceneImages.Add (texture);
			Debug.Log (i);
		}
		sceneShotCamera.enabled = false;
        sceneShotCamera2.enabled = false;
        sceneShotCamera3.enabled = false;


        for (int i = 0; i < transformRefs.Count; i++) {

			byte [] bytes = sceneImages [i].EncodeToPNG ();
			string filename = Application.dataPath + string.Format ("/Screenshot{0}.png", i);
			System.IO.File.WriteAllBytes (filename, bytes);
			//Debug.Log (string.Format ("截屏了一张照片: {0}", filename));

		}



		return sceneImages;
	}

	//从固定的位置截图
	private Texture2D CaptureScene ()
	{
		// 创建一个RenderTexture对象  
		RenderTexture rt = sceneShotCamera.targetTexture;
		sceneShotCamera.Render ();
		RenderTexture.active = rt;
		Texture2D screenShot = new Texture2D (rt.width, rt.height, TextureFormat.ARGB32, false);
		screenShot.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
		screenShot.Apply ();
		RenderTexture.active = null; // JC: added to avoid errors  
		return screenShot;
	}
}
