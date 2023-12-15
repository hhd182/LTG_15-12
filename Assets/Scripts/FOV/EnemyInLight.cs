using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInLight : MonoBehaviour {
    private float timeInLight = 0f;
    public bool isInLight = false;
    public int health;
    public bool isSpotted = false;
    private SpriteRenderer spriteRender;

    private void Start() {
        spriteRender = GetComponent<SpriteRenderer>();

    }

    void Update() {
        if (gameObject == null) // kiểm tra xem đối tượng có vẫn còn tồn tại hay không
        {
            return; // nếu không, bỏ qua đối tượng này
        }

        if (isInLight && isSpotted) {
            spriteRender.enabled = true;
            timeInLight += Time.deltaTime;
            if (timeInLight >= 3f) {
                SubtractHealth(1); // gọi hàm trừ máu
                timeInLight = 0f; // đặt lại thời gian
            }
        }
        else {
            spriteRender.enabled = false;
        }
    }

    public void EnterLight() {
        isInLight = true;
    }

    public void ExitLight() {
        isInLight = false;
        timeInLight = 0f; // đặt lại thời gian
    }

    private void SubtractHealth(int dame) {
        health -= dame;
        print(gameObject.name + ": " + health);
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
