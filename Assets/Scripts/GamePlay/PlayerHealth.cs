using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 6f;
    private float currentHealth;

    public float invincibilityTime = 5f; // Thời gian bất tử sau khi nhận sát thương
    private bool isInvincible; // Trạng thái bất tử

    private void Start()
    {
        currentHealth = 3;
        isInvincible=false;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Current Health: " + currentHealth);
    }

    public void EnmyDame()
    {
        if (!isInvincible)
        {
            currentHealth--;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            Debug.Log("Current Health: " + currentHealth);
            Debug.Log("Anh dang bat tu ehe");
            isInvincible = true;
            Invoke("DisableInvincibility", invincibilityTime);
        }
    }

    // Hàm để hủy trạng thái bất tử
    private void DisableInvincibility()
    {
        isInvincible = false;
        Debug.Log("Het bat tu");
    }
}
