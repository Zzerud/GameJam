using UnityEngine;

public class TriggerEnemyLogs : MonoBehaviour
{
    [SerializeField] private EnemyNPCBehaviour enemy;
    [SerializeField] private Transform newPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == enemy.gameObject)
        {
            enemy.patrolPoints[0] = newPoint;
            enemy.isStanding = false;
            Debug.Log("Enemy is entered Trigger");
            TaskManager.instance.tasks[2].CompleteTask();
        }
    }
}
