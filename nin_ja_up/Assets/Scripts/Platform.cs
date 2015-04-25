using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	// Use this for initialization
    public GameObject _Platform;
    float _PlatformSize;
    public BoxCollider2D boxCollider;
    public int mNodeRelease = 0;
    Vector2 fingerPos1;
    Vector2 fingerPos2;
    public UI2DSprite ui2DSprite;
    
    public Animator anim;
    public static float Angle;

    public static Platform instance;
	void Start () {
      //  _Platform = this.gameObject;
        ui2DSprite = _Platform.GetComponent<UI2DSprite>();
        _Platform.SetActive(false);
        //_Platform.SetActive(false);
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) || ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            if(GamePlay.currentState == GamePlay.STATE_WATTING)
                if(!Player.instance.checkTouchBegin())
                {
                    return;
                }
            _Platform.SetActive(true);
            ui2DSprite.width = 90;


            if (Input.touchCount == 1)
            {
                fingerPos1 = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                fingerPos1 = Input.mousePosition;
            }
            fingerPos1 = Camera.main.ScreenToWorldPoint(fingerPos1);

            _Platform.transform.position = new Vector3(fingerPos1.x, fingerPos1.y, 0);
            

        }
        else if (Input.GetMouseButtonUp(0) || ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Ended)))
        {


            updateFinger2();

        }
        else if((Input.touchCount == 1) && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            updateFinger2();
        }

	}
    void updateFinger2()
    {
        if (Input.touchCount == 1)
        {
            fingerPos2 = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            fingerPos2 = Input.mousePosition;
        }
        fingerPos2 = Camera.main.ScreenToWorldPoint(fingerPos2);
        _PlatformSize = Space2Point(fingerPos1, fingerPos2);
        if (_PlatformSize > 3)
            _PlatformSize = 3;
        ui2DSprite.width = (int)(_PlatformSize * 160);
        boxCollider.size = new Vector2(_PlatformSize * 160 - 40, boxCollider.size.y);
        if (boxCollider.size.x<40)
            boxCollider.size = new Vector2(40, boxCollider.size.y);
        boxCollider.center = new Vector2((boxCollider.size.x) / 2, 0);
        calAngle(fingerPos1, fingerPos2);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.name.Equals("BirdObject"))
        {
            GamePlay.currentState = GamePlay.STATE_DROP;

            GamePlay.instance.initGameOver();
            GamePlay.TimePlayedSubState = 0f;
            //GamePlay.instance.showGameOver ();
            // Debug.Log("Tieu tui rui");
            SoundEngine.playSound("SoundFail");
            this.transform.rigidbody2D.velocity = new Vector2(0, 0);
        }*/
    }
    float Space2Point(Vector2 p1,Vector2 p2)
    {
        float S = Mathf.Sqrt((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y));        
        return S;
    }
  
    void calAngle(Vector2 p1,Vector2 p2)
    {
      //  p1 = new Vector2(0, 2);
       // p2 = new Vector2(2, 1);
        //float Angle = Vector2.Angle(p1, p2);
        //Debug.Log(p1.x +"," + p1.y + " ," +p2.x+","+ p2.y);
        Angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x);
        //Debug.Log(Angle);
        //Debug.Log(" ----------");
        _Platform.transform.eulerAngles = new Vector3(0, 0, Angle * 57.3f);
       
    }
}
