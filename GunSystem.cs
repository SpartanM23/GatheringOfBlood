using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour {

    public Transform gun;
    public SpriteRenderer gunSprite;
    public bulletPool poolOfBullet;
    public GameObject bullet;

    public bool activateGun = true;
    public RopeSystem activateOrNotHook;

    Vector3 mousePosition;
    Vector3 direction;

    public int lifePlayer = 7;
    public int lifeProps = 2;
    public List<GameObject> lifeCase = new List<GameObject>();
    public List<GameObject> lifeCaseAll = new List<GameObject>();

    public AudioSource shootEffectSound;

    public Pause pause_Data_GunSystem;

    int index=1;

    public GameObject panelGameOver;

    // Use this for initialization
    void Start () {
        while (lifeCase.Count != 7)
        {
            foreach (GameObject caseLife in GameObject.FindGameObjectsWithTag("Life"))
            {
                if (caseLife.name == ("Case" + index))
                {
                    lifeCase.Add(caseLife);
                    lifeCaseAll.Add(caseLife);
                    index += 1;
                }

            }
        }
        
        activateOrNotHook = this.GetComponent<RopeSystem>();
    }
	
	// Update is called once per frame
	void Update () {
       


        var worldMousePosition =
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
       
        if(pause_Data_GunSystem.PauseMenu != true) {
            SetGunPosition(aimAngle);

            if (Input.GetMouseButtonUp(0) && activateOrNotHook.activateHook!=true && lifePlayer > 0)
            {

                shootEffectSound.Play();
                lifePlayer -= 1;


                lifeCase[lifePlayer].SetActive(false);
                lifeCase.Remove(lifeCase[lifePlayer]);

                

                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0.0f;
                direction = (mousePosition - transform.position).normalized;

                int i = 0;

                while (i < poolOfBullet.pooledObjects.Count)
                {

                    if (poolOfBullet.pooledObjects[i].active == false)
                    {
                        poolOfBullet.pooledObjects[i].transform.position = gun.position;
                        poolOfBullet.pooledObjects[i].SetActive(true);
                        bullet = poolOfBullet.pooledObjects[i];
                        i = poolOfBullet.pooledObjects.Count + 1;
                    }
                    i++;
                }


                bullet.GetComponent<Rigidbody2D>().velocity = direction * 25;
            }
        }
        
        if(lifePlayer<=0 && lifeProps<=0)
        {
            pause_Data_GunSystem.EndGame = true;
            pause_Data_GunSystem.PauseMenu = true;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            panelGameOver.SetActive(true);
        }

    }

    public void gainLife()
    {
        lifePlayer += 1;
        lifeCase.Add(lifeCaseAll[lifePlayer]);
        lifeCaseAll[lifePlayer].SetActive(true);
        
    }

    private void FixedUpdate()
    {
        
    }   

    private void SetGunPosition(float aimAngle)
    {
        if (!gunSprite.enabled)
        {
            gunSprite.enabled = true;
        }

        var x = transform.position.x + 0.5f * Mathf.Cos(aimAngle);
        var y = transform.position.y + 0.5f * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        gun.transform.position = crossHairPosition;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+90));
        
    }

}
