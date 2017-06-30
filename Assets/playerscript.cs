using UnityEngine;

public class playerscript : MonoBehaviour
{

    public float movespeed = 1f;
    public float jumpspeed = 1f;
    public float jumpheight = -1f;
    private Vector2 move;
    public Vector2 jump;
    private Rigidbody2D rgBD;
    public bool isGrounded;

    private void Start()
    {
        rgBD = GetComponent<Rigidbody2D>();
    }

    //void moveHoriz()
    //{
       
    //}

    void Update()
    {
        //move
        //if (Input.GetButtonDown("Horizontal"))
        //{
        //    move.x = Input.GetAxisRaw("Horizontal") * movespeed;
        //}

        //if (Input.GetButtonUp("Horizontal"))
        //{
        //    move.x = 0;
        //}

        move.x = Input.GetAxisRaw("Horizontal") * movespeed;
        // face direction
        if (Input.GetKey(KeyCode.A))
            gameObject.GetComponent<SpriteRenderer>().flipX = true;

        if (Input.GetKey(KeyCode.D))
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        //jump

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // rgBD.AddForce(new Vector2(0, jumpspeed), ForceMode2D.Force);
            move.y = jumpspeed;
        }

        if (Input.GetButtonUp("Jump") || (gameObject.transform.position.y >= jumpheight))
        {
            move.y = -jumpspeed;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }

    void FixedUpdate()
    {
       rgBD.velocity = move;
    }
}