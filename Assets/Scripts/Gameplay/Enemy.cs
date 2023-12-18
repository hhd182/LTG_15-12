using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

    public int health;

    public Transform[] patrolPoints;
    private int currentPoint;

    public Transform target;
    public bool inRange;

    public float detectRadius;
    public float attackTimer;
    public float maxTimer;

    private float timeInLight = 0f;
    public bool isInLight = false;
    public bool isSpotted = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (gameObject == null)
        {
            return;
        }

        if (isInLight && isSpotted)
        {
            spriteRenderer.enabled = true;

            timeInLight += Time.deltaTime;
            if (timeInLight >= 3f)
            {
                SubtractHealth(1);

                timeInLight = 0f;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= detectRadius)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer > maxTimer)
            {
                inRange = true;

                Vector2 movement = Vector2.MoveTowards(transform.position, target.position, 100f);
                agent.destination = movement;
            }
        }
        else if (distance > detectRadius)
        {
            inRange = false;

            BackToPatrol();
        }
    }

    private void BackToPatrol()
    {
        if (!inRange && agent.remainingDistance < 0.5f)
        {
            agent.destination = patrolPoints[currentPoint].position;

            UpdateCurrentPoint();
        }
    }

    private void UpdateCurrentPoint()
    {
        if (currentPoint == patrolPoints.Length - 1)
        {
            currentPoint = 0;
        }
        else
        {
            currentPoint++;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage();
            Debug.Log("DMM player =)))");
        }
    }

    public void EnterLight()
    {
        isInLight = true;
    }

    public void ExitLight()
    {
        isInLight = false;
        timeInLight = 0f; // đặt lại thời gian
    }

    private void SubtractHealth(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
