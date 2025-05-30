using UnityEngine;

public class TriggerEnemyLogs : MonoBehaviour
{
    [SerializeField] private EnemyNPCBehaviour enemy;
    [SerializeField] private Transform newPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            enemy.patrolPoints[0] = newPoint;
            enemy.currentState = EnemyNPCBehaviour.State.Patrolling;
            enemy.GoToNextPoint();
            Debug.Log("Enemy is entered Trigger");
            TaskManager.instance.tasks[2].CompleteTask();
        }
    }
}
