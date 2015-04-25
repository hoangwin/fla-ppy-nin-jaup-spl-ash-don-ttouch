using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
	public bool isPass = false;
	// Use this for initialization
    public int type = 0;
    public float speedx;
	void Start () {
		isPass = false;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (type == 3)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + speedx * Time.deltaTime, transform.localPosition.y, 1);
          
            if (transform.position.x < -2.5)
                speedx = Mathf.Abs(speedx);// Random.Range(0.7f,1.1f);
            else if (transform.position.x > 2.5)
                speedx = -Mathf.Abs(speedx);// speedx = Random.Range(-0.7f, -1.1f);
        }
    }
	
}
