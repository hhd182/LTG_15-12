using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private bool checkSuper = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();
            if (playerScript.isSupper)
            {
                checkSuper = true;
            } else
            {
                other.GetComponent<Player>().HandleSpeed();
            }
            Debug.Log("This trap!!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (checkSuper == false)
            {
                other.GetComponent<Player>().ResetSpeed();
            } else
            {
                checkSuper = false;
            }

        }
    }
}
