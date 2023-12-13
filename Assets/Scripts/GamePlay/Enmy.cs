using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float speed;
    public float detectRange = 5f; // Khoảng cách
    public float maxReturnDistance = 5f;


    private Vector3 originalPosition;
    private bool isFollowPlayer = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        EnmyMove();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().EnmyDame();
            Debug.Log("DMM player =)))");
        }
    }


    public void EnmyMove()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToOriginal = Vector3.Distance(transform.position, originalPosition);

        if (distanceToPlayer <= detectRange && distanceToOriginal <= maxReturnDistance)
        {
            isFollowPlayer = true;
            FollowPlayer();
        }
        else
        {
            if (isFollowPlayer)
            {
                ReturnPosition();
            }
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 target = player.position - direction * Mathf.Epsilon;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }



    void ReturnPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);

        float distanceToOriginal = Vector3.Distance(transform.position, originalPosition);
        if (distanceToOriginal < Mathf.Epsilon)
        {
            transform.position = originalPosition;
            isFollowPlayer = false;
        }
    }

}
