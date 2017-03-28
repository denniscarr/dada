using UnityEngine;
using System.Collections;


public class CS_PlaySFX : MonoBehaviour {
	public AudioClip[] mySFX;

	[SerializeField] bool playOnce;

	[SerializeField] float playVolume;
	// Use this for initialization
	void Start () {
		
		if (playOnce) {
			Destroy (this);
		}
	}

    public void PlaySFX(int t_number) {
        if (playVolume == 0) {
			Services.AudioManager.PlaySFX(mySFX [t_number]);
        } else { 
			Services.AudioManager.PlaySFX(mySFX [t_number], playVolume);
            }
	}

	public void PlayRandomSFX () {
		PlaySFX (Random.Range (0, mySFX.Length));
	}

	public void PlaySFXPitchJitter(int t_number, float jitterAmt) {
		Services.AudioManager.PlaySFX (mySFX [t_number], (Random.value * jitterAmt) - (jitterAmt / 2f));
	}

	public void Play3DSFX(int t_number) {
		Services.AudioManager.Play3DSFX (mySFX [t_number], transform.position); 
	}
}
