using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class SopkaTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayableAsset asset;
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
        StartCoroutine(EndCoroutine());
    }
    private IEnumerator EndCoroutine()
    {
        CanvasControllerChapter1.instance.blackScreen.DOFade(1, 1);
        ThirdPersonController.instance.StateCharacter(false);
        yield return new WaitForSeconds(1);
        director.playableAsset = asset;
        director.Play();
    }
}
