using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, ISavable
{
    public bool saved = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!saved)
        {
            saved = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            SaveSystem.Instance.Save("saveSlot");
        }
    }

    public object CaptureState()
    {
        return saved;
    }

    public void RestoreState(object state)
    {
        saved = (bool)state;

        if (saved)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
