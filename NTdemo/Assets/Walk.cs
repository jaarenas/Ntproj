using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour
{

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
    }

    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveForce = 10f;
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
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2D.collider != null;
    }

}