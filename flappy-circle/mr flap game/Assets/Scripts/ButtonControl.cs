using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	// Use this for initialization
    public GameObject PanelMainMenu;
    public GameObject PanelGameOver;
    public GameObject PanelUI;
    public static ButtonControl instance;
	void Start () {
        instance = this;
	}
	public void setMainMenu()
    {
        PanelMainMenu.SetActive(true);
        PanelGameOver.SetActive(false);
        PanelUI.SetActive(false);
        
        GamePlay.instance.UI_TEXT_BEST_SCORE_MAINMENU.text = "Best Score:"+ ScoreControl.BEST_SCORE.NUM.ToString();
        ScoreControl.currentScore = 0;
        GamePlay.instance.UI_TEXT_CURRENT_SCORE.text = "0";
    }
    public void setGameOver()
    {
        PanelMainMenu.SetActive(false);
        PanelGameOver.SetActive(true);
        PanelUI.SetActive(false);
        GamePlay.instance.UI_TEXT_BEST_SCORE_GAMEOVER.text = "Best Score:" + ScoreControl.BEST_SCORE.NUM.ToString();
    }
    public void setUI()
    {
        PanelMainMenu.SetActive(false);
        PanelGameOver.SetActive(false);
        PanelUI.SetActive(true);
        GamePlay.instance.UI_READY.SetActive(true); 
    }

	// Update is called once per frame
	void Update () {
	
	}
	public void PlayButtonPress()
	{
		//Application.LoadLevel("GamePlayScence");
		//Debug.Log ("aaaaaaaaaa");
        SoundEngine.playSound("SoundClick");
        AnimationEffect.instance.EndMainMenuEffect();
	}
	public void PlayInGameButtonPress()
	{
        SoundEngine.playSound("SoundClick");
		GamePlay.instance.restart();
		//Application.LoadLevel("GamePlayScence");
		//Debug.Log ("aaaaaaaaaa");
	}
    public void PlayMainMenuPress()
    {
        SoundEngine.playSound("SoundClick");
        //Application.LoadLevel("GamePlayScence");
        //Debug.Log("aaaaaaaaaa");
        AnimationEffect.instance.GameOverToMainMenu();
    }
	public void ButtonRatePress()
	{
        SoundEngine.playSound("SoundClick");
        Application.OpenURL("http://aegamemobile.com/web/view.php?id=22");		
	}
    public void ButtonSaredFBPress()
    {

    }
    public void ButtonChangeIndex()
    {
        Player.instance.index++;
        if (Player.instance.index > 3)
            Player.instance.index = 1;
        Player.instance.anim.Play("Fly_IDE" +  Player.instance.index.ToString());
    }
	

}
