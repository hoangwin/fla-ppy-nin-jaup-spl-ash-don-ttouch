using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	// Use this for initialization
	GameObject BottomBG;
	GameObject BottomBG1;
	void Start () {
		DEF.Init ();
		BottomBG = GameObject.Find("BottomBG");
		BottomBG1 = GameObject.Find("BottomBG1");
		DEF.ScaleAnchorGui();
		ScoreControl.loadGame();
	}	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		if (BottomBG.transform.localPosition.x < -8)
			BottomBG.transform.localPosition = new Vector3 (BottomBG1.transform.localPosition.x + 8 - GamePlay.speedx, BottomBG.transform.localPosition.y, 1);
		else
			BottomBG.transform.localPosition = new Vector3 (BottomBG.transform.localPosition.x - GamePlay.speedx, BottomBG.transform.localPosition.y, 1);
		
		if (BottomBG1.transform.localPosition.x < -8)
			BottomBG1.transform.localPosition = new Vector3 (BottomBG.transform.localPosition.x + 8 - GamePlay.speedx, BottomBG1.transform.localPosition.y, 1);
		else
			BottomBG1.transform.localPosition = new Vector3 (BottomBG1.transform.localPosition.x - GamePlay.speedx, BottomBG1.transform.localPosition.y, 1);
	}
}
