using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	// Use this for initialization
    public GameObject Trap1;
    public GameObject Trap2;
    public GameObject Trap3;
    public GameObject Trap4;
    public static Trap instance;
	void Start () {
        init();
        instance = this;
	}
	public void init()
    {
        this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Trap1.GetComponent<Animator>().enabled = true;
        Trap2.GetComponent<Animator>().enabled = true;
        Trap3.GetComponent<Animator>().enabled = true;
        Trap4.GetComponent<Animator>().enabled = true;
        Trap1.SetActive(false);
        Trap2.SetActive(false);
        Trap3.SetActive(false);
        Trap4.SetActive(false);
    }
    public IEnumerator CallTrap()
    {
        
        yield return new WaitForSeconds(1 * 0.1f);
        if (GamePlay.currentState == GamePlay.STATE_PLAY)
            Trap2.SetActive(true);
        yield return new WaitForSeconds(1 * 0.5f);
        if (GamePlay.currentState == GamePlay.STATE_PLAY)
            Trap3.SetActive(true);
        yield return new WaitForSeconds(1 * 0.5f);
        if (GamePlay.currentState == GamePlay.STATE_PLAY)
            Trap4.SetActive(true);
        yield return new WaitForSeconds(1 * 0.5f);
        if (GamePlay.currentState == GamePlay.STATE_PLAY)
            Trap1.SetActive(true);
        yield break;

    }
    public void Begin()
    {
        StartCoroutine("CallTrap");
    }
	// Update is called once per frame
	void Update () {
        if(GamePlay.currentState == GamePlay.STATE_PLAY)
            transform.Rotate(0, 0, 2*Time.deltaTime);

	}
    public void end()
    {
      
        Trap1.GetComponent<Animator>().enabled = false;
        Trap2.GetComponent<Animator>().enabled = false;
        Trap3.GetComponent<Animator>().enabled = false;
        Trap4.GetComponent<Animator>().enabled = false;
    }

}
