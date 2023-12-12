using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{
    public float normalSpeed = 2f;
    public float boostedSpeed = 4f;

    private float fitness = 100f;   // Thể lực ban đầu
    public float fitnessDecreaseRate = 5f;  // Giảm thể lực mỗi giây khi giữ phím Shift
    public float fitnessThreshold = 10f;    // Ngưỡng thể lực, dưới ngưỡng này không thể sử dụng tốc độ tăng
    public float fitnessRecover = 2;    // Hồi thể lực trên giây

    void Start()
    {

    }

    void Update()
    {
        PlayerMove();
        PlayerFitness();
    }

    public void PlayerMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float currentSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && fitness > fitnessThreshold)
        {
            currentSpeed = boostedSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
        {
            // Di chuyển theo trục ngang
            transform.Translate(horizontalInput * currentSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            // Di chuyển theo trục dọc
            transform.Translate(0, verticalInput * currentSpeed * Time.deltaTime, 0);
        }
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


}
