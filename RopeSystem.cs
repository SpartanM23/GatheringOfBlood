using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RopeSystem : MonoBehaviour {
    // 1
    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    public CharacterController playerMovement;
    private bool ropeAttached;
    private Vector2 playerPosition;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;
    private ParticleSystem ropeHingeAnchorParticle;

    public Sprite crosshairForGun;
    public Sprite crosshairForHook;
    public bool activateHook = false;
    public GunSystem activateOrNotGun;

    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    private float ropeMaxCastDistance = 8f;
    private List<Vector2> ropePositions = new List<Vector2>();

    private bool distanceSet;

    private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();

    public float climbSpeed = 3f;
    private bool isColliding;

    public Image Activable;
    public Sprite Grapplin;
    public Sprite Shot;
    public Image E_Image;
    public Blink blinkData;

    public AudioSource grapplinSoundEffect;

    public Pause pause_Data_RopeSystem;

    void Awake()
    {
   
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
        ropeHingeAnchorParticle = ropeHingeAnchor.GetComponent<ParticleSystem>();
        activateOrNotGun = this.GetComponent<GunSystem>();
        blinkData = GameObject.Find("E").GetComponent<Blink>();
    }

    void Update()
    {
    
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

      
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        
        playerPosition = transform.position;

        if (!ropeAttached)
        {
            playerMovement.isSwinging = false;
            if (pause_Data_RopeSystem.PauseMenu != true)
            {
                SetCrosshairPosition(aimAngle, aimDirection);
            }
        }

        else
        {
            playerMovement.isSwinging = true;
            playerMovement.ropeHook = ropePositions.Last();

            crosshairSprite.enabled = false;
        }

        if (pause_Data_RopeSystem.PauseMenu != true) {

            UpdateRopePositions();
            HandleInput(aimDirection);
            HandleRopeLength();
        }
    }

    private void SetCrosshairPosition(float aimAngle, Vector2 aimDirection)
    {
       
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }

        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        Ray ray = new Ray(this.transform.position, aimDirection);

        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction, 8f, layerMask);//Shouldn't the 20.5f work for the lenght of the raycast??
        Debug.DrawRay(ray.origin, ray.direction * 8f, Color.green);


        var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
        var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

        if (hit2D.collider != null)
        {
            x = hit2D.transform.position.x;
            y = hit2D.transform.position.y;
        }



        

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetKeyDown(KeyCode.E) && ropeAttached!=true)
        {
            if (activateHook == false)
            {
                crosshairSprite.sprite = crosshairForHook;
                activateOrNotGun.activateGun = false;
                activateHook = true;
                blinkData.numberOfBlink = 6;
                blinkData.Invoke("ToggleState" + blinkData.isBlinking, blinkData.startDelay);
                Activable.sprite = Grapplin;
            }
            else
            {
                crosshairSprite.sprite = crosshairForGun;
                activateOrNotGun.activateGun = true;
                activateHook = false;
                blinkData.numberOfBlink = 6;
                blinkData.Invoke("ToggleState" + blinkData.isBlinking, blinkData.startDelay);
                Activable.sprite = Shot;
            }
            
        }

        if (Input.GetMouseButton(0) && activateHook!=false)
        {
   
            if (ropeAttached) return;
            ropeRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

       
            if (hit.collider != null)
            {
                ropeAttached = true;
                if (!ropePositions.Contains(hit.point))
                {
                    grapplinSoundEffect.Play();
                    
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
                    ropePositions.Add(hit.point);
                    ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    ropeJoint.enabled = true;
                    ropeHingeAnchorSprite.enabled = true;
                    var emission = ropeHingeAnchorParticle.emission;
                    emission.enabled = true;
                }
            }
           
            else
            {
                ropeRenderer.enabled = false;
                ropeAttached = false;
                ropeJoint.enabled = false;
            }
        }

        if (Input.GetMouseButton(1))
        {
            ResetRope();
        }
    }

  
    private void ResetRope()
    {
        ropeJoint.enabled = false;
        ropeAttached = false;
        playerMovement.isSwinging = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
        var emission = ropeHingeAnchorParticle.emission;
        emission.enabled = false;

        wrapPointsLookup.Clear();

    }

    private void UpdateRopePositions()
    {
    
        if (!ropeAttached)
        {
            return;
        }

        ropeRenderer.positionCount = ropePositions.Count + 1;

        for (var i = ropeRenderer.positionCount - 1; i >= 0; i--)
        {
            if (i != ropeRenderer.positionCount - 1) 
            {
                ropeRenderer.SetPosition(i, ropePositions[i]);

             
                if (i == ropePositions.Count - 1 || ropePositions.Count == 1)
                {
                    var ropePosition = ropePositions[ropePositions.Count - 1];
                    if (ropePositions.Count == 1)
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                    else
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                }
              
                else if (i - 1 == ropePositions.IndexOf(ropePositions.Last()))
                {
                    var ropePosition = ropePositions.Last();
                    ropeHingeAnchorRb.transform.position = ropePosition;
                    if (!distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        distanceSet = true;
                    }
                }
            }
            else
            {
              
                ropeRenderer.SetPosition(i, transform.position);
            }
        }
    }



  
    private Vector2 GetClosestColliderPointFromRaycastHit(RaycastHit2D hit, PolygonCollider2D polyCollider)
    {
    
        var distanceDictionary = polyCollider.points.ToDictionary<Vector2, float, Vector2>(
            position => Vector2.Distance(hit.point, polyCollider.transform.TransformPoint(position)),
            position => polyCollider.transform.TransformPoint(position));

        
        var orderedDictionary = distanceDictionary.OrderBy(e => e.Key);
        return orderedDictionary.Any() ? orderedDictionary.First().Value : Vector2.zero;
    }


   
    private void HandleRopeLength()
    {
      
        if (Input.GetAxis("Vertical"+GameObject.Find("ScriptContainer").GetComponent<InputSelected>().langage) >= 1f && ropeAttached && !isColliding)
        {
            ropeJoint.distance -= Time.deltaTime * climbSpeed;
        }
        else if (Input.GetAxis("Vertical" + GameObject.Find("ScriptContainer").GetComponent<InputSelected>().langage) < 0f && ropeAttached)
        {
            ropeJoint.distance += Time.deltaTime * climbSpeed;
        }
    }

    void OnTriggerStay2D(Collider2D colliderStay)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D colliderOnExit)
    {
        isColliding = false;
    }

}
