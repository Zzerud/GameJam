using UnityEngine;

public class TriggerEnemyLogs : MonoBehaviour
{
    [SerializeField] private EnemyNPCBehaviour enemy;
    [SerializeField] private Transform newPoint, oldPoint;
    public bool isRetry = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC") && !isRetry)
        {
            enemy.patrolPoints[0] = newPoint;
            enemy.currentState = EnemyNPCBehaviour.State.Patrolling;
            enemy.GoToNextPoint();
            Debug.Log("Enemy is entered Trigger");
            TaskManager.instance.tasks[2].CompleteTask();
        }
    }

    public void Deactivate()
    {
        isRetry = true;
        enemy.patrolPoints[0] = oldPoint;
        enemy.currentState = EnemyNPCBehaviour.State.Patrolling;
        enemy.agent.SetDestination(oldPoint.position);
        enemy.GoToNextPoint();
        enemy.agent.Warp(oldPoint.position);
    }
}
