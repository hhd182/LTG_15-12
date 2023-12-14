using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightItem : MonoBehaviour
{
    public int flashlightAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().RecoverFlashlight(flashlightAmount);
            Destroy(gameObject);
            Debug.Log("This item!!");
        }
    }
}
