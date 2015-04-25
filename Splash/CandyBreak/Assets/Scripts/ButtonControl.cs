using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void PlayClassicButtonPress()
	{
		SoundEngine.playSound("ui_touch");
        GamePlay.gameMode = GamePlay.GAME_MODE_CLASSIC;
		Application.LoadLevel("SelectLevel");
	}

    public void PlayTimeAttackButtonPress()
	{
		SoundEngine.playSound("ui_touch");
        GamePlay.gameMode = GamePlay.GAME_MODE_UNLESS;
		Application.LoadLevel("GamePlayScence");
	}
     

	public void ExitButtonPress()
	{
		Application.Quit();
	}
    public void RePlayButtonPress()
	{
		SoundEngine.playSound("ui_touch");
	//	GamePlay.instance.restart();		
	}

   
   
	public void ButtonRatePress()
	{
		SoundEngine.playSound("ui_touch");
		Application.OpenURL ("market://details?id=com.flappy.bird.kiwi");
		//Application.OpenURL ("http://details?id=com.flappy.bird.kiwi");
	}
	public void RankingButtonPress()
	{
		SoundEngine.playSound("ui_touch");
		Application.LoadLevel("Ranking");
	}
    
	public void InputOkButtonPress()
	{
		SoundEngine.playSound("ui_touch");
		string str = GameObject.Find ("LabelInputName").GetComponent<UILabel> ().text;
		str = str.Trim();
		str = str.Replace("'","_");
		str = str.Replace("\"","_");
		str = str.Replace(" ","_");
		str =str.Replace("=","_");
		GameObject.Find ("LabelInputName").GetComponent<UILabel> ().text = str;
		if (str.Length >= 5) {
			ScoreControl.UserName = str;
			ScoreControl.saveGame();
			NGUITools.SetActive(Raking.instance.PanelInputName,false);  
			NGUITools.SetActive(Raking.instance.PanelBoard,true);  
			Raking.instance.PostHightScore ();
			Raking.instance.getHightScore ();
			Raking.loadRanking = true;			
		}

	}

    //select level
   
    public void ButtonSLLevelPress(int i)
    {
        SoundEngine.playSound("ui_touch");
        SelectLevel.mcurrentLevel = i + SelectLevel.mcurrentPage * 16;
        Debug.Log(SelectLevel.mcurrentLevel);
        Application.LoadLevel("GamePlayScence");
    }
    public void ButtonSLLevelPress1()
    {
        ButtonSLLevelPress(1);
    }
    public void ButtonSLLevelPress2()
    {
        ButtonSLLevelPress(2);
    }
    public void ButtonSLLevelPress3()
    {
        ButtonSLLevelPress(3);
    }
    public void ButtonSLLevelPress4()
    {
        ButtonSLLevelPress(4);
    }
    public void ButtonSLLevelPress5()
    {
        ButtonSLLevelPress(5);
    }
    public void ButtonSLLevelPress6()
    {
        ButtonSLLevelPress(6);
    }
    public void ButtonSLLevelPress7()
    {
        ButtonSLLevelPress(7);
    }
    public void ButtonSLLevelPress8()
    {
        ButtonSLLevelPress(8);
    }
    public void ButtonSLLevelPress9()
    {
        ButtonSLLevelPress(9);
    }
    public void ButtonSLLevelPress10()
    {
        ButtonSLLevelPress(10);
    }
    public void ButtonSLLevelPress11()
    {
        ButtonSLLevelPress(11);
    }
    public void ButtonSLLevelPress12()
    {
        ButtonSLLevelPress(12);
    }
    public void ButtonSLLevelPress13()
    {
        ButtonSLLevelPress(13);
    }
    public void ButtonSLLevelPress14()
    {
        ButtonSLLevelPress(14);
    }
    public void ButtonSLLevelPress15()
    {
        ButtonSLLevelPress(15);
    }
    public void ButtonSLLevelPress16()
    {
        ButtonSLLevelPress(16);
    }
    public void ButtonSLLeft()
    {
        SoundEngine.playSound("ui_touch");
       SelectLevel.mcurrentPage--;

       if (SelectLevel.mcurrentPage < 0)
           SelectLevel.mcurrentPage = SelectLevel.MAX_PAGE - 1;
       SelectLevel.changePage();
    }

    public void ButtonSLRight()
    {
        SoundEngine.playSound("ui_touch");
        SelectLevel.mcurrentPage++;
        if (SelectLevel.mcurrentPage >= SelectLevel.MAX_PAGE)
            SelectLevel.mcurrentPage = 0;
        SelectLevel.changePage();
    }

    public void OnClick()
    {
       
        Debug.Log(this.gameObject.name);
        if (this.gameObject.name.Equals("ButtonSound"))
        {
            SoundEngine.isSound = !SoundEngine.isSound;
            if (SoundEngine.isSound)
            {
                GameObject.Find("BackgroundSound").transform.parent.GetComponent<UIButton>().normalSprite = "MenuButtonSoundOn";
            //    SoundEngine.playSound("bg_music");
            }
            else
            {
                GameObject.Find("BackgroundSound").transform.parent.GetComponent<UIButton>().normalSprite = "MenuButtonSoundOff";
             //   SoundEngine.stopSound("bg_music");
            }
            
            
            //SoundEngine.playSound("ui_touch");
        }
        else if (this.gameObject.name.Equals("ButtonMusic"))
        {
            SoundEngine.isMusic = !SoundEngine.isMusic;
            if (SoundEngine.isMusic)
            {
                GameObject.Find("BgSoundOnOFF").transform.parent.GetComponent<UIButton>().normalSprite = "MenuButtonMusicOn";
                SoundEngine.playSoundBG("bg_music");
            }
            else
            {
                GameObject.Find("BgSoundOnOFF").transform.parent.GetComponent<UIButton>().normalSprite = "MenuButtonMusicOff";
                SoundEngine.stopSound("bg_music");
            }


            //SoundEngine.playSound("ui_touch");
        }
        else if (this.gameObject.name.Equals("ButtonBackMainMenu"))
        {
            Application.LoadLevel("MainMenu");
        }
        else if (this.gameObject.name.Equals("ButtonIGM"))
        {
            GamePlay.instance.setIGM();
        }
        else if (this.gameObject.name.Equals("ButtonPausePlay"))
        {
            GamePlay.instance.setIGM();
        }
        else if (this.gameObject.name.Equals("ButtonPlayNext"))//play level tip theo
        {
            GamePlay.instance.playNextLevel();
        }
        else if (this.gameObject.name.Equals("ButtonRePlay"))//play level tip theo
        {
            GamePlay.instance.rePlayLevel();
        }
            
        else if (this.gameObject.name.Equals("ButtonMainMenu"))//play level tip theo
        {
            Application.LoadLevel("MainMenu");
            //   
        }
        SoundEngine.playSound("ui_touch");
    }
}
