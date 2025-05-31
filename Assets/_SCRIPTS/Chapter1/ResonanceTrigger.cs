using DG.Tweening;
using UnityEngine;

public class ResonanceTrigger : MonoBehaviour
{
    [SerializeField] private CanvasGroup game;

    private bool isCompleted = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isCompleted)
        {
            CanvasControllerChapter1.instance.InteractState(true);
            CanvasControllerChapter1.instance.resonance = this;
            CanvasControllerChapter1.instance.interactsState = CanvasControllerChapter1.InteractStates.Resonance;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(false);
            CanvasControllerChapter1.instance.resonance = null;
        }
    }

    public void StartGame()
    {
        game.gameObject.SetActive(true);
        game.DOFade(1, 0.4f).SetEase(Ease.InQuart);
        ThirdPersonController.instance.StateCharacter(false);
        isCompleted = true;
    }

    public void Deactivate()
    {
        isCompleted = false;
    }
}
