using UnityEngine;
using System.Collections;

public class SoundEngine : MonoBehaviour {
	public static bool isSound = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public static void playSound(string str)
	{
		if (SoundEngine.isSound) {
			GameObject sound = GameObject.Find (str);
			if (sound != null) {
					// Debug.Log("Play Sound");
					sound.GetComponent<AudioSource>().Play ();
			}
		}
	}
}
