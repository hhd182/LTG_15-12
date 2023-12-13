using UnityEngine;

public class Item : MonoBehaviour
{
    public int healAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().Heal(healAmount);
            gameObject.SetActive(false);
            Destroy(gameObject);
            Debug.Log("This item!!");
        }
    }
}
