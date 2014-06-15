using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    // Singleton  
    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance ?? (_instance = FindObjectOfType<SoundManager>()); } }

    public AudioClip BulletSound;
    public AudioClip PowerUpSound;

	// Use this for initialization
	void Start () {
	
	}
	
	public void Play(AudioClip clip)
	{
	    AudioSource.PlayClipAtPoint(clip, Vector2.zero);
	}
}
