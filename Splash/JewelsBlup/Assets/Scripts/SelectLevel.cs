using UnityEngine;
using System.Collections;

public class SelectLevel : MonoBehaviour {


    public static int MAX_PAGE = 5;
    public static int mcurrentPage = 0;
    public static int mcurrentLevel = 0;
	public static int[] starArray = new int[82];
	void Start () {
        mcurrentPage = ScoreControl.levelUnBlock / 16;
        changePage();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }
	}
    public static void changePage()
    {
        GameObject.Find("LabelPage").GetComponent<UILabel>().text = (mcurrentPage + 1).ToString() + "/" + MAX_PAGE.ToString();
        int level = 0;
        for (int i = 1; i < 17; i++)
        {
         
			level = mcurrentPage * 16 + i -1;
			//Debug.Log(" " + level + "," + ScoreControl.levelUnBlock);
            if (level < ScoreControl.levelUnBlock)
            {
                if (SelectLevel.starArray[level] == 2)
                    GameObject.Find("Background" + i.ToString()).transform.parent.GetComponent<UIButton>().normalSprite = "MenuLevelNormal3Star";
                else if (SelectLevel.starArray[level] == 1)
					GameObject.Find("Background" + i.ToString()).transform.parent.GetComponent<UIButton>().normalSprite = "MenuLevelNormal2Star";
                else
					GameObject.Find("Background" + i.ToString()).transform.parent.GetComponent<UIButton>().normalSprite = "MenuLevelNormal1Star";
                GameObject.Find("LabelButton" + i.ToString()).GetComponent<UILabel>().text = (mcurrentPage * 16 + i).ToString();
            }
			else if (level == (ScoreControl.levelUnBlock))
            {
				GameObject.Find("Background" + i.ToString()).transform.parent.GetComponent<UIButton>().normalSprite = "MenuLevelNormal";
                GameObject.Find("LabelButton" + i.ToString()).GetComponent<UILabel>().text = (mcurrentPage * 16 + i).ToString();
			}
			else
			{
				GameObject.Find("Background" + i.ToString()).transform.parent.GetComponent<UIButton>().normalSprite = "MenuLevelLock";
                GameObject.Find("LabelButton" + i.ToString()).GetComponent<UILabel>().text = " ";
            }


        }
    }

   


}
