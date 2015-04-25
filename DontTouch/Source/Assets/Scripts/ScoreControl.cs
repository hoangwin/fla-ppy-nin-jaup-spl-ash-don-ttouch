using UnityEngine;
using System.Collections;

public class ScoreControl : MonoBehaviour {
	
	public static float Score1 = 0;//timer thap nhat
    public static int Score2 = 0;//step trong 30 s. cao nhat
    public static float Score3 = 0;//timer cao nhat
	public static string STRING_USER_NAME ="USER_NAME";
	public static string STRING_USER_SCORE_1 = "USER_SCORE_1";
    public static string STRING_USER_SCORE_2 = "USER_SCORE_2";
    public static string STRING_USER_SCORE_3 = "USER_SCORE_3";
	public static string UserName = "NaN";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public static void saveGame()
	{
		PlayerPrefs.SetString(STRING_USER_NAME, UserName);
        PlayerPrefs.SetFloat(STRING_USER_SCORE_1, Score1);
        PlayerPrefs.SetInt(STRING_USER_SCORE_2, Score2);
        PlayerPrefs.SetFloat(STRING_USER_SCORE_3, Score3);
		PlayerPrefs.Save();
	}
	public static void loadGame()
	{
		//PlayerPrefs.DeleteAll();
		UserName = PlayerPrefs.GetString(STRING_USER_NAME);
		if (UserName.Length <= 4)
						UserName = "NaN";
        
        Score1 =  PlayerPrefs.GetFloat(STRING_USER_SCORE_1);
        Score2 = PlayerPrefs.GetInt(STRING_USER_SCORE_2);
        Score3 = PlayerPrefs.GetFloat(STRING_USER_SCORE_3);
	}
    public static void setScore(float s1,int s2,float s3)
    {
		if (s1 > 0 && s1 <Score1 || Score1 == 0)
        {
            Score1 = s1;
        }
		if (s2 > 0 && s2 > Score2|| Score2 == 0)
        {
            Score2 = s2;
        }
		if (s3 > 0 && s3 > Score3|| Score2 == 0)
        {
            Score3 = s3;
        }    
    }
}
