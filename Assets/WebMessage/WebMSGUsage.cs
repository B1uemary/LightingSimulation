using System.Collections.Generic;
using UnityEngine;
using LitJson;
using LightingExperiment;

public class WebMSGUsage : MonoBehaviour {

    public WebMessage webMessage;
    public TableSet tableSet;
    public ScreenShot screenShot;

    [Header("测试的图像")]
    public List<Texture2D> testImages;//测试
    [Header("数据输出")]
    //public TableSet tableSet;
    [Header("测试的数据")]
    public Dictionary<string, string> Testdata=new Dictionary<string, string>();//测试的数据


    public void SaveProgress()
    {
        Save save = CreatSave();//创建一个存档
        string saveJsonString = JsonMapper.ToJson(save);
        webMessage.SaveProgress(saveJsonString);
    }

    public void submitDataAndImages()
    {
        Testdata = tableSet.GetTableData();
        testImages = screenShot.CaptureAllScene();
        webMessage.submitDataAndImages(Testdata, testImages);//提交数据图像
    }

    //创建档
    private Save CreatSave()
    {
        Save save = new Save();
        save.number = 1;
        save.msg = "test";
        return save;
    }

    //使用存档
    private void SetGame(Save save)
    {

    }

    //将读档返回的字符串转为save
    public void UseLoad(string saveJsonStr)
    {
        Save save = JsonMapper.ToObject<Save>(saveJsonStr);
        SetGame(save);
    }

}
