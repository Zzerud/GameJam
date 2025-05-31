using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private EnemyNPCBehaviour enemy;
    [SerializeField] private Transform newPoint, oldPoint;
    private bool isEnabled = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isEnabled)
        {
            CanvasControllerChapter1.instance.InteractState(true);
            CanvasControllerChapter1.instance.barrel = this;
            CanvasControllerChapter1.instance.interactsState = CanvasControllerChapter1.InteractStates.Barrel;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(false);
            CanvasControllerChapter1.instance.barrel = null;
        }
    }

    public void BarrelAnim()
    {
        anim.SetBool("fall", true);
        enemy.patrolPoints[0] = newPoint;
        enemy.isStanding = false;
        TaskManager.instance.tasks[1].CompleteTask();
    }
    public void Deactivate()
    {
        anim.SetBool("fall", false);
        enemy.patrolPoints[0] = oldPoint;
        enemy.currentState = EnemyNPCBehaviour.State.Patrolling;
        enemy.agent.SetDestination(oldPoint.position);
        enemy.GoToNextPoint();
        enemy.agent.Warp(oldPoint.position);
        enemy.isStanding = true;
        isEnabled = true;
    }
}
