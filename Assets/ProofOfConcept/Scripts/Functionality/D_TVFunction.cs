using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_TVFunction : D_Function {
	private float timeSpeed;
	GameObject ambientMusic;
	// Use this for initialization
	new void Start () {
		base.Start ();
		ambientMusic = GameObject.Find ("MusicSources");
		timeSpeed = Random.Range (0.1f, 10f);
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		Time.timeScale = timeSpeed;
		foreach (AudioSource asource in ambientMusic.GetComponentsInChildren<AudioSource>()) {
			if (asource.isPlaying) {
				asource.pitch = timeSpeed;
			}
		}
        Invoke("BackToNormal", 60f);
	}

    void BackToNormal()
    {
        Time.timeScale = 1f;
		foreach (AudioSource asource in ambientMusic.GetComponentsInChildren<AudioSource>()) {
			if (asource.isPlaying) {
				asource.pitch = Time.timeScale;
			}
		}
    }
}
