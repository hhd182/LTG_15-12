using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed1 = 2f;
    public float speed2 = 3f;
    public float speed3 = 4f;
    float currentSpeed;
    private bool isSpeed = false;



    private float fitness = 20f;   // Thể lực ban đầu
    public float fitnessDecreaseRate = 1f;  // Giảm thể lực mỗi giây khi giữ phím Shift
    public float fitnessRecover = 1f;    // Hồi thể lực trên giây

    private float flashlight = 100f; // Đèn pin
    public float flashlightDecreaseRate = 5f; // Giảm pin mỗi giây
    private bool isFlashlight = false; // Check active

    void Start()
    {
        currentSpeed = speed1;
    }

    void Update()
    {
        PlayerMove();
        PlayerFitness();
        Flashlight();
    }

    public void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSpeed && fitness > 0)
        {
            isSpeed = false;
            setSpeed();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSpeed = false;
            currentSpeed = speed1;
        }

        if (fitness <= 0)
        {
            currentSpeed = speed1;
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
            if (fitness >= 15)
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
        }

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            fitness += fitnessRecover * Time.deltaTime;

            if (fitness > 20)
            {
                fitness = 20;
            }
        }
    }

    public void RecoverFitness(int amount)
    {
        fitness += amount;
        if (fitness > 20)
        {
            fitness = 20;
        }
        Debug.Log("Player fitness!" + fitness);

    }

    public void Flashlight()
    {
        if (Input.GetMouseButtonDown(1) && flashlight > 0)
        {
            isFlashlight = !isFlashlight;
        }

        if (isFlashlight)
        {
            flashlight -= flashlightDecreaseRate * Time.deltaTime;

            if (flashlight < 0)
            {
                flashlight = 0;
            }
        }
    }

    public void RecoverFlashlight(int amount)
    {
        flashlight += amount;
        if (flashlight > 100)
        {
            flashlight = 100;
        }
        Debug.Log("Player flashlight!" + flashlight);
    }
}
