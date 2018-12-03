using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
    public bool PauseMenu=false;
    
    public GameObject PausePanel;

    public GunSystem shootOrNot;

    public bool EndGame = false;

	// Use this for initialization
	void Start () {
        shootOrNot = GameObject.Find("Player").GetComponent<GunSystem>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && EndGame!=true)
        {
            PauseMenu = !PauseMenu;
            if (PauseMenu)
            {
                PausePanel.SetActive(true);
               
                GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                
            }
                        
        }



	}

    public void ResumeGame()
    {
        
        PausePanel.SetActive(false);
        GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        PauseMenu = !PauseMenu;
        shootOrNot.gainLife();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
