using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProgressBarSetting : MonoBehaviour {
    [System.Serializable]
    public struct ProgressStep
    {
        public Image Line;
        public Image point;
    }
    public Color completeColor;
    public Color incompleteColor;
    public List<ProgressStep> progressSteps;
    private int step = 0;
    public UnityEvent setStep0Event;//添加一个进度条前进的事件
    public UnityEvent setStep1Event;//添加一个进度条前进的事件
    public UnityEvent setStep2Event;//添加一个进度条前进的事件
    public UnityEvent setStep3Event;//添加一个进度条前进的事件
    public UnityEvent setStep4Event;//添加一个进度条前进的事件
    public List<UnityEvent> StepEvents;


    /// <summary>
    /// 用于存档
    /// </summary>
    public int Step
    {
        get
        {
            return step;
        }
        set
        {
            step = value;
            for (int i = 0; i < progressSteps.Count; i++)
            {
                if (i <= value)
                {
                    progressSteps[i].point.color = completeColor;
                    progressSteps[i].Line.color = completeColor;
                }
                else
                {
                    progressSteps[i].point.color = incompleteColor;
                    progressSteps[i].Line.color = incompleteColor;
                }
            }
        }
    }


    /// <summary>
    /// 按进度条上的按钮
    /// </summary>
    /// <param name="progressStep"></param>
    public void PressButton(int progressStep)
    {
        if(progressStep<=step+1)//按完成后的按钮，会触发相应的事件
        {
            switch (progressStep)
            {
                case 0:
                    setStep0Event.Invoke();
                    break;
                case 1:
                    setStep1Event.Invoke();
                    break;
                case 2:
                    setStep2Event.Invoke();
                    break;
                case 3:
                    setStep3Event.Invoke();
                    break;
                case 4:
                    setStep4Event.Invoke();
                    break;
            }
        }

        if(progressStep==step+1)
        {
            Step = progressStep;
        }
        else if(progressStep > step+1)
        {
        }
    }
}
