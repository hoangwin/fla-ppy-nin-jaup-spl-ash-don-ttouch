using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float maxSpeed = 10f;
	private Vector2 movement;
	bool isCallJump = false;
	float currentAngle;
	GameObject birdObject;
	float mygravityScale = 1.5f;
	// Use this for initialization
	void Start () {
		DEF.Init ();
		//this.transform.rigidbody2D.AddForce (new Vector2 (0, 200));
		maxSpeed = 5f;
		Debug.Log ("maxSpeed :" + maxSpeed);
		this.transform.rigidbody2D.fixedAngle = true;
		birdObject = GameObject.Find("Bird");
	}
	
	// Update is called once per frame
	void Update () {

		switch (GamePlay.currentState) {
		case GamePlay.STATE_WATTING:
			if (Input.GetMouseButtonDown (0) ||  ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began))) {
				movement = new Vector2 (0, maxSpeed);
				isCallJump = true;	
				GamePlay.currentState = GamePlay.STATE_PLAY;
				GamePlay.instance.UI_READY.GetComponent<SpriteRenderer>().enabled = false;		
				GamePlay.instance.BirdObject.GetComponent<Rigidbody2D>().gravityScale = mygravityScale;
				SoundEngine.playSound ("SoundFly");
				//GameObject sound = GameObject.Find("SoundFly");
				//if (sound != null)
				//{
					// Debug.Log("Play Sound");
				//	sound.audio.Play();
				//}
			}
			break;
		case GamePlay.STATE_PLAY:
			if (Input.GetMouseButtonDown (0) ||  ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began))) {
				movement = new Vector2 (0, maxSpeed);
				isCallJump = true;
				SoundEngine.playSound ("SoundFly");
				//GameObject sound = GameObject.Find("SoundFly");
				//if (sound != null)
				//{
					// Debug.Log("Play Sound");
				//	sound.audio.Play();
				//}
			}
			break;
		case GamePlay.STATE_DROP:

			break;
		}

		float x = this.transform.rigidbody2D.velocity.x;
		float y = this.transform.rigidbody2D.velocity.y;
		if (y > maxSpeed)
		   y = maxSpeed;
		else if (y < -maxSpeed)
			y = -maxSpeed;
		currentAngle = y / maxSpeed;
		currentAngle = Mathf.Asin (currentAngle);
		currentAngle =currentAngle*Mathf.Rad2Deg/3;
	//	GameObject.Find("Box1").transform.localRotation  =Quaternion.Euler( new Vector3(0,0,Random.Range(0,360)));
		if(GamePlay.currentState == GamePlay.STATE_DROP || GamePlay.currentState ==GamePlay.STATE_OVER)
			this.transform.localRotation =  Quaternion.Euler( new Vector3(0,0,-90));
		else
			this.transform.localRotation =  Quaternion.Euler( new Vector3(0,0,currentAngle));
		//this.transform.Rotate ( new Vector3 (0, 0, currentAngle));
		//Debug.Log (currentAngle);
	}
	void FixedUpdate()
	{
		if (isCallJump)
		{
			rigidbody2D.velocity = movement;
			isCallJump = false;
		}
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
		GamePlay.currentState = GamePlay.STATE_DROP;

		GamePlay.instance.initGameOver ();
		GamePlay.TimePlayedSubState = 0f;
		//GamePlay.instance.showGameOver ();
		Debug.Log ("Tieu tui rui");
		SoundEngine.playSound ("SoundFail");
		//GameObject sound = GameObject.Find("SoundFail");
		//if (sound != null)
		//{
			// Debug.Log("Play Sound");
		//	sound.audio.Play();
		//}
	}
}
