using UnityEngine;

public class FitnessItem : MonoBehaviour
{
    public int fitnessAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().RecoverFitness(fitnessAmount);
            Destroy(gameObject);
            Debug.Log("This item!!");
        }
    }
}
