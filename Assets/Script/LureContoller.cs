using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class LureController : MonoBehaviour
{
    public float maxSpeed = 10f; // �ō����x
    public float speed = 0f;
    public float acceleration = 5f; // ���E���ݓ��͎��̉����x
    public float sideAcceleration = 2f; // ���E���ݓ��͎��̉����x
    public float deceleration = 1f; // ������
    public float sideMoveSpeed = 5f; // �Б��A�Ŏ��̉��ړ����x
    public float stunDuration = 1f; // �X�^������

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
        // �X�^����Ԃ̃`�F�b�N
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            // �X�^�����͑��x�𗎂Ƃ�
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * 5f);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -currentSpeed);
            return; // �X�^�����͑�����󂯕t���Ȃ�
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

    // ��Q���Ƃ̏Փ�
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            stunTimer = stunDuration;
        }
    }
}