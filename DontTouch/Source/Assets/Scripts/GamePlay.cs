using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour {

	
	public GameObject PanelOverGame;
	public GameObject UI_READY;
	ArrayList trapList;
	GameObject lastTrap;
	GameObject fistTrap;
    public Sprite SpriteTrap1;
    public Sprite SpriteTrap2;
    public Sprite SpriteTrapFailRight;
    public Sprite SpriteTrapFailLeft;
    public Sprite SpriteTrapOKRight;
    public Sprite SpriteTrapOKLeft;
    public int stepFoot;
	public float timePlay;
    public GameObject starObject;
    public GameObject finishObject;
    UILabel labelScore;

	public static float speedy = 2.0f ;

    public static int gameMode = 0;
    public const int GAME_MODE_CLASSIC = 0; //tap 60 tinh thoi gian
    public const int GAME_MODE_TIME_ATTACK = 1;//Trong 10s thang nao di nhieu hon
    public const int GAME_MODE_TIME_UNLESS = 2;//Khong GIoi Han thoi gian, met thi nghi

    public int typeOver;
    public const int TYPE_OVER_FINISH_60_STEP = 0;
    public const int TYPE_OVER_FINISH_TIME_ATTCK = 1;
    public const int TYPE_OVER_FINISH_ARCADE = 2;
    public const int TYPE_OVER_FAILT = 3;


    public int modeClassicMaxStep = 60;
    public int modeTimeAttackMaxtime = 60;

    
    public static bool isMove = false;
	
    
    public static GamePlay instance;
	public static int currentState = 0;
	public const int STATE_WATTING = 0;
	public const int STATE_PLAY = 1;
	public const int STATE_DROP = 2;
	public const int STATE_WATTING_OVER = 3;
    public const int STATE_OVER = 4;
	public static float TimePlayedSubState = 0f;
    public static float TILE_X = 0;
    public static float TILE_Y = 0;
    public static float BEGIN_X = 0;
    Vector3 touchPostionV3;    

	// Use this for initialization

    //FrameRate
    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    Vector3 v3Move;
    //end framerate
	void Start () {
		DEF.Init();
		GameObject hand = GameObject.Find("BackGround");
		//GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new  Vector3(DEF.scaleX, DEF.scaleY, 1));
		trapList = new ArrayList ();
		fistTrap = GameObject.Find ("Trap");
        labelScore = GameObject.Find("LabelScoreInGame").GetComponent<UILabel>();
       // createNewTrap();
       
        TILE_X = fistTrap.GetComponent<SpriteRenderer>().bounds.size.x;
        TILE_Y = fistTrap.GetComponent<SpriteRenderer>().bounds.size.y;
        BEGIN_X = -TILE_X - TILE_X/2;

        
        isMove = false;

        InitAllTrap();

		if(hand!= null)
		{
			//	DEF.ResizeBySize(hand,DEF.DISPLAY_WIDTH,DEF.DISPLAY_HEIGHT);
			hand.transform.localScale = new Vector3 (DEF.sx_ortho, DEF.sy_ortho, 1);
		}

		currentState = STATE_WATTING;
		instance = this;
		NGUITools.SetActive(PanelOverGame,false);  	
		DEF.ScaleAnchorGui();
        
        timeleft = updateInterval;
        v3Move = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame

     //   Application.targetFrameRate = 30; 
    void destroyTrap()
    {
        		if (trapList != null) {
				if (trapList.Count > 0) {
						//GameObject obj = (GameObject)trapList [0];
						
				}
				if (trapList.Count > 0) {
						GameObject obj = (GameObject)trapList [0];
						if (obj.transform.localPosition.y < -4.2) {
                            if (obj.GetComponent<Trap>().type == Trap.TYPE_CAN_TOUCH)
                            {
								SoundEngine.playSound("SoundFail");
                                currentState = STATE_WATTING_OVER;
								timePlay  = TimePlayedSubState;
								TimePlayedSubState =0;
								if(gameMode == GAME_MODE_TIME_UNLESS)
									typeOver = TYPE_OVER_FINISH_ARCADE;
								else
                                	typeOver = TYPE_OVER_FAILT;
                            }
							trapList.RemoveAt (0);
							DestroyImmediate (obj);
						}
				}
		}
    }
    void Update() 
	{
		TimePlayedSubState += Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.LoadLevel ("MainMenu");
		}
		if (lastTrap != null) {
				if (lastTrap.transform.localPosition.y < 4) {
                    createNewLineTrap();
				}
		}
        destroyTrap();

		switch (currentState) {
            case STATE_WATTING:
                switch (gameMode)
                {
                    case GAME_MODE_CLASSIC:
                        UpdateGame();
                        break;
                    case GAME_MODE_TIME_ATTACK:
                        labelScore.text = string.Format("Step : 0     Time : "+modeTimeAttackMaxtime +":00");
                        UpdateGame();
                        break;
                    case GAME_MODE_TIME_UNLESS:
                        if (Input.GetMouseButtonDown(0) || ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began)))
                        {
                            currentState = GamePlay.STATE_PLAY;
                            TimePlayedSubState = 0;
                            checkTouch();
                            UI_READY.GetComponent<SpriteRenderer>().enabled = false;
                            SoundEngine.playSound("SoundFly");
                            

                        }
                        break;

                }
            break;
            case STATE_PLAY:

            DisplayTimer();

            UpdateGame();
            break;
            case STATE_WATTING_OVER:
                if (TimePlayedSubState > 0.5f)
                {
                    showGameOver();
                    
                    TimePlayedSubState = 0;
					SoundEngine.playSound("SoundFinish");
                    currentState = STATE_OVER;
                }
                break;
			case GamePlay.STATE_DROP:
			//if(TimePlayedSubState >0.5f)
			//	showGameOver();
			break;
		}
    }

    void DisplayTimer()
    {
        if (gameMode == GAME_MODE_TIME_ATTACK)
        {
            if (TimePlayedSubState <= 100)
            {
                labelScore.text = string.Format("Step : " + stepFoot + "     Time : {0:00.00}", (modeTimeAttackMaxtime - TimePlayedSubState)).Replace(".", ":");
            }
            else
            {
                labelScore.text = string.Format("Step : " + stepFoot + "     Time : {0:000.00}", (modeTimeAttackMaxtime - TimePlayedSubState)).Replace(".", ":");
            }
        }
        else
        {
            if (TimePlayedSubState <= 100)
            {
                labelScore.text = string.Format("Step : " + stepFoot + "     Time : {0:00.00}", TimePlayedSubState).Replace(".", ":");
            }
            else
            {
                labelScore.text = string.Format("Step : " + stepFoot + "     Time : {0:000.00}", TimePlayedSubState).Replace(".", ":");
            }
        }
    }

    public void InitAllTrap()
    {
        trapList.Add(createNewTrap(0, Trap.TYPE_CANT_TOUCH, 0, -3*TILE_Y/2+ 0.5f));
        lastTrap = (GameObject)trapList[0];
        createNewLineTrap();
        createNewLineTrap();
        createNewLineTrap();
        createNewLineTrap();

        GameObject obj = (GameObject)trapList[0];
        trapList.RemoveAt(0);
        DestroyImmediate(obj);

        starObject.transform.localPosition = new Vector3(0, -3 * TILE_Y / 2 + 0.5f, 1);
        stepFoot = 0;        
        finishObject.transform.localPosition = new Vector3(0, modeClassicMaxStep * TILE_Y - TILE_Y / 2 + 0.5f, 0);
		speedy = 2;
    }
    public void createNewLineTrap()
    {   
        if (lastTrap != null)
        {
            int r = Random.Range(0, 4);
            float y = 0;
            y = lastTrap.transform.localPosition.y;
            y = y + TILE_Y;
            for (int index = 0; index < 4; index++)
            {
                float x = BEGIN_X + index * TILE_X;              
              
                if (r == index)
                {
                    trapList.Add(createNewTrap(index, Trap.TYPE_CAN_TOUCH, x, y));
                }
                else
                {
                    trapList.Add(createNewTrap(index, Trap.TYPE_CANT_TOUCH, x, y));
                }
                if (index == 3)
                    lastTrap = (GameObject)(trapList[trapList.Count-1]);
            }
        }
    }
    public GameObject createNewTrap(int index,int type, float x,float y)
    {

        GameObject a = (GameObject)(GameObject.Instantiate(fistTrap));
        //GameObject a = (GameObject)Instantiate(Resources.Load("TrapPrefab"));
        if (type == Trap.TYPE_CAN_TOUCH)
        {
            a.GetComponent<SpriteRenderer>().sprite = SpriteTrap1;
            a.GetComponent<Trap>().type = Trap.TYPE_CAN_TOUCH;
        }
        else
        {
            a.GetComponent<Trap>().type = Trap.TYPE_CANT_TOUCH;
        }
        a.transform.localPosition = new Vector3(x, y, 1);
        return a;

    }
	GameObject mobject;
	void UpdateGame()
	{
        switch (gameMode)
        {
            case GAME_MODE_CLASSIC://60 buoc

                if (checkTouch() == 1)
                {
                    v3Move.y = -TILE_Y;
                    for (int i = 0; i < trapList.Count; i++)
                    {
                        mobject = (GameObject)trapList[i];
                        
                       // mobject.transform.localPosition = new Vector3(mobject.transform.localPosition.x, mobject.transform.localPosition.y - TILE_Y, 1);
                      
                        mobject.transform.Translate(v3Move);

                        if (currentState == STATE_WATTING)
                        {
                            TimePlayedSubState = 0;
                            currentState = STATE_PLAY;
                        }
                        
                    }
                    finishObject.transform.Translate(v3Move);
                    if (starObject.transform.localPosition.y > -7)
                        starObject.transform.Translate(v3Move);
                }

                if(stepFoot  >= modeClassicMaxStep)
                {
                    currentState = STATE_WATTING_OVER;
                    typeOver = TYPE_OVER_FINISH_60_STEP;
				timePlay  = TimePlayedSubState;
				TimePlayedSubState =0;       
                    Debug.Log("Da tim duoc no roi ne");
                 
                }

                  
            
                break;
            case GAME_MODE_TIME_ATTACK: //20 second
                if (checkTouch() == 1)
                {
                    v3Move.y = -TILE_Y;
                        for (int i = 0; i < trapList.Count; i++)
                        {
                            mobject = (GameObject)trapList[i];
                            //mobject.transform.localPosition = new Vector3(mobject.transform.localPosition.x, mobject.transform.localPosition.y - TILE_Y, 1);                       
                            mobject.transform.Translate(v3Move);
                            if (currentState == STATE_WATTING)
                            {
                                TimePlayedSubState = 0;
                                currentState = STATE_PLAY;
                            }
                        
                        }
                        if (starObject.transform.localPosition.y > -7)
                            starObject.transform.Translate(v3Move);
            
                }
                if (TimePlayedSubState >= modeTimeAttackMaxtime)
                {
                    TimePlayedSubState = modeTimeAttackMaxtime;
                    DisplayTimer();
                    currentState = STATE_WATTING_OVER;
					timePlay  = TimePlayedSubState;
					TimePlayedSubState =0;
                    typeOver = TYPE_OVER_FINISH_TIME_ATTCK;
                }
                break;

            case GAME_MODE_TIME_UNLESS://khogn gioi han thoi gian
                  checkTouch();
                  v3Move.y = -speedy * Time.deltaTime;
                  speedy += Time.deltaTime / 8;
                    for (int i = 0; i < trapList.Count; i++)
                    {
                        mobject = (GameObject)trapList[i];
                        //mobject.transform.localPosition = new Vector3(mobject.transform.localPosition.x, mobject.transform.localPosition.y - speedy * Time.deltaTime, 1);
                       
                        mobject.transform.Translate(v3Move);
                       
                        
                    }
                if(starObject.transform.localPosition.y > -7)
                    starObject.transform.Translate(v3Move);
                break;
        }

        
	}
    
    public int checkTouch()//neu dung = 1; khong dung = 0; khong touch thi = -1
    {      
            bool isCheckTOuch = false;
            if (Input.touchCount > 0 )
            {
                // Debug.Log(Input.touchCount);            
                Touch touch = Input.GetTouch(0);                
                if (touch.phase == TouchPhase.Began)
                {
                    isCheckTOuch = true;
                    touchPostionV3 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                    isCheckTOuch = true;
                    touchPostionV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    
                  
            }
            if (isCheckTOuch)
            {
                isCheckTOuch = false;
                GameObject mobject;
                bool isPassTileCanTouch = false;
                for (int i = 0; i < trapList.Count; i++)
                {
                    mobject = (GameObject)trapList[i];
                    // mobject.transform.localPosition = new Vector3(mobject.transform.localPosition.x, mobject.transform.localPosition.y - speedy, 1);
                    if (mobject.GetComponent<Trap>().type == Trap.TYPE_CAN_TOUCH)
                    {
                        if (PointInRect(touchPostionV3.x, touchPostionV3.y, mobject.transform.localPosition.x - TILE_X / 2, mobject.transform.localPosition.y - TILE_Y / 2, TILE_X, TILE_Y))
                        {   
							SoundEngine.playSound("SoundStep");
                            if(isPassTileCanTouch)
                            {

                            currentState = STATE_WATTING_OVER;
							timePlay  = TimePlayedSubState;
							TimePlayedSubState =0;
								if(gameMode == GAME_MODE_TIME_UNLESS)
									typeOver = TYPE_OVER_FINISH_ARCADE;
								else
									typeOver = TYPE_OVER_FAILT;
                                
                                mobject.GetComponent<SpriteRenderer>().sprite = ((stepFoot++ % 2) == 0 ? SpriteTrapOKRight : SpriteTrapOKLeft);
                                mobject.GetComponent<Trap>().type = Trap.TYPE_CANT_TOUCH;
                                Debug.Log("Da tim duoc no roi ne");
                                return 0;
                            }

                            else
                            {
                                mobject.GetComponent<SpriteRenderer>().sprite = ((stepFoot++%2)==0?SpriteTrapOKRight:SpriteTrapOKLeft);
                                mobject.GetComponent<Trap>().type = Trap.TYPE_CANT_TOUCH;
                                return 1;
                            }
                        }
                        else//here
                        {
                            if (!isPassTileCanTouch)
                                isPassTileCanTouch = true;
                            
                        }
                    }
                    else
                    {
                        if (PointInRect(touchPostionV3.x, touchPostionV3.y, mobject.transform.localPosition.x - TILE_X / 2, mobject.transform.localPosition.y - TILE_Y / 2, TILE_X, TILE_Y))
                        {
                            mobject.GetComponent<SpriteRenderer>().sprite = ((stepFoot++%2)==0?SpriteTrapFailRight:SpriteTrapFailLeft);
						SoundEngine.playSound("SoundFail");
						currentState = STATE_WATTING_OVER;
						timePlay  = TimePlayedSubState;
						TimePlayedSubState =0;
							if(gameMode == GAME_MODE_TIME_UNLESS)
								typeOver = TYPE_OVER_FINISH_ARCADE;
							else
								typeOver = TYPE_OVER_FAILT;
                         //   Debug.Log("Da tim duoc no roi ne");
                            return 0;
                        }
                    }


                }
            }
        
        return -1;
    }
    public bool PointInRect(float x,float y, float x1,float y1,float w1,float h1)
    {
        if ((x >= x1) && (x <= x1 + w1) && (y >= y1) && (y <= y1 + h1))
            return true;
        return false;
    }
	public void restart() 
	{

        TimePlayedSubState = 0;		
		GameObject obj;
		for (int i= trapList.Count-1; i>-1; i--) {
			obj = (GameObject)trapList [i];
			trapList.RemoveAt (i);
			DestroyImmediate (obj);
		}
        InitAllTrap();
        PanelOverGame.SetActive(false);
        currentState = STATE_WATTING;
        DisplayTimer();
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
		ScoreControl.saveGame ();
        UILabel overTile = GameObject.Find("LabelOverTitle").GetComponent<UILabel>();
        switch (typeOver)
        {
            case TYPE_OVER_FINISH_60_STEP:
               // GameObject.Find("LabelScore").GetComponent<UILabel>().text = "" + ScoreControl.getRealScore();
			ScoreControl.setScore(timePlay, 0, 0);
                overTile.text = "COMPLETED";
                GameObject.Find("LabelTimerText").GetComponent<UILabel>().text = "Timer";
                GameObject.Find("LabelScoreText").GetComponent<UILabel>().text = "Step";
			GameObject.Find("LabelTimer").GetComponent<UILabel>().text = string.Format("{0:000.00}", timePlay).Replace(".", ":");
                GameObject.Find("LabelScore").GetComponent<UILabel>().text = stepFoot.ToString();
                break;
            case TYPE_OVER_FINISH_TIME_ATTCK:
                ScoreControl.setScore(0, stepFoot, 0);
                overTile.text = "COMPLETED";
                GameObject.Find("LabelTimerText").GetComponent<UILabel>().text = "Timer";
                GameObject.Find("LabelScoreText").GetComponent<UILabel>().text = "Step";
                GameObject.Find("LabelTimer").GetComponent<UILabel>().text = string.Format("{0:000.00}", timePlay).Replace(".", ":");
                GameObject.Find("LabelScore").GetComponent<UILabel>().text = stepFoot.ToString();
                break;
            case TYPE_OVER_FINISH_ARCADE:
			ScoreControl.setScore(0, 0, timePlay);
                overTile.text = "COMPLETED";

                GameObject.Find("LabelTimerText").GetComponent<UILabel>().text = "Timer";
                GameObject.Find("LabelScoreText").GetComponent<UILabel>().text = "Step";
			GameObject.Find("LabelTimer").GetComponent<UILabel>().text = string.Format("{0:000.00}", timePlay).Replace(".", ":");
                 GameObject.Find("LabelScore").GetComponent<UILabel>().text = stepFoot.ToString();
                break;
            case TYPE_OVER_FAILT:
                overTile.text = "Game Over";
                GameObject.Find("LabelTimerText").GetComponent<UILabel>().text = " ";
                GameObject.Find("LabelScoreText").GetComponent<UILabel>().text = " ";
                 GameObject.Find("LabelTimer").GetComponent<UILabel>().text = " ";
                GameObject.Find("LabelScore").GetComponent<UILabel>().text = " ";
                break;
        }
        ScoreControl.saveGame();
	}
}
