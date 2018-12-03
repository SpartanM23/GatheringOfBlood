using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour {

    private float sec = 5f;
    private GunSystem modifyLifePlayer;
    public Objective objective_Data;
    public Enemy enemy_Data;


    void Start()
    {
        modifyLifePlayer = GameObject.Find("Player").GetComponent<GunSystem>();
        objective_Data = GameObject.Find("GameComponent").GetComponent<Objective>();
        
        StartCoroutine(LateCall());
    }

    IEnumerator LateCall()
    {

        yield return new WaitForSeconds(sec);

        this.gameObject.SetActive(false);

    }

    void OnCollisionEnter2D(Collision2D col)
    {
  
       if (col.gameObject.name == "Player")
        {
            
       
            
        }
        else if(col.gameObject.tag == "Ennemi")
        {
            enemy_Data = GameObject.Find(col.gameObject.name).GetComponent<Enemy>();
            enemy_Data.kill();
            objective_Data.EnemyToKill -= 1;
            objective_Data.changeNumberOfEnemyLeft();
            modifyLifePlayer.lifeCase.Add(modifyLifePlayer.lifeCaseAll[modifyLifePlayer.lifePlayer]);
            modifyLifePlayer.lifeCaseAll[modifyLifePlayer.lifePlayer].SetActive(true);
            modifyLifePlayer.lifePlayer += 1;
            Debug.Log(col.transform.name);
            this.gameObject.SetActive(false);
        }
        else if (col.gameObject.tag == "Life")
        {
           
            modifyLifePlayer.lifeCase.Add(modifyLifePlayer.lifeCaseAll[modifyLifePlayer.lifePlayer]);
            modifyLifePlayer.lifeCaseAll[modifyLifePlayer.lifePlayer].SetActive(true);
            modifyLifePlayer.lifePlayer += 1;
            Debug.Log(col.transform.name);
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(col.transform.name);
            this.gameObject.SetActive(false);
        }
    }
}
