using UnityEngine;
using System.Collections;

public class Raking : MonoBehaviour {
	public WWW www;
	public static bool loadRanking = true;
	public static bool isMoveFromMainmenu = false;

    public static string[] username1 = new string[11];
	public static string[] Score1 = new string[11];
    public static string[] username2 = new string[11];
	public static string[] Score2 = new string[11];
    public static string[] username3 = new string[11];
	public static string[] Score3 = new string[11];
	public static string[] myPos = new string[3];
	public static string[] myScore = new string[3];

    public static int typeRank;

	public GameObject PanelBoard;
	public GameObject PanelInputName;
	public static Raking instance;
	void Start () {
		DEF.Init();
		GameObject hand = GameObject.Find("Background");
		PanelBoard = GameObject.Find("PanelBoard");
		PanelInputName = GameObject.Find("PanelInputName");
		//GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new  Vector3(DEF.scaleX, DEF.scaleY, 1));
		if(hand!= null)
		{
			//	DEF.ResizeBySize(hand,DEF.DISPLAY_WIDTH,DEF.DISPLAY_HEIGHT);
			hand.transform.localScale = new Vector3 (DEF.sx_ortho, DEF.sy_ortho, 1);
		}

		if (ScoreControl.UserName.Length >= 5)
		{
			PostHightScore ();

			getHightScore ();
			loadRanking = true;
			NGUITools.SetActive(PanelInputName,false);  
		} 
		else
		{
			loadRanking = false;
			NGUITools.SetActive(PanelBoard,false);  

		}
		DEF.ScaleAnchorGui();

		instance = this;
        typeRank = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("MainMenu");
			
		}
		getHightScoreDone();
	}
	
	public void  getHightScore()
	{
		www = new WWW("http://gamethuanviet.com/dontstepthewrongtile/SetGetData.php?type=select&username="+ ScoreControl.UserName);
		Debug.Log("http://gamethuanviet.com/dontstepthewrongtile/SetGetData.php?type=select&username=" + ScoreControl.UserName);
		
	}
    public static void RankingchangeType(int newrankingType)
    {
        typeRank = newrankingType;
        if (typeRank < 0) typeRank = 2;
        if (typeRank > 2) typeRank = 0;
        instance.ShowScoreTOUI();
    }
	public void  getHightScoreDone()
	{
		if(loadRanking && www.isDone)
		{
			if(www.bytesDownloaded <20)
				return;
				

			loadRanking = false;
			Debug.Log(www.text);
			string[] strarray;
			strarray = www.text.Split('|');
            Debug.Log("strarray.Length:" + strarray.Length);
            if(strarray.Length >63)
            {
                for(int i =0;i<10;i++)
                {
                    username1[i] = strarray[i*2];
					Score1[i] = strarray[1+i*2];
					if(Score1[i].Length>5)
						Score1[i] = Score1[i].Substring(0,5);
                    username2[i] =strarray[20+i*2];
					Score2 [i]= strarray[20+1+i*2];
					username3[i] =strarray[40+i*2];
					Score3[i] = strarray[40+1 + i * 2];
					if(Score3[i].Length>5)
						Score3[i] = Score3[i].Substring(0,5);
				}
				myPos[0] = strarray[60];
                myPos[1] = strarray[61];
                myPos[2] = strarray[62];

				if(strarray.Length>67)
				{
					myScore[0] = strarray[64];
					myScore[1] = strarray[65];
					myScore[2] = strarray[66];
				}
				else 
				{
					myScore[0] = ScoreControl.Score1.ToString();
					myScore[1] = ScoreControl.Score2.ToString();
					myScore[2] =  ScoreControl.Score3.ToString();
				}
				if(myScore[0].Length>5)
					myScore[0] = myScore[0].Substring(0,5);
				if(myScore[1].Length>5)
					myScore[1] = myScore[1].Substring(0,5);
				if(myScore[2].Length>5)
					myScore[2] = myScore[2].Substring(0,5);
			}
            ShowScoreTOUI();
		}
	}

    public void ShowScoreTOUI()
    {
        switch(typeRank)
        {
            case 0:
        
            for (int i = 0; i < 10; i++)
			{
				GameObject.Find(("Name" +(i+1))).GetComponent<UILabel>().text =username1[i];
				GameObject.Find(("Score" + (i + 1))).GetComponent<UILabel>().text = Score1[i];
			}
                GameObject.Find("Pos11").GetComponent<UILabel>().text = myPos[0];
                GameObject.Find("Name11").GetComponent<UILabel>().text = ScoreControl.UserName;
			GameObject.Find("Score11").GetComponent<UILabel>().text = myScore[0];
                GameObject.Find("LabelTypeRank").GetComponent<UILabel>().text = "60 Step";
                GameObject.Find("Score").GetComponent<UILabel>().text = "Time";
                
            break;
            case 1:

            for (int i = 0; i < 10; i++)
            {
                GameObject.Find(("Name" + (i + 1))).GetComponent<UILabel>().text = username2[i];
                GameObject.Find(("Score" + (i + 1))).GetComponent<UILabel>().text = Score2[i];
            }

                GameObject.Find("Pos11").GetComponent<UILabel>().text = myPos[1];
                GameObject.Find("Name11").GetComponent<UILabel>().text = ScoreControl.UserName;
                GameObject.Find("Score11").GetComponent<UILabel>().text = myScore[1];

                GameObject.Find("LabelTypeRank").GetComponent<UILabel>().text = "Time Attack";
                GameObject.Find("Score").GetComponent<UILabel>().text = "Step";
            break;
            case 2:

            for (int i = 0; i < 10; i++)
            {
                GameObject.Find(("Name" + (i + 1))).GetComponent<UILabel>().text = username3[i];
				GameObject.Find(("Score" + (i + 1))).GetComponent<UILabel>().text = Score3[i];
			}
			
            GameObject.Find("Pos11").GetComponent<UILabel>().text = myPos[2];
            GameObject.Find("Name11").GetComponent<UILabel>().text = ScoreControl.UserName;
			GameObject.Find("Score11").GetComponent<UILabel>().text = myScore[2];
            GameObject.Find("LabelTypeRank").GetComponent<UILabel>().text = "Arcade";
            GameObject.Find("Score").GetComponent<UILabel>().text = "Time";
            break;
    }
         
    }
	public void PostHightScore()
	{
		//http://gamethuanviet.com/baucuatomca/SetGetData.php?type=update&username=%s&Score=%d&Level=0&Played=0&country=NA "
        string strPost = "http://gamethuanviet.com/dontstepthewrongtile/SetGetData.php?type=update&username=" + ScoreControl.UserName + "&Score1=" + ScoreControl.Score1.ToString() + "&Score2=" + ScoreControl.Score2.ToString() + "&Score3=" +ScoreControl.Score3.ToString() + "&Level=0&Played=0&country=NA";
		strPost = strPost.Replace(",",".");
		Debug.Log(strPost);
		WWW www1 = new WWW(strPost);
	}
	// Use this for initialization

}
