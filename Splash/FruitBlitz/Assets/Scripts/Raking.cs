using UnityEngine;
using System.Collections;

public class Raking : MonoBehaviour {
	public WWW www;
	public static bool loadRanking = true;
	public static bool isMoveFromMainmenu = false;

    public static string[] username1 = new string[11];
	public static string[] Score1 = new string[11];

	public static string myPos = "";
	public static string myScore = "";

    

	public GameObject PanelBoard;
	public GameObject PanelInputName;
	public static Raking instance;
	void Start () {
		DEF.Init();
	//	GameObject hand = GameObject.Find("Background");
		PanelBoard = GameObject.Find("PanelBoard");
		PanelInputName = GameObject.Find("PanelInputName");
		//GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new  Vector3(DEF.scaleX, DEF.scaleY, 1));
		//if(hand!= null)
		{
			//	DEF.ResizeBySize(hand,DEF.DISPLAY_WIDTH,DEF.DISPLAY_HEIGHT);
		//	hand.transform.localScale = new Vector3 (DEF.sx_ortho, DEF.sy_ortho, 1);
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
        www = new WWW("http://gamethuanviet.com/fruitblitzlite/SetGetData.php?type=select&username=" + ScoreControl.UserName);
        Debug.Log("http://gamethuanviet.com/fruitblitzlite/SetGetData.php?type=select&username=" + ScoreControl.UserName);
		
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
            if(strarray.Length >20)
            {
                for(int i =0;i<10;i++)
                {
                    username1[i] = strarray[i*2 +1];
					Score1[i] = strarray[2+i*2];
					if(Score1[i].Length>5)
						Score1[i] = Score1[i].Substring(0,5);
                    
				}
                if (strarray.Length > 23)
                {
                    myPos = strarray[21];
                    myScore = strarray[23];
                }
					
			
			}
            ShowScoreTOUI();
		}
	}

    public void ShowScoreTOUI()
    {
        
            for (int i = 0; i < 10; i++)
			{
				GameObject.Find(("Name" +(i+1))).GetComponent<UILabel>().text =username1[i];
				GameObject.Find(("Score" + (i + 1))).GetComponent<UILabel>().text = Score1[i];
			}
            GameObject.Find("Pos11").GetComponent<UILabel>().text = myPos;
            GameObject.Find("Name11").GetComponent<UILabel>().text = ScoreControl.UserName;
			GameObject.Find("Score11").GetComponent<UILabel>().text = myScore;

         
    }
	public void PostHightScore()
	{
        //http://gamethuanviet.com/fruitblitzlite/SetGetData.php?type=update&username=%s&Score=%d&Level=0&Played=0&country=NA "
        string strPost = "http://gamethuanviet.com/fruitblitzlite/SetGetData.php?type=update&username=" + ScoreControl.UserName + "&Score=" + ScoreControl.bestScore.ToString() + "&Level=0&Played=0&country=NA";
		strPost = strPost.Replace(",",".");
		Debug.Log(strPost);
		WWW www1 = new WWW(strPost);
	}
	// Use this for initialization

}
