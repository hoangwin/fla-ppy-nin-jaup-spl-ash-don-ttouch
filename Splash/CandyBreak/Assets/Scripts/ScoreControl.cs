using UnityEngine;
using System.Collections;

public class ScoreControl : MonoBehaviour {	

	public static string STRING_USER_NAME ="USER_NAME";
    public static string UserName = "NaN";

	public static string STRING_USER_SCORE_1 = "USER_SCORE_1";
    public static int bestScore = 0;//

    public static string STRING_USER_LEVEL = "USER_LEVEL";
    public static int levelUnBlock = 0;//

    public static string STRING_USER_STAR= "USER_START";
	public static string STRING_USER_STAR_NUMBER= "";

	

	public static void loadGame()
	{
		//PlayerPrefs.DeleteAll();
		UserName = PlayerPrefs.GetString(STRING_USER_NAME);
		if (UserName.Length <= 4)
			UserName = "NaN";
		
		bestScore = PlayerPrefs.GetInt(STRING_USER_SCORE_1);
		levelUnBlock = PlayerPrefs.GetInt(STRING_USER_LEVEL);
		//if(levelUnBlock == 0)
		//	levelUnBlock = 1;
		Debug.Log("LevelUnblock : " + levelUnBlock);
		STRING_USER_STAR_NUMBER = PlayerPrefs.GetString(STRING_USER_STAR);

		string[] strarray;
		strarray = STRING_USER_STAR_NUMBER.Split('|');
        Debug.Log("STRING_USER_STAR_NUMBER:" + STRING_USER_STAR_NUMBER);
		Debug.Log("strarray.Length:" + strarray.Length);
		for(int i =0;i <strarray.Length;i++)
		{
            if (i < levelUnBlock)
            {
                Debug.Log("Star : " + strarray[i]);
                SelectLevel.starArray[i] = int.Parse(strarray[i]);
            }
		}
	}

	public static void saveGame()
	{
		PlayerPrefs.SetString(STRING_USER_NAME, UserName);
        PlayerPrefs.SetInt(STRING_USER_SCORE_1, bestScore);
        PlayerPrefs.SetInt(STRING_USER_LEVEL, levelUnBlock);
		string str ="";
		for(int i =1;i <=levelUnBlock;i++)
		{
			str +=( SelectLevel.starArray[i].ToString() + "|");
		}
		PlayerPrefs.SetString(STRING_USER_STAR, str);
		Debug.Log("Str Save :"+str);
		PlayerPrefs.Save();
	}

	public static void setBestScore(int score)
	{
		if (score > 0 && score > bestScore)
		{
			bestScore = score;
		}
	}

    public static void setLevelUnblock(int level)
    {
        Debug.Log("--------------");
        Debug.Log("levelUnBlock " + levelUnBlock);
        if(levelUnBlock < level)
            levelUnBlock = level;
        Debug.Log("levelUnBlock " + levelUnBlock);
    }
}
