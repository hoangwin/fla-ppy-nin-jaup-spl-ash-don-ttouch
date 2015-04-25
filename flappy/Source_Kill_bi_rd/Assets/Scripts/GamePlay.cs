using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour {
	GameObject BottomBG;
	//public GameObject BirdObject;
	public ArrayList ListBirdObject;
	public GameObject PanelOverGame;
	public GameObject ReadyAnim;



	public static float speedx = 0.06f ;
	public static GamePlay instance;
	public static int currentState = 0;
	public const int STATE_WATTING = 0;
	public const int STATE_PLAY = 1;
	public const int STATE_DROP = 2;
	public const int STATE_OVER = 3;
	public static float TimePlayedSubState = 0f;
	public static float TimelastCreateBird = 0f;
	public static float TimeOffsetCreateBird = 0f;
	// Use this for initialization
	void Start () {
		DEF.Init();
		GameObject hand = GameObject.Find("BackGround");
		BottomBG = GameObject.Find("BottomBG");
		ListBirdObject = new ArrayList ();
		ReadyAnim =  GameObject.Find("ReadyAnim");
		PanelOverGame  =  GameObject.Find("PanelOverGame");
		//BirdObject  =  GameObject.Find("BirdObject");
		//BirdObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
		//GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new  Vector3(DEF.scaleX, DEF.scaleY, 1));



		if(hand!= null)
		{
			//	DEF.ResizeBySize(hand,DEF.DISPLAY_WIDTH,DEF.DISPLAY_HEIGHT);
			hand.transform.localScale = new Vector3 (DEF.sx_ortho, DEF.sy_ortho, 1);
			BottomBG.transform.localScale = new Vector3 (DEF.sx_ortho, DEF.sy_ortho, 1);
		}
		currentState = STATE_WATTING;
		instance = this;
		NGUITools.SetActive(PanelOverGame,false);   
		ScoreControl.setDefaultScore ();
		DEF.ScaleAnchorGui();
		restart ();
		SoundEngine.playSound ("SoundGo");
		//GameObject sound = GameObject.Find("SoundGo");
		//if (sound != null)
		//{
		//	sound.audio.Play();
		//}
	}
	
	// Update is called once per frame
	void Update () 
	{
		TimePlayedSubState += Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.LoadLevel ("MainMenu");
		}

		if (currentState == STATE_PLAY) {
			if (Input.GetMouseButtonDown (0) ||  ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began))) {
				Trap trap = GameObject.Find("TrapPrefab").GetComponent<Trap>();
				Animator ani = GameObject.Find("TrapPrefab").GetComponent<Animator>();

				if(trap.state == 0)
				{
					ani.Play("Trap_Open");
					trap.state = 1;
				}
				SoundEngine.playSound ("SoundFly");
				//GameObject sound = GameObject.Find("SoundFly");
				//if (sound != null)
				//{
					// Debug.Log("Play Sound");
				//	sound.audio.Play();
				//}
			}		
		}
		if(currentState ==STATE_PLAY)
		{
			if (ListBirdObject.Count < 20 && TimePlayedSubState > TimelastCreateBird+TimeOffsetCreateBird ) {
					GameObject a = (GameObject)Instantiate (Resources.Load ("BirdPrefab"));			
				a.layer = 11;
				Physics2D.IgnoreLayerCollision (11, 11, true);
					ListBirdObject.Add (a);
				TimelastCreateBird = TimePlayedSubState;
				TimeOffsetCreateBird = Random.Range(0.2f,0.8f);
				}
		}
	

		GameObject obj;
		for (int i= ListBirdObject.Count-1; i>-1; i--) {
			obj = (GameObject)ListBirdObject [i];
			if(!(obj.transform.Find("BirdObjectAnim"). GetComponent<BirdAnim>().isLive))
			{
				ListBirdObject.RemoveAt (i);
				DestroyImmediate (obj);
			}
		}

		switch (currentState) {
			case GamePlay.STATE_DROP:
			//(TimePlayedSubState >1)
				showGameOver();
			break;
		}

	}
	public void setbegin()
	{

		//if (GamePlay.STATE_WATTING == currentState) {
		GamePlay.currentState = GamePlay.STATE_PLAY;
		ReadyAnim.GetComponent<SpriteRenderer> ().enabled = false;
		//GameObject.Find ("ReadyAnim").SetActive (false);
			//	}
	}
	void FixedUpdate()
	{
			if (GamePlay.currentState == GamePlay.STATE_PLAY) {
				GameObject mobject;
			/*
				for (int i= 0; i<trapList.Count; i++) {
						mobject = (GameObject)trapList [i];
						mobject.transform.localPosition = new Vector3 (mobject.transform.localPosition.x - speedx, mobject.transform.localPosition.y, 1);
				}
				*/
			}	
	}

	public void restart() 
	{
		//GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new  Vector3(DEF.scaleX, DEF.scaleY, 1));
		GameObject obj;
		for (int i= ListBirdObject.Count-1; i>-1; i--) {
			obj = (GameObject)ListBirdObject [i];
			ListBirdObject.RemoveAt (i);
			DestroyImmediate (obj);
		}

		instance = this;
		NGUITools.SetActive(PanelOverGame,false);
		if(GamePlay.currentState == GamePlay.STATE_OVER)
			GamePlay.currentState = GamePlay.STATE_PLAY;
		else
			GamePlay.currentState = GamePlay.STATE_WATTING;
		//ReadyAnim.GetComponent<SpriteRenderer>().enabled = true;
		//GameObject.Find ("ReadyAnim").SetActive (true);
		//Animator ani = GameObject.Find ("ReadyAnim").GetComponent<Animator> ();

		//ani.Play ("ReadyGo");



		ScoreControl.setDefaultScore ();
		GameObject.Find("LabelScoreInGame").GetComponent<UILabel>().text =""+ ScoreControl.getRealScore();
		//BirdObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
		TimelastCreateBird = 0;
		TimePlayedSubState = 0;
		TimeOffsetCreateBird = 0;
		ScoreControl.Score = 0;
		ScoreControl.setDefaultScore();
		GameObject.Find("LabelScoreInGame").GetComponent<UILabel>().text =""+ ScoreControl.getRealScore();
	}
	public void initGameOver()
	{
		BoxCollider2D [] arraycollider;

		for (int i= ListBirdObject.Count-1; i>-1; i--) {
			((GameObject) ListBirdObject[i]).transform.rigidbody2D.rigidbody2D.velocity = new Vector2(0,0);
			((GameObject) ListBirdObject[i]).transform.rigidbody2D.rigidbody2D.gravityScale =0f;
			((GameObject) ListBirdObject[i]).transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, 0));


			arraycollider =((GameObject) ListBirdObject[i]).GetComponents<BoxCollider2D>();
			for(int j =0; j<arraycollider.Length;j++)
				arraycollider[j].enabled = false;
		}

	}
	public void showGameOver()
	{
		currentState = STATE_OVER;
		NGUITools.SetActive(GamePlay.instance.PanelOverGame,true);
		if (ScoreControl.Score > ScoreControl.BestScore)
			ScoreControl.BestScore = ScoreControl.Score;
		GameObject.Find("LabelScore").GetComponent<UILabel>().text =""+ ScoreControl.getRealScore();
		GameObject.Find("LabelScoreBest").GetComponent<UILabel>().text =""+ ScoreControl.getRealBestScore();
		
		ScoreControl.saveGame ();
	}
}
