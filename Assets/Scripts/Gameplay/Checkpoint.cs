using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SaveSystem.Instance.Save("saveSlot");
    }
}
