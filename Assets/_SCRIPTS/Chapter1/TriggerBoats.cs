using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TriggerBoats : MonoBehaviour
{
    [SerializeField] private GameObject holes;

    private bool isCompleted = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isCompleted)
        {
            CanvasControllerChapter1.instance.InteractState(true);
            CanvasControllerChapter1.instance.boats = this;
            CanvasControllerChapter1.instance.interactsState = CanvasControllerChapter1.InteractStates.Boats;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(false);
            CanvasControllerChapter1.instance.boats = null;
        }
    }

    public void DestroyBoats()
    {
        StartCoroutine(Destroying());
    }
    private IEnumerator Destroying()
    {
        isCompleted = true;
        CanvasControllerChapter1.instance.blackScreen.DOFade(1, .25f);
        yield return new WaitForSeconds(0.35f);
        TaskManager.instance.tasks[3].CompleteTask();
        WindyController.instance.FillWind(25);
        holes.SetActive(true);
        CanvasControllerChapter1.instance.blackScreen.DOFade(0, .25f);
    }

    public void Deactivate()
    {
        isCompleted = false;
        holes.SetActive(false);
    }
}
