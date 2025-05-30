using UnityEngine;

public class TriggerToTalk : MonoBehaviour
{
    [SerializeField] private FriendNPCBehaviour manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !manager.isTalked && manager.beforeAttack)
        {
            CanvasControllerChapter1.instance.InteractState(true);
            CanvasControllerChapter1.instance.currentNPC = manager;
            CanvasControllerChapter1.instance.interactsState = CanvasControllerChapter1.InteractStates.Talks;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(false);
            CanvasControllerChapter1.instance.currentNPC = null;
        }
    }
}
