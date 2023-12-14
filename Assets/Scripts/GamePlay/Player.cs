using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject staminaBar;
    public GameObject batteryBar;
    public GameObject vision;

    public float speed1 = 2f;
    public float speed2 = 3f;
    public float speed3 = 4f;   
    public float currentSpeed;
    public bool isSpeed = false;

    private float fitness = 20f;   // Thể lực ban đầu
    public float fitnessDecreaseRate = 1f;  // Giảm thể lực mỗi giây khi giữ phím Shift
    public float fitnessRecover = 1f;    // Hồi thể lực trên giây

    private float flashightEnergy = 100f; // Đèn pin
    public float flashlightDecreaseRate = 5f; // Giảm pin mỗi giây
    private bool isFlashlight = false; // Check active
    public bool isSupper = false;

    private Vector3 mouseWorldPosition;

    void Start()
    {
        staminaBar.GetComponent<StaminaBar>().SetMaxStamina(fitness);
        batteryBar.GetComponent<BatteryBar>().SetMaxEnergy(flashightEnergy);
        isFlashlight = false;
    }

    void Update()
    {
        PlayerMove();
        PlayerFitness();
        Flashlight();
        SettingVision();
    }

    public void PlayerMove()
    {
        if (fitness > 0 && currentSpeed != 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                setSpeed();
                isSpeed = true;
            }
        } 
        else
        {
            currentSpeed = speed1;
            if (isSupper)
            {
                isSupper = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = speed1;
            isSpeed = false;
            if (isSupper)
            {
                isSupper = false;
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0).normalized;
        transform.Translate(movement * currentSpeed * Time.deltaTime);

    }

    public void setSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (fitness == 20)
            {
                isSupper = true;
                currentSpeed = speed3;
                Debug.Log("I'm soo supperrr");
            }
            else  if (fitness >= 15)
            {
                currentSpeed = speed3;
                Debug.Log("I'm soo strongg");
            }
            else if (fitness >= 2)
            {
                currentSpeed = speed2;
                Debug.Log("I'm normal");
            }
            else
            {
                currentSpeed = speed1;
                Debug.Log("I can't do it...");
            }
        }
        
    }


    public void PlayerFitness()
    {
        if (Input.GetKey(KeyCode.LeftShift) && fitness > 0 && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            fitness -= fitnessDecreaseRate * Time.deltaTime;
            if (fitness < 0)
            {
                fitness = 0;
            }
            staminaBar.GetComponent<StaminaBar>().SetStamina(fitness);
        }

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            fitness += fitnessRecover * Time.deltaTime;
            if (fitness > 20)
            {
                fitness = 20;
            }
            staminaBar.GetComponent<StaminaBar>().SetStamina(fitness);
        }
    }

    public void RecoverFitness(int amount)
    {
        fitness += amount;
        if (fitness > 20)
        {
            fitness = 20;
        }
        staminaBar.GetComponent<StaminaBar>().SetStamina(fitness);
        Debug.Log("Player fitness!" + fitness);

    }

    public void Flashlight()
    {
        if (Input.GetMouseButtonDown(1) && flashightEnergy > 0)
        {
            isFlashlight = !isFlashlight;
        }

        if (isFlashlight)
        {
            flashightEnergy -= flashlightDecreaseRate * Time.deltaTime;

            if (flashightEnergy < 0)
            {
                flashightEnergy = 0;
                isFlashlight = false;
            }
            batteryBar.GetComponent<BatteryBar>().SetEnergy(flashightEnergy);
        }
    }

    public void RecoverFlashlight(int amount)
    {
        flashightEnergy += amount;
        if (flashightEnergy > 100)
        {
            flashightEnergy = 100;
        }
        batteryBar.GetComponent<BatteryBar>().SetEnergy(flashightEnergy);
        Debug.Log("Player flashlight!" + flashightEnergy);
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

    private void SettingVision() {
        vision.gameObject.SetActive(isFlashlight);
    }

    void RotateCharacter() {
        Vector2 lookDirection = mouseWorldPosition - transform.position;
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
    }
}
