using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour, ISavable
{
    private NavMeshAgent agent;

    public Light2D globalLight;

    public int health;
    public bool defeated = false;

    public Transform[] patrolPoints;
    private int currentPoint;

    public Transform target;
    public bool inRange;

    public float detectRadius;
    public float attackTimer;
    public float maxTimer;

    private float timeInLight = 0f;
    public float maxTimeInLight;
    public bool isInLight = false;
    public bool isSpotted = false;

    //Sound
    public AudioSource sfxSound;
    public Sound[] listSound;

    //Flash
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration;
    // The SpriteRenderer that should flash.
    public SpriteRenderer spriteRenderer;
    // The material that was in use, when the script started.
    private Material originalMaterial;

    // The currently running coroutine.
    private Coroutine flashRoutine;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        originalMaterial = spriteRenderer.material;
        sfxSound.volume = AudioManager.Instance.GetSFXVolume();
        sfxSound.Play();
    }

    private void Update()
    {
        if (gameObject == null)
        {
            return;
        }

        sfxSound.volume = AudioManager.Instance.GetSFXVolume();

        if (globalLight.intensity == 1.0f) {
            //Debug.Log("1");
            StartCoroutine(EnableDisableSpriteRenderer());
        }

        //Debug.Log("isInLight: " + isInLight + ", isSpotted: " + isSpotted);


        if (isInLight && isSpotted)
        {
            //Debug.Log("2");

            //Debug.Log(spriteRenderer.enabled);

            timeInLight += Time.deltaTime;
            if (timeInLight >= maxTimeInLight)
            {
                SubtractHealth(1);

                timeInLight = 0f;
            }
        }
        else
        {

            //spriteRenderer.enabled = false;
            //Debug.Log(spriteRenderer.enabled);
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
        }
    }

    public void EnterLight()
    {
        isInLight = true;
    }

    public void ExitLight()
    {
        isInLight = false;
        timeInLight = 0f;
    }

    private void SubtractHealth(int damage)
    {
        health -= damage;
        this.PlaySFX("Hurt");
        this.Flash();
        if (health <= 0)
        {
            defeated = true;
            StartCoroutine("DestroyGameObject");
        }
    }

    public object CaptureState()
    {
        return defeated;
    }

    public void RestoreState(object state)
    {
        defeated = (bool)state;

        if (defeated)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(string name) {
        Sound s = Array.Find(listSound, x => x.audioName == name);

        if (s == null) {
            Debug.Log("SFX not found");
        }
        else {
            sfxSound.PlayOneShot(s.clip);
        }
    }

    public void Flash() {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null) {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }

        // Start the Coroutine, and store the reference for it.
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine() {
        // Swap to the flashMaterial.
        spriteRenderer.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);

        // After the pause, swap back to the original material.
        spriteRenderer.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }

    private IEnumerator EnableDisableSpriteRenderer() {

        spriteRenderer.sortingLayerName = "Player";
    
        yield return new WaitForSeconds(1f);

        spriteRenderer.sortingLayerName = "Enemies";

    }

    private IEnumerator DestroyGameObject() {
        this.PlaySFX("Disable");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }


}
