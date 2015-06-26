using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float maxSpeedX = 10f;
    public float maxSpeedY = 10f;
    private Vector2 movement;
    bool isCallJump = false;
    float currentAngle;    
    Vector2 squaVec2Jump;
    public Transform PointGravity;
    public Vector3 gravity;
    public int gravitySpeed;
    public Animator anim;

    public Vector3 beginPosition;
    public static Player instance;
    public int index = 1;
    void Start()
    {
        DEF.Init();
        //this.transform.rigidbody2D.AddForce (new Vector2 (0, 200));
        Debug.Log("maxSpeed :" + maxSpeedX);
        //this.transform.GetComponent<Rigidbody2D>().fixedAngle = true;        
        Physics2D.gravity = Vector3.zero;
        anim.Play("Fly_IDE" + index.ToString());
        beginPosition = transform.position;
        instance = this;
    }
    public void reStart()
    {
        Physics2D.gravity = Vector3.zero;
        transform.position = beginPosition;
        this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().isKinematic = false;

    }

    void FixedUpdate()
    {
        if (GamePlay.currentState == GamePlay.STATE_PLAY)
        {
            float x = PointGravity.position.x - transform.position.x;
            float y = PointGravity.position.y - transform.position.y;
            gravity = (PointGravity.transform.position - transform.position).normalized;
            gravity *= gravitySpeed;
            transform.gameObject.GetComponent<Rigidbody2D>().AddForce(gravity);
        }
        if (isCallJump)
        {
            GetComponent<Rigidbody2D>().velocity = movement;
            //GetComponent<Rigidbody2D>().AddForce(movement);
            isCallJump = false;
        }
    }
    // Update is called once per frame

    void Update()
    {

        switch (GamePlay.currentState)
        {
            case GamePlay.STATE_WATTING:
                {
                    if (Input.GetMouseButtonDown(0) || ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began)))
                    {
                        Trap.instance.Begin();
                        Vector2 tempVec2 = -1 * gravity;
                        squaVec2Jump = Vector2.zero;
                        squaVec2Jump.x = tempVec2.y;
                        squaVec2Jump.y = -tempVec2.x;
                        squaVec2Jump.x = (squaVec2Jump.x + tempVec2.x) / 2;
                        squaVec2Jump.y = (squaVec2Jump.y + tempVec2.y) / 2;
                        squaVec2Jump.Normalize();
                        movement = new Vector2(maxSpeedX * 0.4f, maxSpeedY * 0.6f);
                        isCallJump = true;
                        SoundEngine.playSound("SoundFly");
                        GamePlay.currentState = GamePlay.STATE_PLAY;
                        SoundEngine.playSound("SoundFly");
                        //Debug.Log(movement);
                      GamePlay.instance.UI_READY.SetActive(false);        
                    }

                    break;
                }
            case GamePlay.STATE_PLAY:


                if (Input.GetMouseButtonDown(0) || ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began)))
                {

                    Vector2 tempVec2 = -1 * gravity;
                    squaVec2Jump = Vector2.zero;
                    squaVec2Jump.x = tempVec2.y;
                    squaVec2Jump.y = -tempVec2.x;
                    squaVec2Jump.x = (squaVec2Jump.x + tempVec2.x) / 2;
                    squaVec2Jump.y = (squaVec2Jump.y + tempVec2.y) / 2;
                    squaVec2Jump.Normalize();
                    movement = new Vector2(maxSpeedX * squaVec2Jump.x, maxSpeedY * squaVec2Jump.y);
                    isCallJump = true;
                    SoundEngine.playSound("SoundFly");

                }
                float angle = AngleSigned(new Vector3(0, -1, 0),transform.position);
              //  Debug.Log(angle);               
              //  Debug.Log(PointGravity.position);
              //  Debug.Log(transform.position);

                //	GameObject.Find("Box1").transform.localRotation  =Quaternion.Euler( new Vector3(0,0,Random.Range(0,360)));
                if ( GamePlay.currentState == GamePlay.STATE_OVER)
                    this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                else
                    this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
                break;

          
        }

        //  forces2 =  planets.transform.position- transform.position;
        //  forces2.z = 0;
        //  forces2.Normalize();
        //  forces += forces2 / 500;
        //forces = (forces + forces2) / 2;

        //   transform.Translate(forces,Space.World);
    }

   

    // StopCoroutine("changeDirectionAfterDelay");
    public static float AngleSigned(Vector3 v1, Vector3 v2)
    {
        int sign = Vector3.Cross(v1, v2).z < 0 ? -1 : 1;        
        return sign * Vector3.Angle(v1, v2);
    }
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        GamePlay.currentState = GamePlay.STATE_OVER;
        //Time.timeScale = 0;        
        GamePlay.TimePlayedSubState = 0f;
        //GamePlay.instance.showGameOver ();
       // Debug.Log("Tieu tui rui");
        SoundEngine.playSound("SoundFail");

        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Trap.instance.end();
        
        AnimationEffect.instance.GameOverBegin();
        //save score
        if (ScoreControl.BEST_SCORE.NUM < ScoreControl.currentScore)
            ScoreControl.BEST_SCORE.NUM = ScoreControl.currentScore;
        ScoreControl.BEST_SCORE.Save();
        ButtonControl.instance.setGameOver();
        GamePlay.ShowADS();
    }
     void OnTriggerExit2D( Collider2D other) {
         ScoreControl.currentScore++;
         GamePlay.instance.UI_TEXT_CURRENT_SCORE.text = ScoreControl.currentScore.ToString();
         SoundEngine.playSound("SoundCoin");       
	//characterInQuicksand = false;
       //  Debug.Log("aaaaaaaaaaaaac");
}
}
