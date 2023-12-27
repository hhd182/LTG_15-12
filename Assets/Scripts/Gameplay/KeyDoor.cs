using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour, ISavable
{
    public bool used = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            used = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AudioSource>().Play();

            GameObject door = GameObject.Find("Door");
            door.GetComponent<SpriteRenderer>().enabled = false;
            door.GetComponent<BoxCollider2D>().enabled = false;
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

            GameObject door = GameObject.Find("Door");
            door.GetComponent<SpriteRenderer>().enabled = false;
            door.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
