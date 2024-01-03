using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private bool checkSuper = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player playerScript = collision.GetComponent<Player>();
            if (playerScript.isSuper)
            {
                checkSuper = true;
            } else
            {
                collision.GetComponent<Player>().HandleSpeed();
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (checkSuper == false)
            {
                collision.GetComponent<Player>().ResetSpeed();
            } else
            {
                checkSuper = false;
            }

        }
    }
}
