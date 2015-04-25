using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour {
	GameObject BottomBG;
	GameObject BottomBG1;
	public GameObject BirdObject;
	public GameObject PanelOverGame;
	public GameObject UI_READY;
	ArrayList trapList;
	GameObject lastTrap;
	GameObject fistTrap;
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
		BottomBG = GameObject.Find("BottomBG");
		BottomBG1 = GameObject.Find("BottomBG1");
		UI_READY =  GameObject.Find("UI_READY");
		PanelOverGame  =  GameObject.Find("PanelOverGame");
		BirdObject  =  GameObject.Find("BirdObject");
		BirdObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
		//GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new  Vector3(DEF.scaleX, DEF.scaleY, 1));
		trapList = new ArrayList ();
		fistTrap = GameObject.Find ("Trap");
		lastTrap = (GameObject)Instantiate (Resources.Load ("TrapPrefab")); 

		lastTrap.transform.localPosition = new Vector3 (fistTrap.transform.localPosition.x , fistTrap.transform.localPosition.y, 1);
		trapList.Add (lastTrap);

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
		ScoreControl.setDefaultScore ();
		DEF.ScaleAnchorGui();

	}
	
	// Update is called once per frame
	void Update () 
	{
		TimePlayedSubState += Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.LoadLevel ("MainMenu");
		}
		if (lastTrap != null) {
				if (lastTrap.transform.localPosition.x < 4) {
						GameObject a = (GameObject)Instantiate (Resources.Load ("TrapPrefab")); 
						float r = Random.Range (30, 40) / 10f;
						float y = Random.Range (-5, 30) / 10f;
						a.transform.localPosition = new Vector3 (lastTrap.transform.localPosition.x + r, y, 1);
						trapList.Add (a);
						lastTrap = a;
				}
		}
		if (trapList != null) {
				if (trapList.Count > 0) {
						GameObject obj = (GameObject)trapList [0];
						if ((!obj.GetComponent<Trap> ().isPass) && obj.GetComponent<Trap> ().checkPass (obj.transform.localPosition.x, BirdObject.transform.localPosition.x)) {
								ScoreControl.Score++;
								GameObject.Find ("LabelScoreInGame").GetComponent<UILabel> ().text = "" + ScoreControl.getRealScore ();
								obj.GetComponent<Trap> ().isPass = true;
					GameObject sound = GameObject.Find("SoundCoin");
					if (sound != null)
					{
						// Debug.Log("Play Sound");
						sound.audio.Play();
					}
						}
				}
				if (trapList.Count > 0) {
						GameObject obj = (GameObject)trapList [0];
						if (obj.transform.localPosition.x < -4) {
								trapList.RemoveAt (0);
								DestroyImmediate (obj);
						}
				}
		}

		switch (currentState) {
			case GamePlay.STATE_DROP:
			if(TimePlayedSubState >1)
				showGameOver();
			break;
		}

	}
	void FixedUpdate()
	{
		if (GamePlay.currentState == GamePlay.STATE_PLAY || GamePlay.currentState == GamePlay.STATE_WATTING) 
		{
			if (BottomBG.transform.localPosition.x < -8)
					BottomBG.transform.localPosition = new Vector3 (BottomBG1.transform.localPosition.x + 8 - speedx, BottomBG.transform.localPosition.y, 1);
				else
					BottomBG.transform.localPosition = new Vector3 (BottomBG.transform.localPosition.x - speedx, BottomBG.transform.localPosition.y, 1);
				
			if (BottomBG1.transform.localPosition.x < -8)
				BottomBG1.transform.localPosition = new Vector3 (BottomBG.transform.localPosition.x + 8 - speedx, BottomBG.transform.localPosition.y, 1);
			else
				BottomBG1.transform.localPosition = new Vector3 (BottomBG1.transform.localPosition.x - speedx, BottomBG.transform.localPosition.y, 1);
		}
	

	//	BottomBG1.transform.localPosition = new Vector3 (BottomBG1.transform.localPosition.x -speedx, BottomBG1.transform.localPosition.y, 1);
		if (GamePlay.currentState == GamePlay.STATE_PLAY) {
				GameObject mobject;
				for (int i= 0; i<trapList.Count; i++) {
						mobject = (GameObject)trapList [i];
						mobject.transform.localPosition = new Vector3 (mobject.transform.localPosition.x - speedx, mobject.transform.localPosition.y, 1);
				}
			}	
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
		lastTrap = (GameObject)Instantiate (Resources.Load ("TrapPrefab"));		
		lastTrap.transform.localPosition = new Vector3 (fistTrap.transform.localPosition.x , fistTrap.transform.localPosition.y, 1);
		trapList.Add (lastTrap);
		instance = this;
		NGUITools.SetActive(PanelOverGame,false);
		GamePlay.currentState = GamePlay.STATE_WATTING;
		UI_READY.GetComponent<SpriteRenderer>().enabled = true;	
		BirdObject.transform.localPosition = new Vector3 (-1.5f , 1f, 1);
		ScoreControl.setDefaultScore ();
		GameObject.Find("LabelScoreInGame").GetComponent<UILabel>().text =""+ ScoreControl.getRealScore();
		BirdObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
	}
	public void initGameOver()
	{
		BoxCollider2D [] arraycollider;
		for (int i= trapList.Count-1; i>-1; i--) {
			arraycollider =((GameObject) trapList[i]).GetComponents<BoxCollider2D>();
			for(int j =0; j<arraycollider.Length;j++)
				arraycollider[j].enabled = false;
		}
	}
	public void showGameOver()
	{
		NGUITools.SetActive(GamePlay.instance.PanelOverGame,true);
		if (ScoreControl.Score > ScoreControl.BestScore)
			ScoreControl.BestScore = ScoreControl.Score;
		GameObject.Find("LabelScore").GetComponent<UILabel>().text =""+ ScoreControl.getRealScore();
		GameObject.Find("LabelScoreBest").GetComponent<UILabel>().text =""+ ScoreControl.getRealBestScore();
		
		ScoreControl.saveGame ();
	}
}
