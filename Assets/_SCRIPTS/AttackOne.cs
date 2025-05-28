using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class AttackOne : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayableAsset asset;
    
    [Space]
    [SerializeField] private FriendNPCBehaviour[] friends;
    [SerializeField] private Transform startToGame;
    [SerializeField] private GameObject boats, enemys;
    [SerializeField] private Volume volume;
    [SerializeField] private VolumeProfile profile2;

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
        ThirdPersonController.instance.StateCharacter(false);
        CanvasController.instance.blackScreen.DOFade(1, 2);
        StartCoroutine(StartCutScene());
    }
    private IEnumerator StartCutScene()
    {
        yield return new WaitForSeconds(2);
        volume.profile = profile2;
        director.playableAsset = asset;
        director.Play();
    }
    public void EndCutscene()
    {
        ThirdPersonController.instance.transform.position = startToGame.position;
        ThirdPersonController.instance.transform.rotation = Quaternion.Euler(0, -180, 0);
        ThirdPersonController.instance.StateCharacter(true);
        CanvasController.instance.blackScreen.DOFade(0, 1);
        foreach (FriendNPCBehaviour f in friends)
        {
            f.SetPositions();
        }
        boats.SetActive(true);
        enemys.SetActive(true);
    }
}
