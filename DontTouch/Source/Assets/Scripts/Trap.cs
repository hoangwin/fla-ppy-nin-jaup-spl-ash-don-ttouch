using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
	public int type = 0;
    public const int TYPE_CANT_TOUCH = 0;
    public const int TYPE_CAN_TOUCH = 1;
    public const int TYPE_TOUCHED = 2;
    public
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}
	public bool checkPass(float xtrap,float xbird)
	{
        //if (xbird < xbird + 0.3f)
		//	return true;
		return false;
	}
}
