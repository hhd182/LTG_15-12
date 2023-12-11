using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    private Vector2 input;
    private bool isMoving;
    private bool isRunning;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0)
            {
                input.y = 0;
            }

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                Vector3 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (CheckForWalking(targetPos) && !isRunning)
                {
                    StartCoroutine(Walk(targetPos));
                }
                else if (CheckForWalking(targetPos) && isRunning)
                {
                    StartCoroutine(Run(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    private IEnumerator Walk(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, walkSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

/*        CheckForTrigger();*/
    }

    private IEnumerator Run(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, runSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

/*        CheckForTrigger();*/
    }

    private bool CheckForWalking(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, Mathf.Epsilon, GameLayers.Instance.SolidLayer /*| GameLayers.Instance.InteractableLayer*/) != null)
        {
            return false;
        }

        return true;
    }
}