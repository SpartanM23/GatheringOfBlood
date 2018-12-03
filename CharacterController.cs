using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 1f;
    public float jumpSpeed = 3f;
    public bool groundCheck;
    public bool isSwinging;
    private SpriteRenderer playerSprite;
    private Rigidbody2D rBody;
    private bool isJumping;
    private Animator animator;
    private float jumpInput;
    private float horizontalInput;

    public Vector2 ropeHook;
    public float swingForce = 4f;

    public GunSystem gunSystem_Data;

    public AudioSource jumpSoundEffect;

    public GameObject panelGameOver;
    public Pause Pause_Data;

    void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gunSystem_Data = GetComponent<GunSystem>();
        Pause_Data = GameObject.Find("GameComponent").GetComponent<Pause>();

    }

    void Update()
    {
        jumpInput = Input.GetAxis("Jump");
        horizontalInput = Input.GetAxis("Horizontal"+GameObject.Find("ScriptContainer").GetComponent<InputSelected>().langage);
        var halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.04f), Vector2.down, 0.025f);
    }

    void FixedUpdate()
    {
        if (horizontalInput < 0f || horizontalInput > 0f)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            playerSprite.flipX = horizontalInput < 0f;
            if (isSwinging)
            {
                animator.SetBool("IsSwinging", true);

       
                var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

               
                Vector2 perpendicularDirection;
                if (horizontalInput < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                }
                else
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * swingForce;
                rBody.AddForce(force, ForceMode2D.Force);
            }
            else
            {
                animator.SetBool("IsSwinging", false);
                if (groundCheck)
                {
                    var groundForce = speed * 2f;
                    rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0));
                    rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
                }
            }
        }
        else
        {
            animator.SetBool("IsSwinging", false);
            animator.SetFloat("Speed", 0f);
        }

        if (!isSwinging)
        {
            if (!groundCheck) return;

            isJumping = jumpInput > 0f;
            if (isJumping)
            {
                jumpSoundEffect.Play();
                rBody.velocity = new Vector2(rBody.velocity.x, jumpSpeed);
            }
        }
        else
        {
            if (jumpInput > 0f)
            {
                Debug.Log("Here");
                rBody.velocity = new Vector2(rBody.velocity.x, 1f);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {

         if (col.gameObject.tag == "DeathZone")
        {
            Pause_Data.EndGame = true;
            Pause_Data.PauseMenu = true;
            panelGameOver.SetActive(true);
        }
        else if (col.gameObject.tag == "Life")
        {

            Debug.Log("Hello");
            if (gunSystem_Data.lifePlayer == 7)
            {
                Debug.Log("You are full life.");
            }
            else
            {
                gunSystem_Data.lifeCase.Add(gunSystem_Data.lifeCaseAll[gunSystem_Data.lifePlayer]);
                gunSystem_Data.lifeCaseAll[gunSystem_Data.lifePlayer].SetActive(true);
                gunSystem_Data.lifePlayer += 1;
                gunSystem_Data.lifeProps -= 1;
                col.gameObject.SetActive(false);
            }
           
        }


    }

}