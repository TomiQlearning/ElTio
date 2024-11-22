using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float walkSpeed = 3.5f;
    public float runSpeed = 7f;
    public float teleportDistance = 10f;
    public float detectionRange = 15f;
    public float chaseRange = 20f;
    public Light playerFlashlight;
    public Transform[] patrolPoints;
    private Transform player;
    private NavMeshAgent agent;
    private int currentPatrolIndex;
    private bool isChasing = false;
    private bool isTeleporting = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        currentPatrolIndex = 0;
        MoveToNextPatrolPoint();
    }

    void Update()
    {
        if (isTeleporting) return;

        if (Vector3.Distance(transform.position, player.position) <= detectionRange && playerFlashlight.enabled)
        {
            // Detección de linterna
            isChasing = true;
            agent.speed = runSpeed;
        }
        else if (Vector3.Distance(transform.position, player.position) > chaseRange)
        {
            // Dejar de perseguir si el jugador se aleja lo suficiente
            isChasing = false;
            agent.speed = walkSpeed;
            MoveToNextPatrolPoint();
        }

        if (isChasing)
        {
            // Perseguir al jugador
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) <= teleportDistance)
            {
                StartCoroutine(TeleportNearPlayer());
            }
        }
        else
        {
            // Patrullar
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                MoveToNextPatrolPoint();
            }
        }
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    IEnumerator TeleportNearPlayer()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(1f); // Tiempo de espera antes del teletransporte
        Vector3 teleportPosition = player.position + (Random.insideUnitSphere * 2f);
        teleportPosition.y = transform.position.y; // Mantener la misma altura
        agent.Warp(teleportPosition);
        isTeleporting = false;
    }
}
