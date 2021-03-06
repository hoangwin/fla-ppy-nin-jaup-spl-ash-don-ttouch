﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlay : MonoBehaviour
{

    public GameObject AlignObject;
    public GameObject UI_READY;
    public Text UI_TEXT_CURRENT_SCORE;
    public Text UI_TEXT_BEST_SCORE_MAINMENU;
    public Text UI_TEXT_BEST_SCORE_GAMEOVER;

    public static float speedx = 0.06f;
    public static GamePlay instance;
    public static int currentState = 0;
    public const int STATE_MAINMENU = 0;
    public const int STATE_WATTING = 1;
    public const int STATE_PLAY = 2;
    
    public const int STATE_OVER = 3;
    public static float TimePlayedSubState = 0f;



    // Use this for initialization
    void Start()
    {
        DEF.Init();
        GameObject hand = GameObject.Find("Background");
        if (ScoreControl.BEST_SCORE == null)
        {
            ScoreControl.BEST_SCORE = new SuperInt(0, "BEST_SCORE");
            ScoreControl.BEST_SCORE.Load();
        }
        if (hand != null)
        {


            //hand.transform.localScale = new Vector3(DEF.sx_ortho, DEF.sy_ortho, 1);////chi dung cho truong hop bg 800 1280
            hand.transform.localScale = new Vector3(DEF.DISPLAY_WIDTH_ORTHO / DEF.DISPLAY_HEIGHT_ORTHO - 0.02f, DEF.DISPLAY_WIDTH_ORTHO / DEF.DISPLAY_HEIGHT_ORTHO-0.02f , 1);
            Debug.Log(DEF.DISPLAY_WIDTH_ORTHO);
        }
        Debug.Log("DEF.sx_ortho: " + DEF.sx_ortho + "," + DEF.sy_ortho);
        currentState = STATE_MAINMENU;
        instance = this;        
        ButtonControl.instance.setMainMenu();
        //DEF.ScaleAnchorGui();
        ScoreControl.BEST_SCORE.Load();
        //ScoreControl.BEST_SCORE.NUM = 0;
        //ScoreControl.BEST_SCORE.Save();
        ScoreControl.currentScore = 0;
        GamePlay.instance.UI_TEXT_CURRENT_SCORE.text = "0";
       
    }

    // Update is called once per frame
    void Update()
    {
        timeShowAds += Time.deltaTime;
        TimePlayedSubState += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            switch(GamePlay.currentState)
            {                    
                case GamePlay.STATE_MAINMENU:
                    Application.Quit();
                    break;
                case GamePlay.STATE_WATTING:
                    Debug.Log("1111");
                    iTween.Stop();
                    AnimationEffect.instance.WAITINGToMainMenu();
                    break;
                case GamePlay.STATE_PLAY:
                    Debug.Log("2222");
                    iTween.Stop();
                    AnimationEffect.instance.GamePlayDirectToMainMenu();
                    break;
                case GamePlay.STATE_OVER:
                    Debug.Log("3333");
                    iTween.Stop();
                    AnimationEffect.instance.GameOverDirectToMainMenu();
                    break;
            }
            
        }


    }
    void FixedUpdate()
    {
        if (GamePlay.currentState == GamePlay.STATE_PLAY || GamePlay.currentState == GamePlay.STATE_WATTING)
        {

        }
    }

    public void restart()
    {
        Player.instance.reStart();
        Trap.instance.init();
        //NGUITools.SetActive(PanelOverGame,false);
        //GamePlay.currentState = GamePlay.STATE_WATTING;
        GamePlay.instance.UI_READY.SetActive(true);          
        AnimationEffect.instance.GameOverRePlay();
        ScoreControl.currentScore = 0;
        GamePlay.instance.UI_TEXT_CURRENT_SCORE.text = "0";
    }

    public static bool firstShowAdsAtBegin = false;
    static public float timeShowAds = 0;
    public static void ShowADS()
    {
        Debug.Log(timeShowAds);
        if (timeShowAds > 90 || !firstShowAdsAtBegin)
        {
            Debug.Log("Ads");
            //Debug.Log("Ads1");
            if (!firstShowAdsAtBegin)
                firstShowAdsAtBegin = true;
            timeShowAds = 0;
#if UNITY_ANDROID
            using (AndroidJavaClass jc = new AndroidJavaClass("com.hcg.mrflapgame.UnityPlayerNativeActivity"))
			{
				jc.CallStatic<int>("ShowAds");
			}
			
#elif UNITY_WP8

            WP8Statics.ShowAds("");
#elif UNITY_IOS
			IOsStatic.ShowAds(" ", " ");
#endif
        }
    }
}
