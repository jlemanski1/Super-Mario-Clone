using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private int speed = 5;

    [SerializeField]
    private int jumpSpeed = 5;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private Animator animator;

    public bool facingRight = true;
    private bool isJumping = false;

    private float maxJumpHeight = 0.005f;
    private float jumpButtonPressTime;
    private float maxJumpTime = 0.2f;

    // Player's dimensions
    private float width;
    private float height;

    // Set up components
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        // Get collider dimensions
        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f;
    }


    // Handles physics and therefore player movement
    void FixedUpdate() {
        float xMove = Input.GetAxisRaw("Horizontal");       // Get horizontal input
        Vector2 vect = rigidBody.velocity;

        rigidBody.velocity = new Vector2(xMove * speed, vect.y);    // Set new velocity in horizontal direction
        animator.SetFloat("Speed", Mathf.Abs(xMove));               // Set "Speed" condition of animator

        // Flip to the right
        if (xMove > 0 && !facingRight) {
            FlipSprite();
        }
        // Flip to the left
        else if (xMove < 0 && facingRight) {
            FlipSprite();
        }

        float yMove = Input.GetAxis("Jump");        // Get vertical input
        // Player is able to jump
        if (OnGround() && isJumping == false) {
            // Player is jumping
            if (yMove > 0f) {
                isJumping = true;
            }
        }

        // Cancel vertical momentum for jumping too long
        if (jumpButtonPressTime > maxJumpTime) {
            yMove = 0;
        }


        // Player Jump
        if (isJumping && jumpButtonPressTime < maxJumpTime) {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
        }


        // Player is falling
        if (yMove >= 1f)
            jumpButtonPressTime += Time.deltaTime;
        else {
            isJumping = false;
            jumpButtonPressTime = 0f;
        }

    }


    // Flips the player sprite to face the proper direction
    private void FlipSprite() {
        facingRight = !facingRight; // Switch facingRight
        Vector3 scale = transform.localScale;
        scale.x *= -1;              // Inverse the scale
        transform.localScale = scale;
    }


    // Checks if the ground is directly below the player
    public bool OnGround() {
        bool groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - height), -Vector2.up, maxJumpHeight);

        // Check For walljumping
        bool leftCheck = Physics2D.Raycast(new Vector2(transform.position.x + (width - 0.2f), transform.position.y - height), -Vector2.up, maxJumpHeight);
        bool rightCheck = Physics2D.Raycast(new Vector2(transform.position.x - (width - 0.2f), transform.position.y - height), -Vector2.up, maxJumpHeight);

        // Player is on the ground
        if (groundCheck || leftCheck || rightCheck)
            return true;

        // Player is not on the ground
        return false;
    }


    // Destroy Player after disappearing from screen
    private void OnBecameInvisible() {
        Debug.Log("Player Destroyed");
        Destroy(gameObject);
    }

}
