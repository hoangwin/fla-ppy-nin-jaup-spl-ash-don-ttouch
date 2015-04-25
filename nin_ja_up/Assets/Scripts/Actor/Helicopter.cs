using UnityEngine;
using System.Collections;

public class Helicopter : MonoBehaviour {

	// Use this for initialization
    public float speedx;
    public float speedy;
    public int speedOffset;
	void Start () {
        speedOffset = Random.Range(0, 1);
        if (speedOffset == 0)
            speedOffset = -1;
        speedx = speedOffset * Random.Range(0.3f, 0.7f);
        speedy = Random.Range(-0.5f, -0.7f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(transform.localPosition.x + speedx * Time.deltaTime, transform.localPosition.y + speedy*Time.deltaTime, 1);
        if(speedx>0)
        {
          //  speedx -= 0.05f * Time.deltaTime;
          //  if (speedx < 0.3f)
          //      speedx = -0.7f;
        }else
        {
           // speedx += 0.05f * Time.deltaTime;
           // if (speedx > -0.3f)
           //     speedx = 0.7f;
        }
        if (transform.position.x < -2.5)
            speedx =Mathf.Abs(speedx);// Random.Range(0.7f,1.1f);
        else if (transform.position.x > 2.5)
            speedx = -Mathf.Abs(speedx);// speedx = Random.Range(-0.7f, -1.1f);
        //check out screen

	}
}
