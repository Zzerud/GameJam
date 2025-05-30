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
            CanvasControllerChapter1.instance.InteractState(true);
            CanvasControllerChapter1.instance.currentCamp = this;
            CanvasControllerChapter1.instance.interactsState = CanvasControllerChapter1.InteractStates.Camp;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(false);
            CanvasControllerChapter1.instance.currentCamp = null;
        }
    }

    public void Activated()
    {
        activated = true;
        fireVfx.SetActive(true);
        CanvasControllerChapter1.instance.InteractState(false);
        CanvasControllerChapter1.instance.currentCamp = null;
        WindyController.instance.FillWind(10);

        AttackOne.campsOpen++;
        if (AttackOne.campsOpen == 3) TaskManager.instance.tasks[0].CompleteTask();
    }
}
