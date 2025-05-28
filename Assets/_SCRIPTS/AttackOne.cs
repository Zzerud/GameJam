using UnityEngine;

public class AttackOne : MonoBehaviour
{
    private bool isStarted = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isStarted)
        {
            CanvasController.instance.interactsState = CanvasController.InteractStates.Game;
            CanvasController.instance.InteractState(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasController.instance.InteractState(false);
        }
    }

    public void StartAttack()
    {
        isStarted = true;
        WindyController.instance.FillWind(10);
        ThirdPersonController.instance.isEnabledMove = false;
    }
}
