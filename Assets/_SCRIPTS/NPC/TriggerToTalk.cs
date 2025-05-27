using UnityEngine;

public class TriggerToTalk : MonoBehaviour
{
    [SerializeField] private FriendNPCBehaviour manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !manager.isTalked)
        {
            CanvasController.instance.InteractState(true);
            CanvasController.instance.currentNPC = manager;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CanvasController.instance.InteractState(false);
            CanvasController.instance.currentNPC = null;
        }
    }
}
