using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class AttackOne : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayableAsset asset;
    [SerializeField] private AudioClip music;
    
    [Space]
    [SerializeField] private FriendNPCBehaviour[] friends;
    [SerializeField] private Transform startToGame;
    [SerializeField] private GameObject boats, enemys;
    [SerializeField] private Volume volume;
    [SerializeField] private VolumeProfile profile2;
    [SerializeField] private LightFires[] camp;
    [SerializeField] private GameObject barrel, triggerEnemy;

    private bool isStarted = false;
    public static int campsOpen = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isStarted && TaskManager.instance.isCompletedTasks1)
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
        TaskManager.instance.tasks[0].CompleteTask();
        yield return new WaitForSeconds(2);
        MusicManager.instance.ChangeMusic(7, 1, 5, music);
        volume.profile = profile2;
        director.playableAsset = asset;
        director.Play();
    }
    public void EndCutscene()
    {
        ThirdPersonController.instance.transform.position = startToGame.position;
        ThirdPersonController.instance.transform.rotation = Quaternion.Euler(0, -75, 0);
        ThirdPersonController.instance.StateCharacter(true);
        ThirdPersonController.instance.animator.SetBool("Crouch", true);
        ThirdPersonController.instance.isEnabledRun = false;
        ThirdPersonController.instance.isEnabledJump = false;
        ThirdPersonController.instance.pivotOffset.y = 0.65f;
        CanvasController.instance.blackScreen.DOFade(0, 1);
        foreach (FriendNPCBehaviour f in friends)
        {
            f.SetPositions();
        }
        foreach (LightFires f in camp)
        {
            f.isAttacked = true;
        }
        boats.SetActive(true);
        enemys.SetActive(true);
        barrel.SetActive(true);
        triggerEnemy.SetActive(true);

        CanvasController.instance.interactQText.DOFade(1, 0.5f);
        TaskManager.instance.CheckTasks("Зажгите 3 костра", "Опрокиньте бочку", "Приманите врага к лесорубу");
    }
}
