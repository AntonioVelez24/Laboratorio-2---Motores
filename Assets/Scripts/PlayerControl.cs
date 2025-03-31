using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] GameControl gameControl;
    [SerializeField] private int speed;
    [SerializeField] private int health;
    [SerializeField] private int verticalForce;
    [SerializeField] private Transform origin;

    private Rigidbody2D myRigidbody2D;
    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer obstacleSpriteRenderer;   
    
    public int Health => health;

    private float xDirection;

    private bool canJump;
    private int maxJumps = 1;

    [SerializeField] private int extraJumps;
    [SerializeField] private LayerMask raycastlayers;

    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        extraJumps = maxJumps;
    }
    void Update()
    {
        xDirection = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            myRigidbody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
            extraJumps--;
        }
        if (xDirection < 0)
        {
            mySpriteRenderer.flipX = true;
        }
        else if (0 < xDirection)
        {
            mySpriteRenderer.flipX = false;
        }
        if (extraJumps == 0)
        {
            canJump = false;
        }
        float clampedX = Mathf.Clamp(transform.localPosition.x, -8.5f, 8.5f);

        transform.localPosition = new Vector2(clampedX, transform.localPosition.y);
    }
    private void FixedUpdate()
    {
        myRigidbody2D.linearVelocity = new Vector2(speed * xDirection, myRigidbody2D.linearVelocity.y);

        RaycastHit2D hit = Physics2D.Raycast(origin.position, Vector2.down, 1f, raycastlayers);

        if(hit.collider != null)
        {
            Debug.DrawRay(origin.position, Vector2.down * 1f, Color.green);
            extraJumps = maxJumps;
            canJump = true;
        }
        else
        {
            Debug.DrawRay(origin.position, Vector2.down * 1f, Color.red);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        obstacleSpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();

        if (mySpriteRenderer.color != obstacleSpriteRenderer.color)
        {
            health--;
            gameControl.UpdateHealthBar();
        }
    }
}
