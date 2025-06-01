using UnityEngine;

public class EndChapter1_3 : MonoBehaviour
{
    [SerializeField] private GameObject totems;
    [TextArea(10, 10)][SerializeField] private string[] textString;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(true);
            CanvasControllerChapter1.instance.end = this;
            CanvasControllerChapter1.instance.interactsState = CanvasControllerChapter1.InteractStates.End;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(false);
            CanvasControllerChapter1.instance.end = null;
        }
    }

    public void InteractWithItems()
    {
        totems.SetActive(true);
        ThirdPersonController.instance.StateCharacter(false);
        StartChapter1_3.instance.EndChapter();
        StartChapter1_3.instance.textString[0] = textString[0];
        StartChapter1_3.instance.textString[1] = textString[1];
        StartChapter1_3.instance.textString[2] = textString[2];
    }
}
