using UnityEngine;

public class StaminaItem : MonoBehaviour
{
    public int staminaAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().RecoverStamina(staminaAmount);
            Destroy(gameObject);
            Debug.Log("This item!!");
        }
    }
}
