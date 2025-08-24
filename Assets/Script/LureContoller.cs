using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class LureController : MonoBehaviour
{
    public float maxSpeed = 10f; // 最高速度
    public float speed = 0f;
    public float acceleration = 5f; // 左右交互入力時の加速度
    public float sideAcceleration = 2f; // 左右交互入力時の加速度
    public float deceleration = 1f; // 減速量
    public float sideMoveSpeed = 5f; // 片側連打時の横移動速度
    public float stunDuration = 1f; // スタン時間

    private Rigidbody2D rb;
    private float currentSpeed = 0f;
    private float stunTimer = 0f;

    public GameObject sprite;

    public InputActionAsset InputActions;

    private InputAction _flopLeft;
    private InputAction _flopRight;
    private bool inputChanged = false;
    private bool lastInputWasLeft = false;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    void Awake()
    {
        _flopLeft = InputSystem.actions.FindAction("FlopLeft");
        _flopRight = InputSystem.actions.FindAction("FlopRight");
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
    }

    void Update()
    {
        // スタン状態のチェック
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            // スタン中は速度を落とす
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * 5f);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -currentSpeed);
            return; // スタン中は操作を受け付けない
        }

        if (_flopLeft.WasPressedThisFrame())
        {
            if (inputChanged)
            {
                if (lastInputWasLeft)
                {
                    if (sprite.transform.rotation.eulerAngles.z < 16.0f)
                        sprite.transform.rotation = Quaternion.Euler(0, 0, 45);
                    else
                        sprite.transform.rotation = Quaternion.Euler(0, 0, 15);
                    SideAccelerate(-1);
                }
                else
                {
                    sprite.transform.rotation = Quaternion.Euler(0, 0, 15);
                    Accelerate();
                }
            }
            else
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 15);
                inputChanged = true;
            }
            lastInputWasLeft = true;
        }

        if (_flopRight.WasPressedThisFrame())
        {
            if (inputChanged)
            {
                if (!lastInputWasLeft)
                {
                    if (sprite.transform.rotation.eulerAngles.z > 344.0f)
                        sprite.transform.rotation = Quaternion.Euler(0, 0, 315);
                    else
                        sprite.transform.rotation = Quaternion.Euler(0, 0, 345);
                    SideAccelerate(1);
                }
                else
                {
                    sprite.transform.rotation = Quaternion.Euler(0, 0, 345);
                    Accelerate();
                }
            }
            else
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 345);
                inputChanged = true;
            }
            lastInputWasLeft = false;
        }
    }

    void FixedUpdate()
    {
        if (!_flopLeft.WasPressedThisFrame() && !_flopRight.WasPressedThisFrame() && rb.linearVelocityY < 0f)
        {
            rb.linearVelocityY += .5f;
            if (rb.linearVelocityX > 0f)
            {
                rb.linearVelocityX -= .1f;
            }
            else if (rb.linearVelocityX < 0f)
            {
                rb.linearVelocityX += .1f;
            }
            else if (rb.linearVelocityX > -0.05f && rb.linearVelocityX < -0.05f)
            {
                rb.linearVelocityX = 0f;
            }
        }
    }

    private void Accelerate()
    {
        rb.linearVelocityY -= acceleration;
        rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -maxSpeed, 0);
    }
    private void SideAccelerate(int dir)
    {
        rb.linearVelocityX += dir * sideAcceleration;
        rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -maxSpeed, maxSpeed);
    }

    // 障害物との衝突
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            stunTimer = stunDuration;
        }
    }
}