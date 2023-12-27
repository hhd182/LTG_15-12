using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChecked : MonoBehaviour
{
    public GameManager gameManager;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            gameManager.Win();
        }
    }
}
