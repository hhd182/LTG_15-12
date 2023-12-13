using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float normalSpeed = 2f;
    public float boostedSpeed = 4f;

    private float fitness = 100f;   // Thể lực ban đầu
    public float fitnessDecreaseRate = 5f;  // Giảm thể lực mỗi giây khi giữ phím Shift
    public float fitnessThreshold = 10f;    // Ngưỡng thể lực, dưới ngưỡng này không thể sử dụng tốc độ tăng
    public float fitnessRecover = 2;    // Hồi thể lực trên giây

    private float flashlight = 100f; // Đèn pin
    public float flashlightDecreaseRate = 5f; // Giảm pin mỗi giây
    private bool isFlashlight = false; // Check active

    void Start()
    {

    }

    void Update()
    {
        PlayerMove();
        PlayerFitness();
        Flashlight();
    }

    public void PlayerMove()
    {

        float currentSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && fitness > fitnessThreshold)
        {
            currentSpeed = boostedSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0).normalized;
        transform.Translate(movement * currentSpeed * Time.deltaTime);

    }

    public void PlayerFitness()
    {
        if (Input.GetKey(KeyCode.LeftShift) && fitness > fitnessThreshold && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            fitness -= fitnessDecreaseRate * Time.deltaTime;

            if (fitness < 0)
            {
                fitness = 0;
            }
        }
        
        if(!Input.GetKey(KeyCode.LeftShift))
        {
            fitness += fitnessRecover * Time.deltaTime;

            if (fitness > 100)
            {
                fitness = 100;
            }
        }
    }

    public void RecoverFitness (int amount)
    {
        fitness += amount;
        if (fitness > 100)
        {
            fitness = 100;
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

    public void RecoverFlashlight (int amount)
    {
        flashlight += amount;
        if (flashlight > 100)
        {
            flashlight = 100;
        }
        Debug.Log("Player flashlight!" + flashlight);
    }
}
