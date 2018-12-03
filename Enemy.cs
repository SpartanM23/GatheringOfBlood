using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public AudioSource deathSoundEffect;
    public Objective objective_Data;
	// Use this for initialization
	void Start () {
        deathSoundEffect = GameObject.Find("DeathSound").GetComponent<AudioSource>();
        objective_Data = GameObject.Find("GameComponent").GetComponent<Objective>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void kill()
    {
        objective_Data.EnemyToKill -= 1;
        deathSoundEffect.Play();
        this.gameObject.SetActive(false);
    }
}
