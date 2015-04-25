using UnityEngine;
using System.Collections;

public class SoundEngine : MonoBehaviour {
	// Use this for initialization
	public static bool isSound = true;
    public static bool isMusic = true;
	
    public static AudioSource[] soundSelect;
    public static AudioSource[] soundFruitExlotion;

    public static void initGamePlaySound()
    {
        if (soundSelect == null)
        {
            soundSelect = new AudioSource[11];

            
        }
        if (soundSelect[0] == null)
        {
            for (int i = 0; i < 11; i++)
                soundSelect[i] = GameObject.Find("select" + i.ToString()).audio;
        }
        if(soundFruitExlotion == null)
            soundFruitExlotion = new AudioSource[11];
        if (soundFruitExlotion[0] == null)
        {

            for (int i = 0; i < 11; i++)
            {
                GameObject obj = (GameObject)(Instantiate(GameObject.Find("FruitExplotion2")));
                soundFruitExlotion[i] = obj.audio;
            }
            
        }
    }
    public static void playSoundFruitExplotion(int i)
    {
        if (i >= 11)
            i = 10;
        if (SoundEngine.isSound)
        {
            if(!soundFruitExlotion[i].isPlaying)
                soundFruitExlotion[i].Play();
        }

    }
    public static void playSoundSelectFruit(int i)
    {
        if (i >= 11)
            i = 10;
        if (SoundEngine.isSound)
        {
            soundSelect[i].Play();
        }

    }


    public static void playSound(string str)
	{
		if (SoundEngine.isSound) {
			GameObject sound = GameObject.Find (str);
			if (sound != null) {
                if(!sound.audio.isPlaying)				
				    sound.audio.Play();
			}
		}
	}
    public static void playSoundBG(string str)
    {
        if (SoundEngine.isMusic)
        {
            GameObject sound = GameObject.Find(str);
            if (sound != null)
            {
                if (!sound.audio.isPlaying)
                    sound.audio.Play();
            }
        }
    }

	public static void stopSound(string str)
	{
		//if (SoundEngine.isSound)
		{
			GameObject sound = GameObject.Find(str);
			if (sound != null)
			{             
				sound.audio.Stop();
			}
		}
	}
}
