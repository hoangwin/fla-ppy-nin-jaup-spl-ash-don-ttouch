using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationEffect : MonoBehaviour {

	// Use this for initialization
    public GameObject Title;    
    Vector3 beginTitle;

    public GameObject MainMenuTextHightScore;
    Vector3 beginMainMenuTextHightScore;

    public GameObject Button;
    Vector3 beginButton;

    public GameObject BackGround;
    public Color color;
    public SpriteRenderer ImageBG;
    //GameOver
    public GameObject GameObjectGameOverText;
    Vector3 beginPositionGameOverText;

    public GameObject GameObjectGameOverButton;
    Vector3 beginPositionGameOverButton;

    public static AnimationEffect instance;
	void Start () {
        beginTitle = Title.transform.position;
        beginButton = Button.transform.position;
        beginPositionGameOverText = GameObjectGameOverText.transform.position;
        beginPositionGameOverButton = GameObjectGameOverButton.transform.position;
	    instance = this;
        InitMainMenuEffect();
        color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void InitMainMenuEffect()
    {
        Button.transform.position = beginButton;
        Title.transform.position = beginTitle;
        iTween.MoveFrom(Title, iTween.Hash("y", 5 * Screen.height / 4, "easeType", "easeInOutElastic", "time", 1.5));
        iTween.MoveFrom(Button, iTween.Hash("y", -Screen.height / 5, "time", 1.5, "easeType", "easeInOutElastic"));

    }

    public void EndMainMenuEffect()
    {
        iTween.MoveTo(Title, iTween.Hash("y", 5*Screen.height/4 , "time", 0.5, "easeType",iTween.EaseType.linear, "oncomplete", "MainMenuMoveEndCOmpleted"));
        iTween.MoveTo(Button, iTween.Hash("y", -5 * Screen.height / 4, "time", 1));

    }
    public void GameOverBegin()
    {
        iTween.ValueTo(this.gameObject, iTween.Hash("from", 1, "to", 0.5, "time", 1.5f, "onupdate", "onUpdateValue"));
        GameObjectGameOverText.transform.position = beginPositionGameOverText;
        GameObjectGameOverButton.transform.position = beginPositionGameOverButton;
        iTween.MoveFrom(GameObjectGameOverText, iTween.Hash("y", Screen.height*3/4, "time", 0.5, "easeType", iTween.EaseType.linear));
        iTween.MoveFrom(GameObjectGameOverButton, iTween.Hash("y", -Screen.height/8, "time", 1));
    }

    public void GameOverRePlay()
    {
        iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.5f, "to",2 , "time", 1f, "onupdate", "onUpdateValue"));
        iTween.MoveTo(GameObjectGameOverText, iTween.Hash("y", Screen.height*3/4, "time", 0.5, "easeType", iTween.EaseType.linear, "oncomplete", "GameOverReplayAnimCOmpleted"));
        iTween.MoveTo(GameObjectGameOverButton, iTween.Hash("y", -Screen.height / 8, "time", 1));

    }
    public void GameOverToMainMenu()
    {
        iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.5f, "to", 2, "time", 1f, "onupdate", "onUpdateValue"));
        iTween.MoveTo(GameObjectGameOverText, iTween.Hash("y", Screen.height * 3 / 4, "time", 0.5, "easeType", iTween.EaseType.linear, "oncomplete", "GameOverToMainMenuAnimCOmpleted"));
        iTween.MoveTo(GameObjectGameOverButton, iTween.Hash("y", -Screen.height / 8, "time", 1));

    }
    public void WAITINGToMainMenu()
    {
      //  iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.5f, "to", 2, "time", 1f, "onupdate", "onUpdateValue"));
        GamePlay.currentState = GamePlay.STATE_MAINMENU;
        ButtonControl.instance.setMainMenu();
        AnimationEffect.instance.InitMainMenuEffect();
        Player.instance.reStart();
        Trap.instance.init();
        //iTween.MoveTo(GameObjectGameOverText, iTween.Hash("y", Screen.height * 3 / 4, "time", 0.5, "easeType", iTween.EaseType.linear, "oncomplete", "GameOverToMainMenuAnimCOmpleted"));
        //iTween.MoveTo(GameObjectGameOverButton, iTween.Hash("y", -Screen.height / 8, "time", 1));

    }
    public void GameOverDirectToMainMenu()
    {
        color.b = 1;
        color.g = 1;
        ImageBG.color = color;  
        //  iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.5f, "to", 2, "time", 1f, "onupdate", "onUpdateValue"));
        Player.instance.reStart();
        Trap.instance.init();
        GamePlay.currentState = GamePlay.STATE_MAINMENU;
        ButtonControl.instance.setMainMenu();
        AnimationEffect.instance.InitMainMenuEffect();
        
        //iTween.MoveTo(GameObjectGameOverText, iTween.Hash("y", Screen.height * 3 / 4, "time", 0.5, "easeType", iTween.EaseType.linear, "oncomplete", "GameOverToMainMenuAnimCOmpleted"));
        //iTween.MoveTo(GameObjectGameOverButton, iTween.Hash("y", -Screen.height / 8, "time", 1));

    }
    public void GamePlayDirectToMainMenu()
    {
        color.b = 1;
        color.g = 1;
        ImageBG.color = color;        
        Player.instance.reStart();
        Trap.instance.init();
        GamePlay.currentState = GamePlay.STATE_MAINMENU;
        ButtonControl.instance.setMainMenu();
        AnimationEffect.instance.InitMainMenuEffect();
    }
    public void onUpdateValue(float a)
    {
        color.b = a/2;
        color.g = a / 2;
        ImageBG.color = color;        
       //	Debug.Log (a);
    }

}
