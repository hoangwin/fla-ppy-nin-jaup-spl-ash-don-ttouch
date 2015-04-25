using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float maxSpeedx = 0.01f;
	public float maxSpeedy = 0.1f;
	private Vector2 movement;
	bool isCallJump = false;
	float currentAngle;
	//GameObject birdObject;
	float mygravityScale = 1f;
	// Use this for initialization
	public int myLayer = 11;//de loai bo collition giua cac con chim
	void Start () {

		maxSpeedx = Random.Range(40f,110f)/10;
		maxSpeedy = 6f;
		movement = new Vector2 (maxSpeedx, 0);
		isCallJump = true;	
		DEF.Init ();
		//this.transform.rigidbody2D.AddForce (new Vector2 (0, 200));
		//maxSpeed = 3f;
		Debug.Log ("maxSpeed :" + maxSpeedy);
		this.transform.rigidbody2D.fixedAngle = true;
		//birdObject = GameObject.Find("Bird");
	}
	
	// Update is called once per frame
	void Update () {

		switch (GamePlay.currentState) {
		case GamePlay.STATE_WATTING:

			break;
		case GamePlay.STATE_PLAY:

			if (this.transform.localPosition.y <0.0) {
				movement = new Vector2 (maxSpeedx, maxSpeedy);
				Debug.Log("MAXSpeed : " + maxSpeedx +"," + maxSpeedy);
				isCallJump = true;
			}
			if (this.transform.localPosition.x > 6.0)
			{
				SoundEngine.playSound ("Sound_Lose");
				//GameObject sound = GameObject.Find("Sound_Lose");
				//if (sound != null)
				//{
				//	Debug.Log("Play sound:hhh");
				//	sound.audio.Play ();
				//}
				GamePlay.currentState = GamePlay.STATE_OVER;
				GamePlay.instance.initGameOver ();
				GamePlay.instance.showGameOver();
				GamePlay.TimePlayedSubState = 0f;
			}
			break;
		case GamePlay.STATE_DROP:

			break;
		}
		if (GamePlay.currentState != GamePlay.STATE_OVER) {
			float x = this.transform.rigidbody2D.velocity.x;
			float y = this.transform.rigidbody2D.velocity.y;
			if (y > maxSpeedy)
					y = maxSpeedy;
			else if (y < -maxSpeedy)
					y = -maxSpeedy;
			currentAngle = y / maxSpeedy;
			currentAngle = Mathf.Asin (currentAngle);
			currentAngle = currentAngle * Mathf.Rad2Deg / 10;
			//	GameObject.Find("Box1").transform.localRotation  =Quaternion.Euler( new Vector3(0,0,Random.Range(0,360)));
			if (GamePlay.currentState == GamePlay.STATE_DROP || GamePlay.currentState == GamePlay.STATE_OVER)
					this.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, -90));
			else
					this.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, currentAngle));
				}
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
	
		//GamePlay.instance.showGameOver ();
		Debug.Log ("Tieu tui rui");

		if (transform.localPosition.x < 0.6) {
			movement = new Vector2 (-maxSpeedx, maxSpeedy);
			isCallJump = true;
		}
		else{
			Animator ani = transform.Find ("BirdObjectAnim").GetComponent<Animator> ();
			ani.Play ("Bird_Destroy");

			transform.rigidbody2D.rigidbody2D.velocity = new Vector2 (0, 0);
			transform.rigidbody2D.rigidbody2D.gravityScale = 0f;
			transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, 0));


			BoxCollider2D [] arraycollider = GetComponents<BoxCollider2D> ();
			for (int j =0; j<arraycollider.Length; j++)
					arraycollider [j].enabled = false;
			ScoreControl.Score +=1;
			GameObject.Find("LabelScoreInGame").GetComponent<UILabel>().text =""+ ScoreControl.getRealScore();
		}
		SoundEngine.playSound ("SoundFail");
		//GameObject sound = GameObject.Find("SoundFail");
		//if (sound != null)
		//{
			// Debug.Log("Play Sound");
		//	sound.audio.Play();
		//}
	}
}
