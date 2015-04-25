using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Play60StepButtonPress()
	{
		SoundEngine.playSound("SoundClick");
        GamePlay.gameMode = GamePlay.GAME_MODE_CLASSIC;
		Application.LoadLevel("GamePlayScence");
	}

    public void PlayTimeAttackButtonPress()
	{
		SoundEngine.playSound("SoundClick");
        GamePlay.gameMode = GamePlay.GAME_MODE_TIME_ATTACK;
		Application.LoadLevel("GamePlayScence");
	}
     public void PlayArcadeButtonPress()
	{
		SoundEngine.playSound("SoundClick");
        GamePlay.gameMode = GamePlay.GAME_MODE_TIME_UNLESS;
		Application.LoadLevel("GamePlayScence");
	}

	public void ExitButtonPress()
	{
		Application.Quit();
	}
    public void RePlayButtonPress()
	{
		SoundEngine.playSound("SoundClick");
		GamePlay.instance.restart();
		//Application.LoadLevel("GamePlayScence");
		//Debug.Log ("aaaaaaaaaa");
	}
    public void MainMenuButtonPress()
    {
		SoundEngine.playSound("SoundClick");
        Application.LoadLevel("MainMenu");
    }

	public void ButtonRatePress()
	{
		SoundEngine.playSound("SoundClick");
		Application.OpenURL ("market://details?id=com.flappy.bird.kiwi");
		//Application.OpenURL ("http://details?id=com.flappy.bird.kiwi");
	}
	public void RankingButtonPress()
	{
		SoundEngine.playSound("SoundClick");
		Application.LoadLevel("Ranking");
	}
    public void RakingLeftButtonPress()
    {
		SoundEngine.playSound("SoundClick");
        Raking.RankingchangeType(Raking.typeRank -1);
    }
    public void RakingRightButtonPress()
    {
		SoundEngine.playSound("SoundClick");
        Raking.RankingchangeType(Raking.typeRank+1);
    }
	public void InputOkButtonPress()
	{
		SoundEngine.playSound("SoundClick");
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
	public void ButtonSoundPress()
	{

		SoundEngine.isSound = !SoundEngine.isSound;
		MainMenu.instance.setBGButton ();
		SoundEngine.playSound("SoundClick");
	}

}
