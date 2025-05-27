using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemyNPCBehaviour : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float viewDistance = 10;
    [SerializeField] private float detectionRate = 1;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Transform player;
    [SerializeField] private float loseTargetTime = 3f;
    [SerializeField] private float waitTimeAtPoint = 2f;
    [SerializeField] private float detectionMin = 10f;

    [Space]
    [SerializeField] private Volume volume;
    private Vignette vg;

    private NavMeshAgent agent;
    private int currentPatrolIndex;
    public float detectionMeter = 0;
    public float maxDetection = 100;
    public float loseTimer = 0;
    private float waitTimer = 0f;

    private enum State { Patrolling, Waiting, Detecting }
    private State currentState = State.Patrolling;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = ThirdPersonController.instance.transform;
        GoToNextPoint();

        if (volume.profile.TryGet(out vg))
        {
            vg.color.value = Color.red;
            vg.intensity.value = 0;
            vg.smoothness.value = 0.666f;
        }
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
        }

        CheckForPlayer();
        vg.intensity.value = detectionMeter / 100;
    }

    private void GoToNextPoint()
    {
        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
    private void WaitLogic()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTimeAtPoint)
        {
            GoToNextPoint();
            currentState = State.Patrolling;
            agent.isStopped = false;
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
        Vector3 dirToPlayer = player.position - transform.position;
        float distToPlayer = dirToPlayer.magnitude;

        if (distToPlayer <= viewDistance && !Physics.Linecast(transform.position, player.position, obstacleMask))
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
}
