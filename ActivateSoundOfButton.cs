using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSoundOfButton : MonoBehaviour {
    public AudioSource bipSound;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playSound()
    {
        bipSound.Play();
    }
}
