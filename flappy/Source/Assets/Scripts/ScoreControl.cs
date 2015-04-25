using UnityEngine;
using System.Collections;

public class ScoreControl : MonoBehaviour {
	public static int OFFSET_SCORE = 111;
	public static int Score = 0;
	public static int BestScore = 0;
	public static string STRING_USER_NAME ="USER_NAME";
	public static string STRING_USER_SCORE ="USER_SCORE";
	public static string UserName = "NaN";
	public static int getRealScore()
	{
			return Score - OFFSET_SCORE;
	}
	public static void setDefaultScore()
	{
		 Score = OFFSET_SCORE;
	}
	public static int getRealBestScore()
	{
		return BestScore - OFFSET_SCORE;
	}
	public static void setDefaultBestScore()
	{
		BestScore = OFFSET_SCORE;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public static void saveGame()
	{
		PlayerPrefs.SetString(STRING_USER_NAME, UserName);
		PlayerPrefs.SetInt(STRING_USER_SCORE, BestScore);
		PlayerPrefs.Save();
	}
	public static void loadGame()
	{
		//PlayerPrefs.DeleteAll();
		UserName = PlayerPrefs.GetString(STRING_USER_NAME);
		if (UserName.Length <= 4)
						UserName = "NaN";
		BestScore = PlayerPrefs.GetInt(STRING_USER_SCORE);
		if (BestScore < 1)
						setDefaultBestScore ();
		//BestScore += OFFSET_SCORE;

	}
}
