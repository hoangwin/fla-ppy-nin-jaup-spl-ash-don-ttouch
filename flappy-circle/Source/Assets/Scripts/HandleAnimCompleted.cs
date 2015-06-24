using UnityEngine;
using System.Collections;

public class HandleAnimCompleted : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public  void MainMenuMoveEndCOmpleted()
    {
        GamePlay.currentState = GamePlay.STATE_WATTING;
        ButtonControl.instance.setUI();
    }

    public void GameOverReplayAnimCOmpleted()
    {
        GamePlay.currentState = GamePlay.STATE_WATTING;
        ButtonControl.instance.setUI();
    }

    public void GameOverToMainMenuAnimCOmpleted()
    {
        Debug.Log("aa");
        GamePlay.currentState = GamePlay.STATE_MAINMENU;
        ButtonControl.instance.setMainMenu();
        AnimationEffect.instance.InitMainMenuEffect();
        Player.instance.reStart();
        Trap.instance.init();
    }
}
