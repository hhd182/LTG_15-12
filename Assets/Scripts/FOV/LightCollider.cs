using System;
using System.Collections.Generic;
using UnityEngine;

public class LightCollider : MonoBehaviour {
    public BoxCollider2D boxCollider2D;
    public SpriteMask mask;
    private List<Enemy> enemiesInLight = new List<Enemy>();
    public LayerMask layerMask;
    private bool hasWall = false;

    void Start() {
        boxCollider2D.isTrigger = true;
    }

    private void Update() {
        foreach (var enemyInLight in enemiesInLight) {
            // Thực hiện kiểm tra liên tục bằng cách vẽ raycast
            Vector2 direction = (enemyInLight.transform.position - transform.parent.position).normalized;
            float dynamicDistance = Vector2.Distance(transform.parent.position, enemyInLight.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.parent.position, direction, dynamicDistance, layerMask);
            //Debug.DrawRay(transform.parent.position, direction * dynamicDistance, Color.white, 2);
            if (hit.collider != null) {
                enemyInLight.isSpotted = false;
            }
            else {
                enemyInLight.isSpotted = true;
                print(enemyInLight.transform.name + " has spotted");
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            Enemy enemyInLight = other.gameObject.GetComponent<Enemy>();
            if (enemyInLight != null && !enemiesInLight.Contains(enemyInLight)) {
                enemiesInLight.Add(enemyInLight);
                enemyInLight.EnterLight();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            Enemy enemyInLight = other.gameObject.GetComponent<Enemy>();
            if (enemyInLight != null && enemiesInLight.Contains(enemyInLight)) {
                enemiesInLight.Remove(enemyInLight);
                enemyInLight.ExitLight();
            }
        }
    }
}
