using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject heartBar;

    public int maxHealth = 5;
    [SerializeField] private int currentHealth = 3;

    public float invincibilityTime = 5f; // Thời gian bất tử sau khi nhận sát thương
    private bool isInvincible; // Trạng thái bất tử

    private void Start()
    {
        heartBar.GetComponent<Health>().SetMaxHealth(maxHealth);
        heartBar.GetComponent<Health>().SetHealth(currentHealth);
        isInvincible =false;
    }

    public void Healing(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        heartBar.GetComponent<Health>().SetHealth(currentHealth);
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
            heartBar.GetComponent<Health>().SetHealth(currentHealth);
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
