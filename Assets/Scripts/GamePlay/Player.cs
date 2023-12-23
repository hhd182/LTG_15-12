using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ISavable
{
    private Rigidbody2D rb2d;
    private Animator animator;
    private Vector2 movementInput;

    public GameObject healthBar;
    public GameObject staminaBar;
    public GameObject batteryBar;
    public GameObject flashlight;

    public int maxHealth = 5;
    [SerializeField] int currentHealth = 3;
    public float invincibileTime = 1f;
    private bool isInvincible = false;

    public float speed1 = 2f;
    public float speed2 = 3f;
    public float speed3 = 4f;
    public float currentSpeed;
    public bool isSpeed = false;

    private float stamina = 20f;
    public float staminaDecreaseRate = 1f;
    public float staminaRecoverRate = 1f;

    private float battery = 100f;
    public float batteryDecreaseRate = 5f;

    private bool flashlightOn = false;
    public bool isSuper = false;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        healthBar.GetComponent<HealthBar>().SetMaxHealth(maxHealth);
        healthBar.GetComponent<HealthBar>().SetHealth(currentHealth);
        staminaBar.GetComponent<StaminaBar>().SetMaxStamina(stamina);
        batteryBar.GetComponent<BatteryBar>().SetMaxBattery(battery);

        flashlightOn = false;
    }

    void Update()
    {
        HandleInput();
        Animate();
        HandleStamina();
        TurnOnFlashlight();
        SettingVision();
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + movementInput * currentSpeed * Time.fixedDeltaTime);
    }

    public void HandleInput()
    {
        if (stamina > 0 && currentSpeed != 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SetSpeed();
                isSpeed = true;
            }
        }
        else
        {
            currentSpeed = speed1;
            if (isSuper)
            {
                isSuper = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = speed1;
            isSpeed = false;
            if (isSuper)
            {
                isSuper = false;
            }
        }

        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    private void Animate()
    {
        animator.SetFloat("speed", movementInput.magnitude);
        animator.SetFloat("moveX", movementInput.x);
        animator.SetFloat("moveY", movementInput.y);
    }

    public void SetSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina == 20)
            {
                isSuper = true;
                currentSpeed = speed3;

            }
            else if (stamina >= 15)
            {
                currentSpeed = speed3;

            }
            else if (stamina >= 2)
            {
                currentSpeed = speed2;

            }
            else
            {
                currentSpeed = speed1;

            }
        }
    }

    public void RecoverHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.GetComponent<HealthBar>().SetHealth(currentHealth);

    }

    public void TakeDamage()
    {
        if (!isInvincible)
        {
            currentHealth--;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            healthBar.GetComponent<HealthBar>().SetHealth(currentHealth);
            animator.SetTrigger("Hurt");

            isInvincible = true;
            Invoke("DisableInvincibility", invincibileTime);
        }
    }

    private void DisableInvincibility()
    {
        isInvincible = false;

    }


    public void HandleStamina()
    {
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            stamina -= staminaDecreaseRate * Time.deltaTime;
            if (stamina < 0)
            {
                stamina = 0;
            }
            staminaBar.GetComponent<StaminaBar>().SetStamina(stamina);
        }

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            stamina += staminaRecoverRate * Time.deltaTime;
            if (stamina > 20)
            {
                stamina = 20;
            }
            staminaBar.GetComponent<StaminaBar>().SetStamina(stamina);
        }
    }

    public void RecoverStamina(int amount)
    {
        stamina += amount;
        if (stamina > 20)
        {
            stamina = 20;
        }
        staminaBar.GetComponent<StaminaBar>().SetStamina(stamina);

    }

    public void TurnOnFlashlight()
    {
        if (Input.GetMouseButtonDown(1) && battery > 0)
        {
            flashlightOn = !flashlightOn;
        }

        if (flashlightOn)
        {
            battery -= batteryDecreaseRate * Time.deltaTime;

            if (battery < 0)
            {
                battery = 0;
                flashlightOn = false;
            }
            batteryBar.GetComponent<BatteryBar>().SetEnergy(battery);
        }
    }

    public void RecoverBattery(int amount)
    {
        battery += amount;
        if (battery > 100)
        {
            battery = 100;
        }
        batteryBar.GetComponent<BatteryBar>().SetEnergy(battery);

    }

    public void HandleSpeed()
    {
        currentSpeed *= 0.5f;
        speed1 *= 0.5f;
        speed2 *= 0.5f;
        speed3 *= 0.5f;
    }

    public void ResetSpeed()
    {
        currentSpeed *= 2f;
        speed1 *= 2f;
        speed2 *= 2f;
        speed3 *= 2f;
    }

    private void SettingVision()
    {
        flashlight.gameObject.SetActive(flashlightOn);
    }

    public object CaptureState()
    {
        float[] state = new float[] { transform.position.x, transform.position.y, currentHealth, stamina, battery };
        return state;
    }

    public void RestoreState(object state)
    {
        float[] capturedState = (float[])state;
        transform.position = new Vector3(capturedState[0], capturedState[1]);
        currentHealth = (int)capturedState[2];
        stamina = capturedState[3];
        battery = capturedState[4];
    }
}
