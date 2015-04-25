using UnityEngine;
using System.Collections;

public class PanelMove : MonoBehaviour {

	// Use this for initialization
    public void IGMMoveComplete()
    {
        Debug.Log("rrrrrrrrrrrrrrrr");
        GamePlay.instance.PanelPause.SetActive(false);
        GamePlay.instance.objIGMButton.SetActive(true);

    }

    public void PauseMoveComplete()
    {
        GamePlay.instance.initgame();
        GamePlay.instance.PanelPause.SetActive(false);
        GamePlay.instance.objIGMButton.SetActive(true);
    }
	public void OverMoveComplete()
	{
		GamePlay.instance.initgame();
		GamePlay.instance.PanelPause.SetActive(false);
		GamePlay.instance.objIGMButton.SetActive(true);

	}
}
