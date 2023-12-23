using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaItem : MonoBehaviour, ISavable
{
    public int staminaAmount = 10;
    public bool used = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            used = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            collision.GetComponent<Player>().RecoverStamina(staminaAmount);
        }
    }

    public object CaptureState()
    {
        return used;
    }

    public void RestoreState(object state)
    {
        used = (bool)state;

        if (used)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
