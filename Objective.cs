using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour {
    public GunSystem gunSystem_Data;
    public int EnemyToKill = 7;
    public Text EnemyToKill_Text;
    public GameObject VictoryScreen;
    public Pause Pause_Data;
    // Use this for initialization
    void Start () {
        Pause_Data = GetComponent<Pause>();
    }
	
	// Update is called once per frame
	void Update () {
        if (EnemyToKill == 0)
        {
            Pause_Data.EndGame = true;
            Pause_Data.PauseMenu = true;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            VictoryScreen.SetActive(true);
        }
	}

    public void changeNumberOfEnemyLeft()
    {
        EnemyToKill_Text.text = EnemyToKill.ToString();
    }
}
