using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float maxSpeed = 10f;
	private Vector2 movement;
	bool isCallJump = false;
	float currentAngle;
	GameObject birdObject;
    public BoxCollider2D boxCollider2d;
	float mygravityScale = 1.5f;
    public static Player instance;
    public Animator anim;
	// Use this for initialization
	void Start () {
		DEF.Init ();
		//this.transform.rigidbody2D.AddForce (new Vector2 (0, 200));
		maxSpeed = 15f;
		Debug.Log ("maxSpeed :" + maxSpeed);
		this.transform.rigidbody2D.fixedAngle = true;
		birdObject = GameObject.Find("Bird");
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {

		switch (GamePlay.currentState) {
		case GamePlay.STATE_WATTING:
			if (Input.GetMouseButtonDown (0) ||  ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began))) {
              //  Jump();
                if (checkTouchBegin())
                {
                    GamePlay.currentState = GamePlay.STATE_PLAY;
                    GamePlay.instance.UI_READY.SetActive(false);
                    GamePlay.instance.BGScore.SetActive(true);
                    GamePlay.instance.BirdObject.GetComponent<Rigidbody2D>().gravityScale = mygravityScale;
                    
                }
           //     GamePlay.instance.PlatformBegin.SetActive(false);

			}
			break;

		case GamePlay.STATE_PLAY:

			break;

		case GamePlay.STATE_DROP:
                transform.position = new Vector3(0,-6,0);
			break;
		}

		float x = this.transform.rigidbody2D.velocity.x;
		float y = this.transform.rigidbody2D.velocity.y;
		//if (y > maxSpeed)
		//   y = maxSpeed;
		//else if (y < -maxSpeed)
		//	y = -maxSpeed;
		currentAngle = y / maxSpeed;
		currentAngle = Mathf.Asin (currentAngle);
		currentAngle =currentAngle*Mathf.Rad2Deg/3;
	//	GameObject.Find("Box1").transform.localRotation  =Quaternion.Euler( new Vector3(0,0,Random.Range(0,360)));
		/*
        if(GamePlay.currentState == GamePlay.STATE_DROP || GamePlay.currentState ==GamePlay.STATE_OVER)
			this.transform.localRotation =  Quaternion.Euler( new Vector3(0,0,-90));
		else
			this.transform.localRotation =  Quaternion.Euler( new Vector3(0,0,currentAngle));
         */
		//this.transform.Rotate ( new Vector3 (0, 0, currentAngle));
		//Debug.Log (currentAngle);
	}
    public bool checkTouchBegin()
    {
        Vector2 fingerPos1 = new Vector2(0,0);
        if (Input.touchCount == 1)
        {
            fingerPos1 = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            fingerPos1 = Input.mousePosition;
        }
        fingerPos1 = Camera.main.ScreenToWorldPoint(fingerPos1);
        Debug.Log(fingerPos1);
        if (fingerPos1.y < 0.0f)
            return true;
        return false;
    }
    public void Jump(float x,float y)
    {
        anim.Play("Bird_JUMP");
        movement = new Vector2(x, y);
        
        // movement = new Vector2(Mathf.Sin(Platform.Angle) * maxSpeed, Mathf.Cos(Platform.Angle) * maxSpeed);
        isCallJump = true;
    }
	void FixedUpdate()
	{
		if (isCallJump)
		{
			rigidbody2D.velocity = movement;
			isCallJump = false;
            PlaySoundJump();
		}
        
       switch (GamePlay.currentState) {
		case GamePlay.STATE_WATTING:
			
			break;
		case GamePlay.STATE_PLAY:
			if(this.transform.rigidbody2D.velocity.x != 0)
            {
                if (this.transform.rigidbody2D.velocity.x > 0)
                {
                    this.transform.rigidbody2D.velocity = new Vector2(this.transform.rigidbody2D.velocity.x - this.transform.rigidbody2D.velocity.x*Time.deltaTime, this.transform.rigidbody2D.velocity.y);
                }else
                {
                    this.transform.rigidbody2D.velocity = new Vector2(this.transform.rigidbody2D.velocity.x + this.transform.rigidbody2D.velocity.x * Time.deltaTime, this.transform.rigidbody2D.velocity.y);
                }
            }
            if (transform.position.y < CameraFollow.instance._transform.position.y - 5)
            {
                GamePlay.currentState = GamePlay.STATE_DROP;
                GamePlay.instance.initGameOver();
                GamePlay.TimePlayedSubState = 0f;
                //GamePlay.instance.showGameOver ();
                // Debug.Log("Tieu tui rui");
                
                this.transform.rigidbody2D.velocity = new Vector2(0, 0);
                boxCollider2d.enabled = false;
                Platform.instance.transform.position = new Vector3(0, CameraFollow.instance._transform.position.y + 5, 0);
                GamePlay.instance.PanelOverGame.transform.position = new Vector3(0, CameraFollow.instance._transform.position.y + 5, 0);
                iTween.MoveTo(GamePlay.instance.PanelOverGame, iTween.Hash("y", CameraFollow.instance._transform.position.y-1));
                SoundEngine.instance.PlayOneShot(SoundEngine.instance._soundLose);
            }
			break;
		case GamePlay.STATE_DROP:

			break;
		}
        
        CheckCreateNewTrap();
	}
    int indexTrap = 0;
    float offsetBegin =5f;
    float offsetNew = 9f;
    
    void CheckCreateNewTrap()
    {
        GameObject lastTrap = null;
        if (indexTrap < 2)
        {
            if (transform.position.y > indexTrap * offsetNew + offsetBegin)
            {
                int temp = Random.Range(0, 2);
                indexTrap++;              
                if(temp == 0)
                    lastTrap = (GameObject)Instantiate(Resources.Load("Trap1"));
                else
                     lastTrap = (GameObject)Instantiate(Resources.Load("Trap2"));
                lastTrap.transform.position = new Vector3(lastTrap.transform.position.x, indexTrap * offsetNew, lastTrap.transform.position.z);
            }
        }
        else
        {
            if (transform.position.y > indexTrap * offsetNew + offsetBegin)
            {
                int temp = Random.Range(0, 10);
                indexTrap++;
               
                if (temp == 0)
                {
                    lastTrap = (GameObject)Instantiate(Resources.Load("Trap1"));
                    lastTrap.transform.position = new Vector3(lastTrap.transform.position.x, indexTrap * offsetNew + offsetBegin, lastTrap.transform.position.z);
                }
                else if (temp == 1)
                {
                    lastTrap = (GameObject)Instantiate(Resources.Load("Trap2"));
                    lastTrap.transform.position = new Vector3(lastTrap.transform.position.x, indexTrap * offsetNew + offsetBegin, lastTrap.transform.position.z);
                }else if (temp <=7)
                {
                    int count = (indexTrap -2) / 5 + 1;
                    if (count > 4)
                        count = 4;
                    for (int i = 0; i < count; i++)
                    {
                        lastTrap = (GameObject)Instantiate(Resources.Load("SpiderFly"));
                        float Randomx = Random.Range(-3, 3);
                        float RandomofsetY = Random.Range(-2 , 2);
                        if (RandomofsetY > -0.5f && RandomofsetY < 0.5f)
                            RandomofsetY = 0.5f;
                        lastTrap.transform.position = new Vector3(Randomx, indexTrap * offsetNew + RandomofsetY + offsetBegin, lastTrap.transform.position.z);
                    }
                    
                }
                else if (temp <=9)
                {
                    lastTrap = (GameObject)Instantiate(Resources.Load("Trap3"));
                    lastTrap.transform.position = new Vector3(lastTrap.transform.position.x, indexTrap * offsetNew + offsetBegin, lastTrap.transform.position.z);                
                }
                
            }
        }
        if (lastTrap != null)
            GamePlay.instance.trapList.Add(lastTrap);
    }
	void OnCollisionEnter2D(Collision2D collision)
	{

        if (collision.gameObject.name.Equals("Platform"))
        {
            float Angle = Mathf.Atan2(this.transform.rigidbody2D.velocity.y, this.transform.rigidbody2D.velocity.x);
            float x = this.transform.rigidbody2D.velocity.x;
            float y = this.transform.rigidbody2D.velocity.y;
            if (y < 0)
                y = -y;
            float max = Mathf.Sqrt(x * x + y * y);
           // max = (maxSpeed / max);
            float percent = 90f / Platform.instance.ui2DSprite.width;
            percent = 4 * percent;
            if (percent > 1)
                percent = 1;
            else if (percent < 0.4f)
                percent = 0.4f;
           // Debug.Log(percent);
            max = (maxSpeed / max )*percent;
            
            //_Platform.transform.eulerAngles = new Vector3(0, 0, Angle * 57.3f);
            Jump(x * max, y * max);
        }
        else if (collision.gameObject.name.Equals("PlatformBegin"))
        {
            Jump(this.transform.rigidbody2D.velocity.x, 10);
        }
            
        else if (collision.gameObject.name.Equals("BG"))
        {
          
         
            float x = this.transform.rigidbody2D.velocity.x;
          if(x>3)
              this.transform.rigidbody2D.velocity = new Vector2(3, this.transform.rigidbody2D.velocity.y);
          else
              if (x <- 3)
                  this.transform.rigidbody2D.velocity = new Vector2(-3, this.transform.rigidbody2D.velocity.y);
            //_Platform.transform.eulerAngles = new Vector3(0, 0, Angle * 57.3f);
            //Jump(x * max, y * max);
        }
        else if (collision.gameObject.tag.Equals("Trap"))
        {

            if (collision.transform.position.y > transform.position.y)
            {
                this.transform.rigidbody2D.velocity = new Vector2(0, 0);
                boxCollider2d.enabled = false;
                SoundEngine.instance.PlayOneShot(SoundEngine.instance._soundFail);
            }
            else
            {
                PlaySoundJump();
            }
        }

        else if (collision.gameObject.tag.Equals("Helicopter") || collision.gameObject.tag.Equals("Trap3"))
        {
            SoundEngine.instance.PlayOneShot(SoundEngine.instance._soundFail);
            this.transform.rigidbody2D.velocity = new Vector2(0, 0);
            boxCollider2d.enabled = false;
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
    }
    public void restart()
    {
      
        transform.rigidbody2D.velocity = new Vector2(0, 0);
        boxCollider2d.enabled = true;
        indexTrap = 0;
        transform.localPosition = new Vector3(0f, 0f, 1);
    }
    public void PlaySoundJump()
    {
        float x = transform.rigidbody2D.velocity.x;
        float y = transform.rigidbody2D.velocity.y;
        Debug.Log(x * x + y * y);
        if ((x * x + y * y) > 170)
            SoundEngine.instance.PlayOneShot(SoundEngine.instance._soundJumpLong);
        else
            SoundEngine.instance.PlayOneShot(SoundEngine.instance._soundJumpShoot);
    }
}
