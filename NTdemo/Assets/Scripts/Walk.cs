using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour
{
    private bool isDashing;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCooldown;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;



    Animator anim;
    private float moveForce = 3f;
    [SerializeField] private LayerMask platformsLayerMask;
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        Movement();

        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", move);

        CheckDash();
    }

    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveForce = 20f;
        }

        if (Input.GetKey(KeyCode.D))
        {

            transform.Translate(Vector2.right * moveForce * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {

            transform.Translate(-Vector2.right * moveForce * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
        }

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            float jumpVelocity = 10f;
            rigidbody2d.velocity = Vector2.up * jumpVelocity;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (rigidbody2d.velocity.y > 0)
            {
                float jumpVelocity = 0f;
                rigidbody2d.velocity = Vector2.down * jumpVelocity;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            moveForce = 3f;
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCooldown))
                AttemptToDash();
        }

    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    private void CheckDash()
    {
        if(isDashing)
        {
            if(dashTimeLeft > 0)
            {
                Translate = false;
                canFlip = false;
                RenderBuffer.velocity = new Vector2(dashSpeed * facingDirection, RenderBuffer.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetComponent();
                    lastImageXpos = transform.position.x;
                }

            }

            if(dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
            
        }
    }
    

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2D.collider != null;
    }

}