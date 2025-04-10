using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] GameControl gameControl;
    [SerializeField] private int speed;
    [SerializeField] private int health;
    [SerializeField] private int score;
    [SerializeField] private int verticalForce;
    [SerializeField] private Transform origin;
    [SerializeField] private Color[] playerColors = new Color[3];
    [SerializeField] private int extraJumps;
    [SerializeField] private LayerMask raycastlayers;

    private int colorIndex = 0;

    private Rigidbody2D myRigidbody2D;
    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer obstacleSpriteRenderer;   
    
    public SpriteRenderer PlayerSpriteRenderer => mySpriteRenderer;
    public int Health => health;
    public int Score => score;

    private float xDirection;

    private bool canJump;
    private int maxJumps = 1;    

    #region Events
    public event Action OnHealthChanged;
    public event Action OnScoreChanged;
    #endregion

    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        extraJumps = maxJumps;

        playerColors[0] = Color.red;
        playerColors[1] = Color.blue;
        playerColors[2] = Color.green;

    }
    void Update()
    {
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
    #region Mecanica de cambio de color
    public void ChangeColor()
    {
        mySpriteRenderer.color = playerColors[colorIndex];
    }
    public void RightColorChange()
    {
        colorIndex = (colorIndex + 1) % playerColors.Length;
        ChangeColor();
    }
    public void LeftColorChange()
    {
        colorIndex = (colorIndex - 1 + playerColors.Length) % playerColors.Length;
        ChangeColor();
    }
    #endregion
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

        if (mySpriteRenderer.color != obstacleSpriteRenderer.color && collision.gameObject.CompareTag("Color"))
        {
            health--;
            OnHealthChanged?.Invoke();
        }

        if (collision.gameObject.CompareTag("Heart"))
        {
            health = Mathf.Clamp(health + 1, 0, 10);
            Destroy(collision.gameObject);
            OnHealthChanged?.Invoke();
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            score = score + 100;
            Destroy(collision.gameObject);
            OnScoreChanged?.Invoke();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        gameControl.canChangeColor = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        gameControl.canChangeColor = true;
    }
    #region NewInputSystem
    public void OnJumpInput (InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && canJump && !gameControl.IsPaused)
        {
            myRigidbody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
            extraJumps--;
        }
    }
    public void OnMovementInput (InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && !gameControl.IsPaused)
        {
            xDirection = callbackContext.ReadValue<float>();
        }
        else
        {
            xDirection = 0;
        }
    }
    public void OnColorChangeRight(InputAction.CallbackContext context)
    {
        if (context.performed && !gameControl.IsPaused)
        {
            RightColorChange();
        }
    }
    public void OnColorChangeLeft(InputAction.CallbackContext context)
    {
        if (context.performed && !gameControl.IsPaused)
        {
            LeftColorChange();
        }
    }
    #endregion
}
