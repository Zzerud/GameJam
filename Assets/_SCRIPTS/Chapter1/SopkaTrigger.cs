using UnityEngine;

public class SopkaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.interactsState = CanvasControllerChapter1.InteractStates.Sopka;
            CanvasControllerChapter1.instance.InteractState(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(false);
        }
    }

    public void EndGame()
    {

    }
}
