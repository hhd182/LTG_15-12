using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public int healthAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().RecoverHealth(healthAmount);
            Destroy(gameObject);
            Debug.Log("This item!!");
        }
    }
}
