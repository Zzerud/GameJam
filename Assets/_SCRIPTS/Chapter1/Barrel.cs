using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private EnemyNPCBehaviour enemy;
    [SerializeField] private Transform newPoint;
    private bool isEnabled = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isEnabled)
        {
            CanvasController.instance.InteractState(true);
            CanvasController.instance.barrel = this;
            CanvasController.instance.interactsState = CanvasController.InteractStates.Barrel;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasController.instance.InteractState(false);
            CanvasController.instance.barrel = null;
        }
    }

    public void BarrelAnim()
    {
        anim.SetTrigger("fall");
        enemy.patrolPoints[0] = newPoint;
        enemy.isStanding = false;
        TaskManager.instance.tasks[1].CompleteTask();
    }
}
