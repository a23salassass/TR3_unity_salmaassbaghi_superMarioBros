using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.W;

    private Rigidbody2D rb;
    private Collider2D capsuleCollider;
    private Vector2 velocity;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;

    private float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    private float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);
    
    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f;
    public bool sliding =>
        (velocity.x > 0 && Input.GetKey(leftKey)) ||
        (velocity.x < 0 && Input.GetKey(rightKey));
    public bool falling => velocity.y < 0f && !grounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        capsuleCollider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        capsuleCollider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(jumpKey))
{
    Debug.Log("Salto detectado con: " + jumpKey);
}

        grounded = rb.Raycast(Vector2.down);

        float inputAxis = 0f;
        if (Input.GetKey(leftKey)) inputAxis = -1f;
        if (Input.GetKey(rightKey)) inputAxis = 1f;

        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        if (grounded)
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
            jumping = velocity.y > 0f;

if (grounded && Input.GetKey(jumpKey) && !jumping)
{
    velocity.y = jumpForce;
    jumping = true;
}
        }

        ApplyGravity();
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position += velocity * Time.fixedDeltaTime;
        rb.MovePosition(position);
    }

    private void ApplyGravity()
    {
        bool isFalling = velocity.y < 0f || !Input.GetKey(jumpKey);
        float multiplier = isFalling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }
}
