using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float speed;
    public float detectRange = 5f; // Khoảng cách
    public float disLimit = 1f;

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

    public void EnmyMove()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectRange)
        {
            isFollowPlayer = true;
            FollowPlayer();
        }
        else
        {
            if (isFollowPlayer)
            {
                ReturnToOriginalPosition();
            }
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 target = player.position - direction * disLimit;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void ReturnToOriginalPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);

        float distanceToOriginal = Vector3.Distance(transform.position, originalPosition);
        if (distanceToOriginal < 0.1f)
        {
            transform.position = originalPosition;
            isFollowPlayer = false;
        }
    }

}
