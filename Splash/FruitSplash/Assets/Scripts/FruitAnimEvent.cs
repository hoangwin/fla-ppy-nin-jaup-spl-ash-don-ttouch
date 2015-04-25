using UnityEngine;
using System.Collections;

public class FruitAnimEvent : MonoBehaviour {
    public GameObject superObject;
    public void SetWattingRenew()
    {
        //GameObject obj = bubbleTableArray[j,i];
        //bubbleTableArray[j,i] = null;
        //	GameObject.Destroy(superObject);
        
        GamePlay.tableFruit[superObject.GetComponent<FruitAnim>().row,superObject.GetComponent<FruitAnim>().col].GetComponent<Fruit>().state = Fruit.FRUIT_STATE_RENEW;
        if(GamePlay.isX2Effect)
            GamePlay.tempScore += 200;
        else
            GamePlay.tempScore += 100;
        if (GamePlay.gameMode == GamePlay.GAME_MODE_UNLESS)
        {
            if(GamePlay.isX2Effect)
                GamePlay.timmerCountDown += 0.6f;
            else
                GamePlay.timmerCountDown += 0.3f;
        }
        GamePlay.instance.labelScore.text = GamePlay.tempScore.ToString();        
    }
}
