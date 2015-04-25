using UnityEngine;
using System.Collections;

public class SoundEngine : MonoBehaviour {
	// Use this for initialization
	public static bool isSound = true;
	public static GameObject soundclick = null;

	// Use this for initialization
	//void Start () {
	
	//}
	
	// Update is called once per frame
	//void Update () {
	
	//}
	
	public static void playSound(string str)
	{
		if (SoundEngine.isSound) {
			GameObject sound = GameObject.Find (str);
			if (sound != null) {
				//	 Debug.Log("Play Sound");			
				sound.audio.Play();
			}
		}
	}
	
	public static void stopSound(string str)
	{
		if (SoundEngine.isSound)
		{
			GameObject sound = GameObject.Find(str);
			if (sound != null)
			{             
				sound.audio.Stop();
			}
		}
	}
}
