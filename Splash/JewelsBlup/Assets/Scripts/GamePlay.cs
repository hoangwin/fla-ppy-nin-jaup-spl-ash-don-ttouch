using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour {

	
	public GameObject PanelOverGame;
    public GameObject PanelPause;
    public GameObject objIGMButton;

    public Sprite Star1;
    public Sprite Star2;
    public Sprite Star3;
	public float timePlay;

    public UILabel labelScore;
    public UILabel labelTarget;
    public UILabel labelLevel;

	
    public static int gameMode = 0;
    public const int GAME_MODE_CLASSIC = 0; //tap 60 tinh thoi gian
    public const int GAME_MODE_UNLESS = 1;//Trong 10s thang nao di nhieu hon
	public const int MAX_COL = 7;
	public const int MAX_ROW = 7;
	public static GameObject [,] tableFruit = new GameObject[MAX_ROW,MAX_COL];
    public static GameObject[,] tableFruitEffect = new GameObject[MAX_ROW, MAX_COL];
    public GameObject fistObjectSelect;// de lam cac hieu ung neu du dieu kien thi thang dau tien
	public static  ArrayList arrayListSelect;
	public static GameObject lastObjectEffect;


    public UISlider timerbar;
    public static  float MAX_TIMER_UNLESS = 60;
    public int currentSelectIndex = 0;
    
    public static GamePlay instance;	
    public static bool isIGM = false;

    public static int currentState = 0;
    public const int STATE_WATTING_START = 0;
    public const int STATE_WATTING = 1;
	public const int STATE_PLAY = 2;
	public const int STATE_DROP = 3;
	public const int STATE_WATTING_OVER = 4;
    public const int STATE_OVER = 5;


    public static int currentSubUnGameState = 0;
    public const int STATE_SUB_WATTING_CLICK = 0;
    public const int STATE_SUB_CLICK = 1;
    public const int STATE_SUB_RELEASE = 2;
    public const int STATE_SUB_DRAP = 3;
    
	public static int countUpdate = 0;
    public static float TILE_X = 0;
    public static float TILE_Y = 0;
    public static float BEGIN_X = 0;
	public static float BEGIN_Y = 0;

   
    Vector3 touchPostionV3;
    int touchState;
    public const int TOUCH_STATE_IDE = 0;
    public const int TOUCH_STATE_CLICK = 1;
    public const int TOUCH_STATE_DRAP = 2;
    public const int TOUCH_STATE_RELEASE = 3;

    public static bool isX2Effect;
    public static bool isBoomEffect;
    public static bool isThunderEffect;


    public static float timmerCountDown = 0;//
    public static float timmerCountState = 0;//
    public static int completedScore = 0;
    public static int tempScore = 0;//
    public static int tempStep = 0;//
	void Start () {
		DEF.Init();
		GameObject hand = GameObject.Find("BackGround");
        if (gameMode == GAME_MODE_CLASSIC)
        {
            GameObject.Find("bgTile_Best").SetActive(false);
            GameObject.Find("TimerText").SetActive(false);
        }
        else if (gameMode == GAME_MODE_UNLESS)
        {
            GameObject.Find("bgTile_Target").SetActive(false);
            GameObject.Find("TimerCompletedText").SetActive(false);
        }

        if (hand != null)
        {          
            hand.transform.localScale = new Vector3(DEF.sx_ortho, DEF.sy_ortho, 1);
        }
        DEF.ScaleAnchorGui();
        labelScore = GameObject.Find("LabelScoreScore").GetComponent<UILabel>();
        labelLevel = GameObject.Find("LabelScoreLevel").GetComponent<UILabel>();
        labelTarget = GameObject.Find("LabelScoreTarget").GetComponent<UILabel>();
        // createNewTrap();
        TILE_Y = 0.88f;// GameObject.Find("bgTile_Board").GetComponent<SpriteRenderer>().bounds.size.y / 7;
        float Board_x = GameObject.Find("bgTile_Board").GetComponent<SpriteRenderer>().bounds.size.x ;
        Board_x = Board_x - Board_x / 19;
        TILE_X = Board_x/7;            
       
        BEGIN_X = -TILE_X * 7/2 +TILE_X/2;// -GameObject.Find("bgTile_Board").GetComponent<SpriteRenderer>().bounds.size.x / 2 + TILE_X / 2 + 2 * offsetx;        
        BEGIN_Y = 2.24f;// TILE_Y * 7 / 2 - TILE_Y / 2 - 0.40f;
        initgame();
        SoundEngine.initGamePlaySound();
        
	}

    public void initgame()
    {
        InitAllFruit();
        currentState = STATE_WATTING_START;
		currentSubUnGameState = STATE_SUB_WATTING_CLICK;
        isIGM = false;
        instance = this;
        NGUITools.SetActive(PanelOverGame, false);
        NGUITools.SetActive(PanelPause, false);
       
        if (arrayListSelect == null)
            arrayListSelect = new ArrayList();
        else
            arrayListSelect.Clear();
        if (gameMode == GAME_MODE_CLASSIC)
        {
           
            completedScore = SelectLevel.mcurrentLevel * 1750 + 15750;
            labelTarget.text = completedScore.ToString();
            labelScore.text = "0";
            labelLevel.text = SelectLevel.mcurrentLevel.ToString();
        }
        else
        {
           
            completedScore = 0;
            timmerCountDown = MAX_TIMER_UNLESS;
            labelTarget.text = ScoreControl.bestScore.ToString();
            labelScore.text = "0";
            labelLevel.text = "Arcade";
        }
        tempScore = 0;
        tempStep = 0;
        if (gameMode == GAME_MODE_CLASSIC)
            timerbar.value = 0.0f;
        else if (gameMode == GAME_MODE_UNLESS)
            timerbar.value = 1.0f;
        countUpdate = 0;
        timmerCountState = 0;
        SoundEngine.playSound("game_start");
    }

    void destroyTrap()
    {
    }

    void Update() 
	{
		countUpdate++;

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if(currentState == STATE_OVER ||currentState == STATE_WATTING_OVER)
				Application.LoadLevel("MainMenu");
			else
            	setIGM();
		}
        if (isIGM)
            return;
        
		switch (currentState) {
            case  STATE_WATTING_START:
                timmerCountState += Time.deltaTime;                
                if (timmerCountState > 1.3f)
                {
                    currentState = STATE_WATTING;
                    SoundEngine.playSound("go");
                }
                break;
            case STATE_WATTING://drop
                currentState = STATE_PLAY;
           	 break;
            case STATE_PLAY:	           
	            UpdateGame();
                updateProcessingBar();
				checkGameOver();
	            break;
		case GamePlay.STATE_DROP:
			//if(TimePlayedSubState >0.5f)
			//	showGameOver();
                updateProcessingBar();

			if(countUpdate%3 ==0)
			{
               // while (arrayListSelect.Count > 0)
				if(arrayListSelect.Count >0)
				{
					GameObject obj = (GameObject)(arrayListSelect[0]);
                   // int i = obj.GetComponent<Fruit>().row;
                   // int j = obj.GetComponent<Fruit>().col;
                    //boom effect
                    if(isBoomEffect)
                        tableFruitEffect[obj.GetComponent<Fruit>().row, obj.GetComponent<Fruit>().col].GetComponent<FruitAnim>().setplayAnimDestroy2();
                    else
                        tableFruitEffect[obj.GetComponent<Fruit>().row, obj.GetComponent<Fruit>().col].GetComponent<FruitAnim>().setplayAnimDestroy();
                    //score effect
                    if(isX2Effect)
                        tableFruitEffect[obj.GetComponent<Fruit>().row, obj.GetComponent<Fruit>().col].GetComponent<FruitAnim>().setplayEffectScoreX2();
                    else
                        tableFruitEffect[obj.GetComponent<Fruit>().row, obj.GetComponent<Fruit>().col].GetComponent<FruitAnim>().setplayEffectScore();

                    obj.GetComponent<Fruit>().setAnimAnimEffectNone();
                    if (tableFruit[obj.GetComponent<Fruit>().row, obj.GetComponent<Fruit>().col].GetComponent<Fruit>().typeEffect == 2)
                    {
                        tableFruit[obj.GetComponent<Fruit>().row, obj.GetComponent<Fruit>().col].GetComponent<Fruit>().typeEffect = 0;//here
                        tableFruitEffect[obj.GetComponent<Fruit>().row, obj.GetComponent<Fruit>().col].GetComponent<FruitAnim>().setAnimThunder();
                    }
                    if (arrayListSelect.Count == 1)
                    {
                      // 
                        lastObjectEffect = obj;
                    }
                    SoundEngine.playSoundFruitExplotion(arrayListSelect.Count%11);
                    arrayListSelect.RemoveAt(0);
                    
                    
				}

			}	
			if(lastObjectEffect != null)
			{
				if(lastObjectEffect.GetComponent<Fruit>().state == Fruit.FRUIT_STATE_RENEW)
				{                    
					currentState = STATE_PLAY;	//here thus hien o
                    currentSubUnGameState = STATE_SUB_WATTING_CLICK;
                  //  Debug.Log("aaaaaaaaaa");
                    lastObjectEffect = null;                   
                    sortTable();
                    MoveToRightPosition();                    
                    tempStep++;
				}

			}
            
			break;
            case STATE_WATTING_OVER:
            SoundEngine.playSound("Sound_Game_Win");
                PanelOverGame.SetActive(true);
                objIGMButton.SetActive(false);
                iTween.MoveTo(PanelOverGame, iTween.Hash("y", 0));
				if (gameMode == GAME_MODE_CLASSIC)
				{
					GameObject.Find("LabelOverLevel").GetComponent<UILabel>().text = SelectLevel.mcurrentLevel.ToString();
					GameObject.Find("LabelOverScore").GetComponent<UILabel>().text = tempScore.ToString();
					GameObject.Find("LabelOverLevelText").GetComponent<UILabel>().text = "Level";
					GameObject.Find("LabelOverScoreText").GetComponent<UILabel>().text = "Score";
                    int star = 0;//1 sao
                    Debug.Log("AAA : " + tempScore / tempStep);
                    if(tempScore/tempStep >= 800)
                    {
                        star = 2;
                        GameObject.Find("UI_5Star").GetComponent<SpriteRenderer>().sprite = Star3;

                    }
                    else if (tempScore / tempStep > 500)
                    {
                        star = 1;
                        GameObject.Find("UI_5Star").GetComponent<SpriteRenderer>().sprite = Star2;
                    }
                    else
                    {
                        GameObject.Find("UI_5Star").GetComponent<SpriteRenderer>().sprite = Star1;
                    }
                    SelectLevel.starArray[SelectLevel.mcurrentLevel] = star;
                    ScoreControl.setLevelUnblock(SelectLevel.mcurrentLevel);				
				}
				else if (gameMode == GAME_MODE_UNLESS)
				{
                    ScoreControl.setBestScore(tempScore);
					GameObject.Find("LabelOverLevel").GetComponent<UILabel>().text = tempScore.ToString();
					GameObject.Find("LabelOverScore").GetComponent<UILabel>().text = ScoreControl.bestScore.ToString();
					GameObject.Find("LabelOverLevelText").GetComponent<UILabel>().text = "Score";
					GameObject.Find("LabelOverScoreText").GetComponent<UILabel>().text = "Best";
                    
				}               
                
                ScoreControl.saveGame();
				SelectLevel.mcurrentLevel++;
				currentState = STATE_OVER;
			break;
		}
	}
	
	void MoveToRightPosition()
	{
        int count = 0;
         for(int col=0;col<MAX_COL;col++)
        {
            count = 0;
           
            for (int row = MAX_ROW -1; row >=0 ; row--)
            {
                if (tableFruit[row, col].GetComponent<Fruit>().state == Fruit.FRUIT_STATE_IDE)
                {
                    tableFruit[row, col].GetComponent<Fruit>().setMoveToRightPostiontion(tableFruit[row, col], row, col);
                }
                else
                {
                    tableFruit[row, col].GetComponent<Fruit>().state = Fruit.FRUIT_STATE_IDE;
                    tableFruit[row, col].transform.localPosition = new Vector3(tableFruit[row, col].transform.localPosition.x, 4 + count*TILE_Y, tableFruit[row, col].transform.localPosition.z);
                    tableFruit[row, col].GetComponent<Fruit>().setIndex(Random.Range(1, 6));
                    tableFruit[row, col].GetComponent<Fruit>().setMoveToRightPostiontion(tableFruit[row, col], row, col);
                    count++;
                }
            }
         }
    }

    void sortTable()
    {
        GameObject tempObj;
        for(int col=0;col<MAX_COL;col++)
        {
        //    int col = 0;
           
            for (int row = MAX_ROW-1; row >=0 ; row--)
            {

                for (int rowRun = row - 1; rowRun >= 0; rowRun--)
                {
                    if (tableFruit[row, col].GetComponent<Fruit>().state > tableFruit[rowRun, col].GetComponent<Fruit>().state)
                    {
                        //doi cho
                        tempObj = tableFruit[row, col];
                        tableFruit[row, col] = tableFruit[rowRun, col];
                        tableFruit[row, col].GetComponent<Fruit>().row = row;
                        tableFruit[row, col].GetComponent<Fruit>().col = col;
                        tableFruit[rowRun, col] = tempObj;
                        tableFruit[rowRun, col].GetComponent<Fruit>().row = rowRun;
                        tableFruit[rowRun, col].GetComponent<Fruit>().col = col;
                    }
                }
            }
        }

        if (fistObjectSelect.GetComponent<Fruit>().typeEffect > 0)
        {
            int row = fistObjectSelect.GetComponent<Fruit>().row;
            int col = fistObjectSelect.GetComponent<Fruit>().col;
            tableFruit[row, col].GetComponent<Fruit>().typeEffect = fistObjectSelect.GetComponent<Fruit>().typeEffect;
            tableFruit[row, col].GetComponent<Fruit>().setAnimEffectIndex();
        }
    } 
    public void InitAllFruit()
    {
        int indexDrop = Random.Range(1, 4);
		for(int row =0;row < MAX_ROW;row ++)
            for (int col = 0; col < MAX_COL; col++)
            {
                //tableFruit[row,col]  = (GameObject)(GameObject.Instantiate(fistTrap));

                int index = Random.Range(1,6);
                if(tableFruit[row, col] == null)
				{
                    tableFruit[row, col] = (GameObject)Instantiate(Resources.Load("FruitPrefab"));
				}
				else
				{
					tableFruit[row, col].GetComponent<Fruit>().setAnimEffectNone();
					tableFruit[row, col].GetComponent<Fruit>().state = Fruit.FRUIT_STATE_IDE;
					tableFruit[row, col].GetComponent<Fruit>().typeEffect = 0;					
				}

                tableFruit[row, col].transform.localPosition = new Vector3(BEGIN_X + col * TILE_X, BEGIN_Y - row * TILE_Y, 0);
                tableFruit[row, col].GetComponent<Fruit>().setIndex(index);
                tableFruit[row, col].GetComponent<Fruit>().row = row;
                tableFruit[row, col].GetComponent<Fruit>().col = col;
				
                if (indexDrop == 1)
                    tableFruit[row, col].GetComponent<Fruit>().setMoveBegin(6 + (MAX_ROW - row) * MAX_ROW * 0.1f + 0.1f * (col) * MAX_COL);
                else
                    tableFruit[row, col].GetComponent<Fruit>().setMoveBegin(6 + (MAX_ROW - row) * MAX_ROW * 0.1f + 0.1f * (MAX_COL - col) * MAX_COL);
                if(tableFruitEffect[row, col]  == null)
                    tableFruitEffect[row, col] = (GameObject)Instantiate(Resources.Load("FruitAnimPrefab"));
                tableFruitEffect[row, col].transform.localPosition = new Vector3(BEGIN_X + col * TILE_X, BEGIN_Y - row * TILE_Y, 0);
                tableFruitEffect[row, col].GetComponent<FruitAnim>().row = row;
                tableFruitEffect[row, col].GetComponent<FruitAnim>().col = col;
            }
    }   
	
	void UpdateGame()
	{
        checkTouch();
        switch (currentSubUnGameState)
        {
	  	  	case STATE_SUB_WATTING_CLICK:  
				checkAndAddFruitInSelectedLish();
			break;
			case  STATE_SUB_DRAP:
					checkAndAddFruitInSelectedLish();
			if (touchState == TOUCH_STATE_RELEASE)
			{
				if(arrayListSelect.Count >=3)
				{
                    checkSetIndexEffectNextTime();

					currentState = STATE_DROP;//kiem tra cai da ne
                    checkAndSetEffectAnimCurrentTIme();
                    if (isThunderEffect)
                        SoundEngine.playSound("effect_lightning");
                    if(isBoomEffect)
                        SoundEngine.playSound("fire");
                    if(isX2Effect)
                        SoundEngine.playSound("soundx2");
				}
				else
				{
					for(int i= arrayListSelect.Count - 1;i>=0;i--)
					{
						Fruit  fruit = ((GameObject)(arrayListSelect[i])).GetComponent<Fruit>();
						
						fruit.setResetANim();
						fruit.state = Fruit.FRUIT_STATE_IDE;
						tableFruitEffect[fruit.row, fruit.col].GetComponent<FruitAnim>().setAnimObjectToNONE();
						arrayListSelect.RemoveAt(i);
					}
                    SoundEngine.playSound("error");
				}
			}
			break;
        }
	}
    private void checkAndSetEffectAnimCurrentTIme()
    {
        isThunderEffect = false;
        isX2Effect = false;
        isBoomEffect = false;
        for (int i = 0; i < arrayListSelect.Count; i++)
        {
            if (((GameObject)arrayListSelect[i]).GetComponent<Fruit>().typeEffect == Fruit.TYPE_EFFECT_THUNDER)
            {
                isThunderEffect = true;
                for (int col = 0; col < MAX_COL; col++)
                {
                    if (tableFruit[((GameObject)arrayListSelect[i]).GetComponent<Fruit>().row, col].GetComponent<Fruit>().state == Fruit.FRUIT_STATE_IDE)
                    {
                        tableFruit[((GameObject)arrayListSelect[i]).GetComponent<Fruit>().row, col].GetComponent<Fruit>().state = Fruit.FRUIT_STATE_SELECT;
                        arrayListSelect.Add(tableFruit[((GameObject)arrayListSelect[i]).GetComponent<Fruit>().row, col]);
                        
                    }
                }
            }
            else if (((GameObject)arrayListSelect[i]).GetComponent<Fruit>().typeEffect == Fruit.TYPE_EFFECT_X2_SCORE)
            {
                isX2Effect = true;
            }
            else if (((GameObject)arrayListSelect[i]).GetComponent<Fruit>().typeEffect == Fruit.TYPE_EFFECT_BOMM_ALL)
            {

                isBoomEffect = true;
                ((GameObject)arrayListSelect[i]).GetComponent<Fruit>().typeEffect = 0;
                for (int row = 0; row < MAX_COL; row++)
                    for (int col = 0; col < MAX_COL; col++)
                {
                    if (tableFruit[row, col].GetComponent<Fruit>().index == currentSelectIndex && tableFruit[row, col].GetComponent<Fruit>().state == Fruit.FRUIT_STATE_IDE)
                    {
                        tableFruit[row, col].GetComponent<Fruit>().state = Fruit.FRUIT_STATE_SELECT;
                        arrayListSelect.Add(tableFruit[row, col]);
                    }
                }
            }
        }
    }
    private void checkSetIndexEffectNextTime()
    {
        if (arrayListSelect.Count == 6)//x2 score
        {   
            fistObjectSelect.GetComponent<Fruit>().typeEffect = Fruit.TYPE_EFFECT_X2_SCORE;
            fistObjectSelect.GetComponent<Fruit>().row = ((GameObject)arrayListSelect[0]).GetComponent<Fruit>().row;
            fistObjectSelect.GetComponent<Fruit>().col = ((GameObject)arrayListSelect[0]).GetComponent<Fruit>().col;

        }
        else if (arrayListSelect.Count == 7)//thunder
        {
            fistObjectSelect.GetComponent<Fruit>().typeEffect = Fruit.TYPE_EFFECT_THUNDER;
			fistObjectSelect.GetComponent<Fruit>().row = ((GameObject)arrayListSelect[0]).GetComponent<Fruit>().row;
            fistObjectSelect.GetComponent<Fruit>().col = ((GameObject)arrayListSelect[0]).GetComponent<Fruit>().col;
        }
        else if (arrayListSelect.Count >= 8)//boom
        {
            isBoomEffect = true;
            fistObjectSelect.GetComponent<Fruit>().typeEffect = Fruit.TYPE_EFFECT_BOMM_ALL;
			fistObjectSelect.GetComponent<Fruit>().row = ((GameObject)arrayListSelect[0]).GetComponent<Fruit>().row;
            fistObjectSelect.GetComponent<Fruit>().col = ((GameObject)arrayListSelect[0]).GetComponent<Fruit>().col;
        }else
            fistObjectSelect.GetComponent<Fruit>().typeEffect = 0;
    }
    
    public void checkTouch()//neu dung = 1; khong dung = 0; khong touch thi = -1
    {      
        if(touchState == TOUCH_STATE_RELEASE)
            touchState = TOUCH_STATE_IDE;
            
            if (Input.touchCount > 0 )
            {
                // Debug.Log(Input.touchCount);            
                Touch touch = Input.GetTouch(0);                
                if (touch.phase == TouchPhase.Began)
                {
                    touchState = TOUCH_STATE_CLICK;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    touchState = TOUCH_STATE_DRAP;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    touchState = TOUCH_STATE_RELEASE;
                }
                     
                touchPostionV3 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                touchState = TOUCH_STATE_CLICK;
                    touchPostionV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                  touchState = TOUCH_STATE_DRAP;
                  touchPostionV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }if (Input.GetMouseButtonUp(0))
            {
                touchState = TOUCH_STATE_RELEASE;
                touchPostionV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
//            Debug.Log((touchState));
    }
	public void checkAndAddFruitInSelectedLish()
	{
		if (touchState == TOUCH_STATE_CLICK || touchState == TOUCH_STATE_DRAP)
		{
			GameObject obj = checkTouchAtBegin();
			if ( obj!= null)
			{
				Fruit fruit = obj.GetComponent<Fruit>();
                if (fruit.state == Fruit.FRUIT_STATE_IDE)
				{
                    if (arrayListSelect.Count == 0)//phan tu dau tien
                        currentSelectIndex = fruit.index;
                    if (fruit.index == currentSelectIndex)
                    {
                        bool isAdd = true;
                        if (arrayListSelect.Count > 0)
                        {
                            Fruit lastObj = ((GameObject)arrayListSelect[arrayListSelect.Count -1]).GetComponent<Fruit>();
                            //kiem tra 2 thang on o day
                            if (fruit.row < lastObj.row - 1 ||
                                fruit.row > lastObj.row + 1 ||
                                fruit.row - 1 > lastObj.row ||
                                fruit.row + 1 < lastObj.row ||
                                fruit.col < lastObj.col - 1 ||
                                fruit.col > lastObj.col + 1 ||
                                fruit.col - 1 > lastObj.col ||
                                fruit.col + 1 < lastObj.col
                                )
                            {
                                isAdd = false;
                            }//here
							else
							{
								if(!PointInRect(touchPostionV3.x,touchPostionV3.y,obj.transform.localPosition.x - TILE_X/2 + TILE_X/6 ,obj.transform.localPosition.y - TILE_Y/2 + TILE_Y/6, TILE_X - TILE_X/3,TILE_Y - TILE_Y/3))
								{
									isAdd = false;
								}
							}
                        }
                        if (isAdd)
                        {

                            fruit.state = Fruit.FRUIT_STATE_SELECT;
                            fruit.setIndexIsSelect(fruit.index);
                            //	Debug.Log("Da CO Mot Chu");
                            arrayListSelect.Add(obj);
                            SoundEngine.playSoundSelectFruit(arrayListSelect.Count);
                            if (currentSubUnGameState == STATE_SUB_WATTING_CLICK)
                            {
                                currentSubUnGameState = STATE_SUB_DRAP;
                            }
                            else
                            {

                            }
                            int indexLink = getLink2Objectindex();
                           // Debug.Log("Index Link :" + indexLink);
                            if (indexLink > 0 )
                            {
                                tableFruitEffect[fruit.row, fruit.col].GetComponent<FruitAnim>().setIndexLink2Object(indexLink);
                            }
                        }
                    }                    
				}
				else if(fruit.state == Fruit.FRUIT_STATE_SELECT)//
				{
					if(arrayListSelect.Count >1)//xet de loai ra khi no tro ve vi tri truoc do
					{
						Fruit lastObj = ((GameObject)arrayListSelect[arrayListSelect.Count -2]).GetComponent<Fruit>();
						if(fruit.col == lastObj.col && fruit.row == lastObj.row)
						{
							lastObj = ((GameObject)arrayListSelect[arrayListSelect.Count -1]).GetComponent<Fruit>();
							lastObj.setResetANim();
							lastObj.state = Fruit.FRUIT_STATE_IDE;


							tableFruitEffect[lastObj.row,lastObj.col].GetComponent<FruitAnim>().setAnimObjectToNONE();

							arrayListSelect.RemoveAt(arrayListSelect.Count-1);
                            SoundEngine.playSoundSelectFruit(arrayListSelect.Count);

						}
					}
				}
				
			}
		}
	}

    public GameObject checkTouchAtBegin()
    {
        float t = (BEGIN_Y + TILE_Y / 2 - touchPostionV3.y);
        if (t < 0)
            return null;
        int row = (int)((BEGIN_Y + TILE_Y/2-touchPostionV3.y) / TILE_Y);
		int col =(int)((touchPostionV3.x - BEGIN_X +  TILE_X/2) / TILE_X);
		if(col < 0 || row <0 ||col >= MAX_COL ||row >=MAX_ROW)
			return null;
        if (tableFruit[row, col] != null)
            return tableFruit[row, col];
        return null;
    }
    public bool PointInRect(float x,float y, float x1,float y1,float w1,float h1)
    {
        if ((x >= x1) && (x <= x1 + w1) && (y >= y1) && (y <= y1 + h1))
            return true;
        return false;
    }

    public int getLink2Objectindex()
    {
        //chi set arrayListSelect // lay chi so de set cho last object
        int count = arrayListSelect.Count;
        
        if (count > 1)
        {
            Fruit fruitlast = ((GameObject)arrayListSelect[count - 1]).GetComponent<Fruit>();
            Fruit fruitpre = ((GameObject)arrayListSelect[count - 2]).GetComponent<Fruit>();
            
			if (fruitlast.row == fruitpre.row - 1 && fruitlast.col == fruitpre.col - 1)
                return 5;
            else if (fruitlast.row == fruitpre.row + 1 && fruitlast.col == fruitpre.col + 1)
                return 1;
            else if (fruitlast.row == fruitpre.row && fruitlast.col == fruitpre.col - 1)
                return 4;
            else if(fruitlast.row == fruitpre.row && fruitlast.col == fruitpre.col + 1)
                return 8;
            else if (fruitlast.row == fruitpre.row - 1 && fruitlast.col == fruitpre.col)
                return 6;
            else if (fruitlast.row == fruitpre.row + 1 && fruitlast.col == fruitpre.col)
                return 2;
            else if (fruitlast.row == fruitpre.row - 1 && fruitlast.col == fruitpre.col + 1)
                return 7;
            else if (fruitlast.row == fruitpre.row + 1 && fruitlast.col == fruitpre.col - 1)
                return 3;
        }

        
        return 0;//0 la khong co duong link
    }
    public void setIGM()
    {
        isIGM = !isIGM;

        if (isIGM)
        {
            PanelPause.SetActive(true);
            objIGMButton.SetActive(false);
            iTween.MoveTo(PanelPause, iTween.Hash("y", 0));
        }
        else
        {
            iTween.MoveTo(PanelPause, iTween.Hash("y", 12, "oncomplete", "IGMMoveComplete"));
        }
    }
    public void checkGameOver()
    {
        if(gameMode== GAME_MODE_CLASSIC)
        {
            if (tempScore >= completedScore)
            {
                currentState = STATE_WATTING_OVER;
            }
        }
        else if (gameMode == GAME_MODE_UNLESS)
        {
            if (timmerCountDown <= 0)
            {
                timmerCountDown = 0;
                currentState = STATE_WATTING_OVER;
            }
        }

    }
    public void updateProcessingBar()
    {
        if (gameMode == GAME_MODE_CLASSIC)
        {
            if (completedScore < tempScore)
                timerbar.value = 1;
            else
                timerbar.value = tempScore * 1.0f / completedScore;
        }
        else if (gameMode == GAME_MODE_UNLESS)
        {
            if (currentState == STATE_PLAY|| currentState == STATE_DROP)
                timmerCountDown -= Time.deltaTime;
            if (timmerCountDown > MAX_TIMER_UNLESS)
            {
                timmerCountDown = MAX_TIMER_UNLESS;
                timerbar.value = 1;
            }
            else
            {
                float t = timmerCountDown / MAX_TIMER_UNLESS;
                timerbar.value = t;
                Debug.Log(" " + t + "," + timerbar.value);
            }

        }
    }

	public void playNextLevel()
	{
        for(int row =0;row < MAX_ROW;row ++)
            for (int col = 0; col < MAX_COL; col++)
            {
                tableFruit[row, col].GetComponent<Fruit>().setAnimAnimEffectNone();             
				tableFruitEffect[row, col].GetComponent<FruitAnim>().setAnimObjectToNONE();
            }
		iTween.MoveTo(PanelOverGame, iTween.Hash("y", 12, "oncomplete", "OverMoveComplete"));	
	}

    public void rePlayLevel()
    {
        for (int row = 0; row < MAX_ROW; row++)
            for (int col = 0; col < MAX_COL; col++)
            {
                tableFruit[row, col].GetComponent<Fruit>().setAnimAnimEffectNone();
                tableFruitEffect[row, col].GetComponent<FruitAnim>().setAnimObjectToNONE();
            }
        iTween.MoveTo(PanelPause, iTween.Hash("y", 12, "oncomplete", "PauseMoveComplete"));
    }   
}

