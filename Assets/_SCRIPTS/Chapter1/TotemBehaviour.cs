using UnityEngine;

public class TotemBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject totemObj;
    private bool isCaptured = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isCaptured)
        {
            CanvasControllerChapter1.instance.InteractState(true);
            CanvasControllerChapter1.instance.currentTotem = this;
            CanvasControllerChapter1.instance.interactsState = CanvasControllerChapter1.InteractStates.Totems;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(false);
            CanvasControllerChapter1.instance.currentTotem = null;
        }
    }

    public void TakeTotem()
    {
        isCaptured = true;
        totemObj.SetActive(false);
        TotemsManager.instance.CheckTotems();
    }
}
