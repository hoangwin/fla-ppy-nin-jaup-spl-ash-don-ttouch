using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	// Use this for initialization

    public GameObject soundOn;
    public GameObject soundOff;
	public static MainMenu instance;
	void Start () {
		DEF.Init ();
		DEF.ScaleAnchorGui();
		ScoreControl.loadGame();		
		instance = this;
        /*
        GameObject obj = GameObject.Find("ButtonPlay1");
        Vector3 pos = new Vector3(obj.transform.localPosition.x,obj.transform.localPosition.y,obj.transform.localPosition.z);
        obj.transform.localPosition = new Vector3(pos.x - 700, pos.y, pos.z);
         
        iTween.MoveBy(obj, iTween.Hash("x", -Camera.main.ScreenToWorldPoint( obj.transform.localPosition).x/4, "easeType", "easeInOutExpo", "loopType", "none", "delay", 0.1));
        */
        setSoundButton();
	}	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

	}
	
    void Awake()
    {
        GameObject gameMusic = GameObject.Find("bg_music");
        if (SoundEngine.isMusic)
        {
            if (!gameMusic.audio.isPlaying)              
                    gameMusic.audio.Play();
            
        }
        DontDestroyOnLoad(gameMusic);
        GameObject selectSound = GameObject.Find("ui_touch");        
        DontDestroyOnLoad(selectSound);
    }
    void setSoundButton()
    {
        if(!SoundEngine.isSound)
            GameObject.Find("BackgroundSound").transform.parent.GetComponent<UIButton>().normalSprite = "MenuButtonSoundOff";

        if (!SoundEngine.isMusic)
            GameObject.Find("BgSoundOnOFF").transform.parent.GetComponent<UIButton>().normalSprite = "MenuButtonMusicOff";
    }
}
