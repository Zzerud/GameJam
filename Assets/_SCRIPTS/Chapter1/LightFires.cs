using UnityEngine;

public class LightFires : MonoBehaviour
{
    [SerializeField] private GameObject fireVfx;
    public bool isAttacked = false;
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !activated && isAttacked)
        {
            CanvasController.instance.InteractState(true);
            CanvasController.instance.currentCamp = this;
            CanvasController.instance.interactsState = CanvasController.InteractStates.Camp;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasController.instance.InteractState(false);
            CanvasController.instance.currentCamp = null;
        }
    }

    public void Activated()
    {
        activated = true;
        fireVfx.SetActive(true);
        CanvasController.instance.InteractState(false);
        CanvasController.instance.currentCamp = null;
        WindyController.instance.FillWind(5);

        AttackOne.campsOpen++;
        if (AttackOne.campsOpen == 3) TaskManager.instance.tasks[0].CompleteTask();
    }
}
