using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  private Rigidbody2D rb;
  private bool CanJump = true;
  private Animator playerAnimation;
  [SerializeField] private float MovementSpeed;
  [SerializeField] private float JumpSpeed;
  [SerializeField] private float RayLength;
  [SerializeField] private float RayPositionOffset;

  Vector3 RayPositionCenter;
  Vector3 RayPositionMidLeft;
  Vector3 RayPositionLeft;
  Vector3 RayPositionRight;

  RaycastHit2D[] GroundHitsCenter;
  RaycastHit2D[] GroundHitsMidLeft;
  RaycastHit2D[] GroundHitsLeft;
  RaycastHit2D[] GroundHitsRight;

  RaycastHit2D[][] AllRaycastHits = new RaycastHit2D[4][];


  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    playerAnimation = GetComponent<Animator>();
  }

  private void Update()
  {
    Movement();
    Jump();
  }

  private void Jump()
  {
    RayPositionCenter = transform.position + new Vector3(.5f, RayLength * .3f, 0);
    RayPositionMidLeft = transform.position + new Vector3(-.5f, RayLength * .3f, 0);
    RayPositionLeft = transform.position + new Vector3(-RayPositionOffset, RayLength * .3f, 0);
    RayPositionRight = transform.position + new Vector3(RayPositionOffset, RayLength * .3f, 0);

    GroundHitsCenter = Physics2D.RaycastAll(RayPositionCenter, -Vector2.up, RayLength);
    GroundHitsMidLeft = Physics2D.RaycastAll(RayPositionMidLeft, -Vector2.up, RayLength);
    GroundHitsLeft = Physics2D.RaycastAll(RayPositionCenter, -Vector2.up, RayLength);
    GroundHitsRight = Physics2D.RaycastAll(RayPositionCenter, -Vector2.up, RayLength);

    AllRaycastHits[0] = GroundHitsCenter;
    AllRaycastHits[1] = GroundHitsMidLeft;
    AllRaycastHits[2] = GroundHitsLeft;
    AllRaycastHits[3] = GroundHitsRight;

    CanJump = GroundCheck(AllRaycastHits);

    Debug.DrawRay(RayPositionCenter, -Vector2.up * RayLength, Color.red);
    Debug.DrawRay(RayPositionMidLeft, -Vector2.up * RayLength, Color.red);
    Debug.DrawRay(RayPositionLeft, -Vector2.up * RayLength, Color.red);
    Debug.DrawRay(RayPositionRight, -Vector2.up * RayLength, Color.red);


  }

  private bool GroundCheck(RaycastHit2D[][] GroundHits)
  {
    foreach (RaycastHit2D[] HitList in GroundHits)
    {
      foreach (RaycastHit2D hit in HitList)
      {
        if (hit.collider != null)
        {
          if (hit.collider.tag != "PlayerCollider")
          {
            return true;
          }
        }
      }
    }

    return false;
  }
  private void Movement()
  {
    playerAnimation = GetComponent<Animator>();

    if (Input.GetAxisRaw("Horizontal") > 0)
    {
      rb.velocity = new Vector2(MovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
      transform.localScale = new Vector2(1, 1);
    }
    else if (Input.GetAxisRaw("Horizontal") < 0)
    {
      rb.velocity = new Vector2(-MovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
      transform.localScale = new Vector2(-1, 1);

    }
    else
    {
      rb.velocity = new Vector2(0, rb.velocity.y);

    }

    if (Input.GetKey(KeyCode.Space) && CanJump)
    {
      rb.velocity = new Vector2(rb.velocity.x, 0);
      rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
    }
    if (Input.GetKey(KeyCode.UpArrow) && CanJump)
    {
      rb.velocity = new Vector2(rb.velocity.x, 0);
      rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
    }
    playerAnimation.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

  }
}
