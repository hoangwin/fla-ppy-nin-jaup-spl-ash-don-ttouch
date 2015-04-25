using UnityEngine;
using System.Collections;

public class BirdAnim : MonoBehaviour {

	// Use this for initialization
	public bool isLive = true; 
	public void setDestroy()
	{
		isLive = false;
	}
	public void readyAnimFinish()
	{
		if(GamePlay.currentState ==GamePlay.STATE_WATTING)
			GamePlay.instance.setbegin ();
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
