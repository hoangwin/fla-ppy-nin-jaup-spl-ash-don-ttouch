using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void PlayButtonPress()
	{
		Application.LoadLevel("GamePlayScence");
		Debug.Log ("aaaaaaaaaa");
	}
	public void PlayInGameButtonPress()
	{
		GamePlay.instance.restart();
		//Application.LoadLevel("GamePlayScence");
		//Debug.Log ("aaaaaaaaaa");
	}
	public void ButtonRatePress()
	{
#if UNITY_ANDROID
        SoundEngine.instance.PlayOneShot(SoundEngine.instance._soundclick);
        Application.OpenURL("market://details?id=com.xiaxio.ninjaup");
#elif UNITY_WP8
        WP8Statics.RateApp("Test");
#endif
	}
	public void RankingButtonPress()
	{
        Application.LoadLevel("Ranking");
	}
    public void RankingBackButtonPress()
    {
        Application.LoadLevel("GamePlayScence");
    }
	public void InputOkButtonPress()
	{
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
		//SoundEngine.isSound = !SoundEngine.isSound;
		MainMenu.instance.setBGButton ();
	}

}
