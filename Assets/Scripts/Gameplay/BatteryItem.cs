using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryItem : MonoBehaviour
{
    public int batteryAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().RecoverBattery(batteryAmount);
            Destroy(gameObject);
            Debug.Log("This item!!");
        }
    }
}
