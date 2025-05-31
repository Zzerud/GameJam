using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class StartStealthAgain : MonoBehaviour
{
    public static StartStealthAgain instance { get; private set; }

    [SerializeField] private TMP_Text retryText;
    [SerializeField] private AttackOne attack;
    [SerializeField] private LightFires[] camps;
    [SerializeField] private Barrel barrel;
    [SerializeField] private TriggerEnemyLogs logs;
    [SerializeField] private TriggerBoats boats;
    [SerializeField] private ResonanceTrigger resonanceT;
    [SerializeField] private ResonancePuzzle resonanceP;
    [SerializeField] private EnemyNPCBehaviour[] enemys;

    [Space]
    [SerializeField] private GameObject triggerSopka;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void GotYa()
    {
        StartCoroutine(Retry());
    }
    private IEnumerator Retry()
    {
        CanvasControllerChapter1.instance.blackScreen.DOFade(1, 1);
        CanvasControllerChapter1.instance.interactText.text = "Нажмите \"E\" для взаимодействия";
        triggerSopka.SetActive(false);
        WindyController.instance.ended = false;
        ThirdPersonController.instance.StateCharacter(false);
        retryText.DOFade(1, 2);
        yield return new WaitForSeconds(2);

        attack.EndCutscene();
        retryText.DOFade(0, 1);
        RedVignetteManager.instance.Retry();
        foreach (EnemyNPCBehaviour enemy in enemys)
        {
            enemy.detectionMeter = 0;
        }
        TaskManager.instance.Retry("Зажгите 3 костра", "Опрокиньте бочку", "Приманите неприятеля к лесорубу", "Проколите лодки неприятелей", "Проведите ритуал на вышине");
        WindyController.instance.currentFill = 5;
        WindyController.instance.FillWind(0);
        foreach (var camp in camps)
        {
            camp.Deactivate();
        }
        barrel.Deactivate();
        logs.Deactivate();
        boats.Deactivate();
        resonanceT.Deactivate();
        resonanceP.isSolved = false;

        yield return new WaitForSeconds(4);
        logs.isRetry = false;
    }
}
