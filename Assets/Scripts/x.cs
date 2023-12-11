using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class x : MonoBehaviour
{
    public float normalSpeed = 2f;
    public float boostedSpeed = 4f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayMove();
    }

    public void PlayMove()
    {
        float currentSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = boostedSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate((-currentSpeed) * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(currentSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, currentSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, (-currentSpeed) * Time.deltaTime, 0);
        }

    }


}
