using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
	public bool isPass = false;
	// Use this for initialization
	void Start () {
		isPass = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public bool checkPass(float xtrap,float xbird)
	{
		if (xtrap < xbird + 0.3f)
			return true;
		return false;
	}
}
