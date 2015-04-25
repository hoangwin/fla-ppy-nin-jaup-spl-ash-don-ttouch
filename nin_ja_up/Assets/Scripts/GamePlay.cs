using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour {
	GameObject BottomBG;
	GameObject BottomBG1;
	public GameObject BirdObject;
	public GameObject PanelOverGame;
	public GameObject UI_READY;
    public GameObject ButtonRating;
    public GameObject BGScore;
    public GameObject PlatformBegin;
    public UILabel LabelGameScore;
    public ArrayList trapList;
	public static float speedx = 0.06f ;
	public static GamePlay instance;
	public static int currentState = 0;
	public const int STATE_WATTING = 0;
	public const int STATE_PLAY = 1;
	public const int STATE_DROP = 2;
	public const int STATE_OVER = 3;
	public static float TimePlayedSubState = 0f;
	// Use this for initialization
	void Start () {
		DEF.Init();
		GameObject hand = GameObject.Find("BackGround");
		BottomBG = GameObject.Find("BG");
		BottomBG1 = GameObject.Find("BG1");
		UI_READY.SetActive(true);
        BGScore.SetActive(false);
		PanelOverGame.SetActive(true);
        trapList = new ArrayList();
        ScoreControl.loadGame();
	//	BirdObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
	

		if(hand!= null)
		{
			//	DEF.ResizeBySize(hand,DEF.DISPLAY_WIDTH,DEF.DISPLAY_HEIGHT);
			hand.transform.localScale = new Vector3 (DEF.sx_ortho, DEF.sy_ortho, 1);
			BottomBG.transform.localScale = new Vector3 (DEF.sx_ortho, DEF.sy_ortho, 1);
			BottomBG1.transform.localScale = new Vector3 (DEF.sx_ortho, DEF.sy_ortho, 1);
			Debug.Log("aaaaaaaaaaa");
		}
		currentState = STATE_WATTING;
		instance = this;
		NGUITools.SetActive(PanelOverGame,false);
        if (ScoreControl.mPlayed < 3)
            ButtonRating.SetActive(false);
		ScoreControl.setDefaultScore ();
		DEF.ScaleAnchorGui();

	}
	
	// Update is called once per frame
	void Update () 
	{
		TimePlayedSubState += Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.Escape)) {
            Application.Quit();// ("MainMenu");
		}
       

		switch (currentState) {
			case GamePlay.STATE_DROP:
			if(TimePlayedSubState >1)
				showGameOver();
			break;
		}

        GameObject obj;		
        for (int i = trapList.Count - 1; i > -1; i--)
        {
            obj = (GameObject)trapList[i];
            if (obj.transform.position.y < CameraFollow.instance._transform.position.y - 5)
            {
                trapList.RemoveAt(i);
                DestroyImmediate(obj);
            }
        }

	}
	void FixedUpdate()
	{
		if (GamePlay.currentState == GamePlay.STATE_PLAY || GamePlay.currentState == GamePlay.STATE_WATTING) 
		{
            
			if (BottomBG.transform.localPosition.y < CameraFollow.instance._transform.position.y - 25.5f)
                BottomBG.transform.localPosition = new Vector3(BottomBG1.transform.localPosition.x, BottomBG.transform.localPosition.y + 50f, 1);
			else
				BottomBG.transform.localPosition = new Vector3 (BottomBG.transform.localPosition.x, BottomBG.transform.localPosition.y, 1);

            if (BottomBG1.transform.localPosition.y < CameraFollow.instance._transform.position.y - 25.5f)
                BottomBG1.transform.localPosition = new Vector3(BottomBG1.transform.localPosition.x, BottomBG1.transform.localPosition.y + 50f, 1);
			else
				BottomBG1.transform.localPosition = new Vector3 (BottomBG1.transform.localPosition.x, BottomBG1.transform.localPosition.y, 1);
             
		}
	

	//	BottomBG1.transform.localPosition = new Vector3 (BottomBG1.transform.localPosition.x -speedx, BottomBG1.transform.localPosition.y, 1);
        LabelGameScore.text = "" + ScoreControl.getRealScore() +"M";
	}

	public void restart() 
	{
		//GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new  Vector3(DEF.scaleX, DEF.scaleY, 1));
		GameObject obj;		
        for (int i= trapList.Count-1; i>-1; i--) {
			obj = (GameObject)trapList [i];
			trapList.RemoveAt (i);
			DestroyImmediate (obj);
		}

        BottomBG.transform.localPosition = new Vector3(BottomBG1.transform.localPosition.x,0, 1);
        BottomBG1.transform.localPosition = new Vector3(BottomBG1.transform.localPosition.x,25.5f, 1);
		instance = this;
        CameraFollow.instance.reset();
		NGUITools.SetActive(PanelOverGame,false);
		GamePlay.currentState = GamePlay.STATE_WATTING;
		UI_READY.SetActive(true);
        BGScore.SetActive(false);
        PlatformBegin.SetActive(true);
        Platform.instance._Platform.SetActive(false);
		
		ScoreControl.setDefaultScore ();
		LabelGameScore.text =""+ ScoreControl.getRealScore();
        Player.instance.restart();
}
	public void initGameOver()
	{
        /*
		BoxCollider2D [] arraycollider;
		for (int i= trapList.Count-1; i>-1; i--) {
			arraycollider =((GameObject) trapList[i]).GetComponents<BoxCollider2D>();
			for(int j =0; j<arraycollider.Length;j++)
				arraycollider[j].enabled = false;
		}
         */
	}
	public void showGameOver()
	{
        Debug.Log("Game Over");
		NGUITools.SetActive(GamePlay.instance.PanelOverGame,true);
		if (ScoreControl.Score > ScoreControl.BestScore)
			ScoreControl.BestScore = ScoreControl.Score;
		GameObject.Find("LabelScore").GetComponent<UILabel>().text =""+ ScoreControl.getRealScore();
		GameObject.Find("LabelScoreBest").GetComponent<UILabel>().text =""+ ScoreControl.getRealBestScore();
        ScoreControl.mPlayed++;
		ScoreControl.saveGame ();
        if (ScoreControl.mPlayed > 3)
            ButtonRating.SetActive(true);
        currentState = STATE_OVER;
	}

    
}
