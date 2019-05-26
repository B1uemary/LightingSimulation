using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 网络通信
/// </summary>
public class WebMessage : MonoBehaviour {

    /// <summary>
    /// 转送数据的类型
    /// </summary>
    enum PostState
    {
        Common,
        SaveProgress,
        LoadProgress,
        SubmitExperiment,
        SubmitImage,
        SubmitDataAndImage
    }

    [SerializeField]
    [Header("默认远程地址，得不到当前网页地址时使用")]
    private string testRemoveUrl= "http://172.20.143.251:8080/?userId=114&courseId=713";
    [SerializeField]
    [Header("ak")]
    private string ak = "XGgu82xNAYQGNoxSgzw9NDhXWcC5y6Jt";
    private string saveProgressUrl;
    private string loadProgressUrl;
    private string submitDataUrl;
    private string submitImageUrl;
    private string submitExperimentUrl;
    private string keepSessionUrl;
    private string anotherExerimentUrl;

    [Header("插槽号")]
    public int slotNO = 0;//插槽号
    private string hostUrl;//完整的主机url
    private string wwwText;//返回的值
   // private bool haveSubmitThis = false;//自己的实验结果是否提交成功
    private bool haveSubmitAnother = true;//另一边实验是否做完
    [HideInInspector]
    public int haveSubmit = 0;//是否自己这边已经提交

    //连续传送数据和图片
    private bool isSubmitDataAndImage = false;//开启通道
    private int progressNo = 0;//传的进度
    private Dictionary<string, string> resultData;//保存data
    private List<byte[]> resultImages;//图表

    //保持会话
    float lastTime;
    private float keepSessionTime = 20 * 60;

    //开始时从当前网页上获取地址
    private void Awake()
    {
        //组装调用接口的默认网址，可用于测试
        BreakUrl(testRemoveUrl,false);
        CallFunctionInWeb();
        lastTime = Time.time;
    }

    //每隔20分钟保持一次会话
    private void Update()
    {
        if (Time.time - lastTime > keepSessionTime)
        {
            lastTime = Time.time;
            KeepSession();
        }
    }

    //1、请求存档到服务器
    public void SaveProgress(string saveData)
    {
        hostUrl = saveProgressUrl;
        WWWForm form = new WWWForm();
        form.AddField("data", saveData);
        Debug.Log("saveUrl:" + hostUrl);
        WWW www = new WWW(hostUrl, form);
        StartCoroutine(PostData(www, PostState.SaveProgress));
    }

    //2、请求从服务器请求读档
    public void loadProgress()
    {
        hostUrl = loadProgressUrl;
        WWW www = new WWW(hostUrl);
        StartCoroutine(PostData(www, PostState.LoadProgress));
    }

    //3、请求提交数据到服务器
    public void SubmitData(Dictionary<string,string> resultData)
    {
        hostUrl = submitDataUrl;
        WWWForm form = new WWWForm();
        form.AddField("Data", LitJson.JsonMapper.ToJson(resultData));
        WWW www = new WWW(hostUrl, form);
        StartCoroutine(PostData(www, PostState.SubmitDataAndImage));
    }

    //4、请求提交图片到服务器
    public void SubmitImage(int imageNO, byte[] imageBytes)
    {
        string imageNOStr = "&ImageNO=" + (imageNO).ToString();
        hostUrl = submitImageUrl + imageNOStr;
        WWWForm form = new WWWForm();
        //form.AddField("userId", userIdStr);//加上会报错，不能有两个重复的userID
        form.AddBinaryData("file", imageBytes);
        Debug.Log("请求提交图片");
        //uploadProgressBar.SetProgress(imageNO + 1, 7, "图片" + (imageNO + 1));
        // MyDebug.Add("图片的Bytes长度："+imageBytes.Length.ToString());
        Debug.Log("请求提交图片"+hostUrl);
        WWW www = new WWW(hostUrl, form);
        StartCoroutine(PostData(www, PostState.SubmitDataAndImage));
    }

    //5、请求完成试验提交，只提交一次
    public void submitExperiment()
    {

        hostUrl = submitExperimentUrl;
        WWW www = new WWW(hostUrl);
        Debug.Log("开始最终提交"+ hostUrl);
        StartCoroutine(PostData(www, PostState.SubmitExperiment));
    }

    //6、一起提交数据和图片
    public void submitDataAndImages(Dictionary<string,string> data, List<byte[]> images)
    {
        progressNo = 0;
        isSubmitDataAndImage = true;
        resultData = data;
        resultImages = images;
        //uploadProgressBar.gameObject.SetActive(true);//显示进度条
        //uploadProgressBar.SetProgress(1, 7, "表格数据");//设置进度条
        SubmitData(data);
    }

    //7、一起提交数据和图片
    public void submitDataAndImages(Dictionary<string, string> data, List<Texture2D> images)
    {
        progressNo = 0;
        isSubmitDataAndImage = true;
        resultData = data;
        resultImages = new List<byte[]>();
        for(int i = 0; i < images.Count; i++)
        {
            resultImages.Add(images[i].EncodeToPNG());
        }
        //uploadProgressBar.gameObject.SetActive(true);//显示进度条
        //uploadProgressBar.SetProgress(1, 7, "表格数据");//设置进度条
        SubmitData(data);
    }

    //从当前的网页上得到URL
    public void CallFunctionInWeb(string functionName = "SendUrlToUnity")
    {
        Debug.Log("调用网上的函数"+ functionName);
        Application.ExternalCall("SendUrlToUnity");
    }

    /// <summary>
    /// 跳转到照明实验
    /// </summary>
    /// <param name="haveSubmit">0：未提交；1：提交</param>
    public void CallJumpFunctionInWeb()
    {
        if(haveSubmit==1)
        {
            Debug.Log("实验完成");

            string submitStr = "&submit=1";

            char[] submitStrEncode = submitStr.ToCharArray();
            for (int i = 0; i < submitStrEncode.Length; i++)
            {
                submitStrEncode[i] = (char)(submitStrEncode[i] ^ 1);
            }
            Application.ExternalCall("JumpToAnother", new string(submitStrEncode));
            Debug.Log("调用网上的函数,跳转到照明实验，参数");
        }
        else
        {
            Debug.Log("实验结果还未提交");
        }
    }



    //由网页上调用
    public void GetUrlFormWeb(string url)
    {
        Debug.Log("调用成功"+ url);
        BreakUrl(url);
        loadProgress();
    }

    //初始化，得到一些id信息
    private bool BreakUrl(string url, bool ifDecrypt = true)
    {
        if(url==null)
        {
            return false;
        }
        string tempURL;
        if (ifDecrypt)
        {
            tempURL = DecryptUrl(url);//解密
        }
        else
        {
            tempURL = url;
        }
        //获取解密后的url中的信息
        //根据GetUrlParam（）工具方法获取键值拼接得到后半部信息
        string webMessageOfEnd =
            "ak=" + ak
            + "&userId=" + GetUrlParam("userId", tempURL)
            + "&courseId=" + GetUrlParam("courseId", tempURL)
            + "&JSESSIONID=" + GetUrlParam("JSESSIONID", tempURL);
        //先除去url开头“http://”这个字符，再利用’/‘截取字符串，从而得到前半部分信息
        string webMessageOfBegin = GetWebMessageOfBegin(url);//得到前面的网址

        //得到调用接口的网址
        string slotNOStr = "&slotNO=" + slotNO;
        saveProgressUrl = "http://" + webMessageOfBegin + "/generalAPI.saveProgress.action?" + webMessageOfEnd + slotNOStr; //存挡调用接口
        loadProgressUrl = "http://" + webMessageOfBegin + "/generalAPI.loadProgress.action?" + webMessageOfEnd + slotNOStr;
        submitDataUrl = "http://" + webMessageOfBegin + "/generalAPI.submitData.action?" + webMessageOfEnd + slotNOStr;
        submitImageUrl = "http://" + webMessageOfBegin + "/generalAPI.submitImage.action?" + webMessageOfEnd + slotNOStr;
        submitExperimentUrl = "http://" + webMessageOfBegin + "/generalAPI.submitExperiment.action?" + webMessageOfEnd + slotNOStr +"&place=6";//??需要加SlotNO吗？？
        keepSessionUrl = "http://" + webMessageOfBegin + "/generalAPI.keepSession.action?" + webMessageOfEnd + slotNOStr;
        //判断另一个实验是否提交
        string submitValue = GetUrlParam("submit", url);
        if (submitValue == "1")
        {
            haveSubmitAnother = true;
        }
        else
        {
            haveSubmitAnother = false;
        }
        return true;
    }

    //得到URL中的参数值
    public string GetUrlParam(string name, string url)
    {
        System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?");
        System.Text.RegularExpressions.MatchCollection mc = re.Matches(url);
        foreach (System.Text.RegularExpressions.Match m in mc)
        {
            if (m.Result("$2").Equals(name))
            {
                return m.Result("$3");
            }
        }
        return "";
    }

    //得到前面的网址
    public string GetWebMessageOfBegin(string url)
    {
        //先除去url开头“http://”这个字符，再利用’/‘截取字符串，从而得到前半部分信息
        string str1 = url.Remove(0, 7);
        string[] str2 = str1.Split('/');
        Debug.Log("URLremove: " + str2[0]);
        return str2[0];
    }

    //对网页上获取的url进行解密操作
    public string DecryptUrl(string url)
    {
        //对网页上获取的url进行解密操作
        string[] tempSplit = url.Split('?');
        char[] transformURL = tempSplit[1].ToCharArray();
        for (int i = 0; i < transformURL.Length; i++)
        {
            transformURL[i] = (char)(transformURL[i] ^ 1);
        }
        tempSplit[1] = new string(transformURL);
        return tempSplit[0] + "?" + tempSplit[1];
    }

    //保持对话期调用接口
    public void KeepSession()
    {
        WWWForm form = new WWWForm();
        Dictionary<string, string> headers = form.headers;
        //获取1970至今时间（ms）
        TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
        long test = Convert.ToInt64(ts.TotalMilliseconds);
        string startTime = test.ToString();
        headers["Content-Type"] = "application/json";
        headers["Accept"] = "application/json";
        headers["Cookie"] = "JSESSIONID=node01skyverfqrpzbjr3o51cepdd20.node();menuClickTime=" + startTime;
        //keepSessionURL = "http://172.20.143.251:8081/generalAPI.keepSession.action?ak=CQUCHXZJES20190326V1.0&userId=53&courseId=49&slotNO=1";
        WWW www = new WWW(keepSessionUrl, null, headers);
        StartCoroutine(SendCookie(www));
    }

    //保持对话期协程
    IEnumerator SendCookie(WWW www)
    {
        yield return www;
        Debug.Log(www.text);
    }

    //post到远程服务器，并得到返回值
    private IEnumerator PostData(WWW www, PostState postState = PostState.Common)
    {
        yield return www;
        Debug.Log(www.text);
        if (!string.IsNullOrEmpty(www.error) || www.text.Equals("false"))
        {
            //在控制台输出错误信息
            Debug.Log(www.error);
            ProcessFailed(postState);
            //将失败面板显示  上传中不显示;
        }
        else if(www.text.Substring(1, 9)== "\"code\": 0")
        {
            //如果成功，处理返回值
            switch (postState)
            {
                case PostState.SaveProgress:
                    break;
                case PostState.LoadProgress://请求读档成功
                    string loadString = www.text.Substring(www.text.IndexOf("\"data\":") + 9);
                    loadString = loadString.Substring(0, loadString.Length - 2);
                    Debug.Log("读档成功："+ loadString);
                    break;
                case PostState.SubmitDataAndImage://请求传送数据和图片一次成功
                    ProcessReturnValue(www.text);//处理返回值
                    break;
                case PostState.SubmitExperiment://请求提交试验成功
                    //saveAndLoadByJSON.SaveToWebByJson();
                    Application.ExternalCall("SubmitExperimentSuccess");//提交成功后，调用网上的函数
                    Debug.Log("提交试验成功：");
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("服务器拒绝请求"+ www.text);
            ProcessFailed(postState);
        }
    }

    //处理失败
    private void ProcessFailed(PostState postState)
    {
        if (isSubmitDataAndImage)
        {
            //uploadProgressBar.gameObject.SetActive(false);//如果失败，同样隐藏进度条
            Debug.Log("上传失败");
            isSubmitDataAndImage = false;
        }
    }

    //处理成功后的返回值
    private void ProcessReturnValue(string wwwText)
    {
        //string code = wwwText.Substring(1, 9);
        Debug.Log("操作成功"+ wwwText);
        if (isSubmitDataAndImage)
        {
            progressNo++;//表示传成功一次
            Debug.Log("图片开始传送:" + (progressNo - 1).ToString());
            if (progressNo >= 1 && progressNo < resultImages.Count+1)//总共传七次 1data， 6images
            {
                SubmitImage(progressNo - 1, resultImages[progressNo - 1]);
                Debug.Log("图片传输成功 " + progressNo.ToString());
            }
            if (progressNo == resultImages.Count + 1)
            {
                isSubmitDataAndImage = false;
                haveSubmit = 1;
                //uploadProgressBar.gameObject.SetActive(false);//使进度条消失
                Debug.Log("上传成功");
                submitExperiment();
                //saveAndLoadByJSON.SaveToWebByJson();//存档
            }
        }

    }

   

}
