using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	// Use this for initialization
	
	
	public static MainMenu instance;
	void Start () {
		DEF.Init ();
		DEF.ScaleAnchorGui();
		ScoreControl.loadGame();
		setBGButton ();
		instance = this;
		if (SoundEngine.soundclick == null)
		{
			SoundEngine.soundclick = GameObject.Find("SoundClick");
			DontDestroyOnLoad(SoundEngine.soundclick);
		}
	}	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		
	}
	public void setBGButton()
	{
		GameObject bgButton = GameObject.Find("BgSoundOnOFF");
		UISprite  target = bgButton.GetComponentInChildren<UISprite>();
		if(SoundEngine.isSound)
			target.spriteName = "MenuButtonSoundOn";
		else
			target.spriteName = "MenuButtonSoundOff";
		target.MakePixelPerfect();
	}
}
