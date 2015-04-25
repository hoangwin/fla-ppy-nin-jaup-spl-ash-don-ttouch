﻿using UnityEngine;
using System.Collections;

public static class WP8Statics
{

    //public delegate void MyDelegate(int num);
    //public static MyDelegate myDelegate;

    public static event System.EventHandler WP8FunctionHandleSMSOpen; //WP8FunctionHandle se la ham trong file xaml.cs
    public static event System.EventHandler WP8FunctionHandleStopAds;//StopAds
    public static event System.EventHandler WP8FunctionHandleRateApp;//StopAds
    public static event System.EventHandler WP8FunctionHandle2FbShared;//StopAds
    public static void SendSMS(string str)
    {
        if (WP8FunctionHandleSMSOpen != null)
        {

            WP8FunctionHandleSMSOpen(str, null);
        }
    }

    public static void RateApp(string str)
    {
        if (WP8FunctionHandleRateApp != null)
        {

            WP8FunctionHandleRateApp(str, null);
        }
    }

    public static void StopAds(string str)
    {
     //   ScoreControl._isAdcoin = 1;
        ScoreControl.saveGame();
        if (WP8FunctionHandleStopAds != null)
        {
            WP8FunctionHandleStopAds(str, null);
        }
    }
    public static void SharedFB(string str)
    {
      //  string path = Application.persistentDataPath +"/BanCaAnXu.jpg" ;
      //  Debug.Log(path);
      //  Application.CaptureScreenshot(path);
        if (WP8FunctionHandle2FbShared != null)
        {
            WP8FunctionHandle2FbShared("", null);
        }
    }
}