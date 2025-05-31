using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemyNPCBehaviour : MonoBehaviour
{
    public Transform[] patrolPoints;
    [SerializeField] private float viewDistance = 10;
    [SerializeField] private float detectionRate = 1;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Transform player;
    [SerializeField] private float loseTargetTime = 3f;
    [SerializeField] private float waitTimeAtPoint = 2f;
    [SerializeField] private float detectionMin = 10f;
    public bool isStanding = false;
    [SerializeField] private Vector3 rotationPerson;
    [SerializeField] private Animator animator;

    public NavMeshAgent agent;
    private int currentPatrolIndex;
    public float detectionMeter = 0;
    public float maxDetection = 100;
    public float loseTimer = 0;
    private float waitTimer = 0f;
    private Vector3 investigationPosition;
    private bool reachedWhistlePoint = false;

    public enum State { Patrolling, Waiting, Detecting, Investigating }
    public State currentState = State.Patrolling;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = ThirdPersonController.instance.transform;
        GoToNextPoint();
    }
    private void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                PatrolLogic();
                DetectionLose();
                break;

            case State.Waiting:
                WaitLogic();
                DetectionLose();
                break;

            case State.Detecting:
                DetectingLogic();
                break;
            case State.Investigating:
                InvestigateLogic();
                break;
        }

        CheckForPlayer();
        RedVignetteManager.instance.RegisterIntensity(detectionMeter / 100f);

        animator.SetBool("Walk", agent.velocity.magnitude > 0.1f && !agent.isStopped);
    }

    public void GoToNextPoint()
    {
        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
    private void WaitLogic()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTimeAtPoint && !isStanding)
        {
            GoToNextPoint();
            currentState = State.Patrolling;
            agent.isStopped = false;
        }
        if (isStanding)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(rotationPerson), Time.deltaTime * 5f);
        }
    }

    private void PatrolLogic()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Waiting;
            waitTimer = 0f;
            agent.isStopped = true;
        }
    }

    private void DetectionLose()
    {
        if (detectionMeter > 0)
        {
            detectionMeter -= Time.deltaTime * detectionMeter;
        }
        else if (detectionMeter < 0.1f) detectionMeter = 0;
    }

    private void CheckForPlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        float adjustedViewDistance = viewDistance;

        if (angleToPlayer > 135f) 
            adjustedViewDistance *= 0.66f;
        else if (angleToPlayer > 45f)
            adjustedViewDistance *= 0.85f;

        if (distToPlayer <= adjustedViewDistance && !Physics.Linecast(transform.position, player.position, obstacleMask))
        {
            currentState = State.Detecting;
            agent.isStopped = true;
            loseTimer = 0f;
        }
    }

    void DetectingLogic()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(player.position - transform.position), Time.deltaTime * 5f);

        Vector3 dirToPlayer = player.position - transform.position;
        float distToPlayer = dirToPlayer.magnitude;

        if (distToPlayer > viewDistance || Physics.Linecast(transform.position, player.position, obstacleMask))
        {
            loseTimer += Time.deltaTime;
            if (loseTimer >= loseTargetTime)
            {
                detectionMeter = Mathf.Max(0, detectionMeter - Time.deltaTime * 30f); 
                currentState = State.Patrolling;
                agent.isStopped = false;
                GoToNextPoint();
            }
        }
        else
        {
            loseTimer = 0f;
            float rate = detectionRate * (1f / distToPlayer);
            detectionMeter = Mathf.Clamp(detectionMeter + rate * Time.deltaTime * 100f, 0, maxDetection);
        }
    }

    void InvestigateLogic()
    {
        if (!reachedWhistlePoint && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            reachedWhistlePoint = true;
            StartCoroutine(WaitAndReturn());
        }
    }

    public void Investigate(Vector3 position)
    {
        investigationPosition = position;
        agent.SetDestination(investigationPosition);
        currentState = State.Investigating;
        agent.isStopped = false;
        reachedWhistlePoint = false;
    }
    private IEnumerator WaitAndReturn()
    {
        yield return new WaitForSeconds(2f);

        Vector3 dirToPlayer = player.position - transform.position;
        float distToPlayer = dirToPlayer.magnitude;

        if (distToPlayer <= viewDistance && !Physics.Linecast(transform.position, player.position, obstacleMask))
        {
            currentState = State.Detecting;
            agent.isStopped = true;
        }
        else
        {
            currentState = State.Patrolling;
            GoToNextPoint();
        }
    }


}
