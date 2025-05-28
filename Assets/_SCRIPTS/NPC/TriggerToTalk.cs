using UnityEngine;

public class TriggerToTalk : MonoBehaviour
{
    [SerializeField] private FriendNPCBehaviour manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !manager.isTalked)
        {
            CanvasController.instance.InteractState(true);
            CanvasController.instance.currentNPC = manager;
            CanvasController.instance.interactsState = CanvasController.InteractStates.Talks;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasController.instance.InteractState(false);
            CanvasController.instance.currentNPC = null;
        }
    }
}
