using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class ResonancePuzzle : MonoBehaviour
{
    [SerializeField] private CanvasGroup game;
    [Header("Puzzle Rings")]
    public Transform innerRing;    // Tone
    public Transform middleRing;   // Wind Direction
    public Transform outerRing;    // Volume

    [Space]
    [SerializeField] private Transform neededTone;
    [SerializeField] private Transform neededDir;
    [SerializeField] private Transform neededVol;

    [Header("Target Angles")]
    [Range(0f, 360f)] public float targetToneAngle;
    [Range(0f, 360f)] public float targetDirectionAngle;
    [Range(0f, 360f)] public float targetVolumeAngle;

    [Header("Tolerances (degrees)")]
    public float angleTolerance = 10f;


    private bool isSolved = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        targetToneAngle = Random.Range(0, 360f);
        targetDirectionAngle = Random.Range(0, 360f);
        targetVolumeAngle = Random.Range(0, 360f);

        neededTone.rotation = Quaternion.Euler(0, 0, targetToneAngle);
        neededDir.rotation = Quaternion.Euler(0, 0, targetDirectionAngle);
        neededVol.rotation = Quaternion.Euler(0, 0, targetVolumeAngle);
    }
    void Update()
    {
        if (isSolved) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float tone = NormalizeAngle(innerRing.eulerAngles.z);
        float direction = NormalizeAngle(middleRing.eulerAngles.z);
        float volume = NormalizeAngle(outerRing.eulerAngles.z);

        bool toneMatch = Mathf.Abs(tone - targetToneAngle) <= angleTolerance;
        bool dirMatch = Mathf.Abs(direction - targetDirectionAngle) <= angleTolerance;
        bool volMatch = Mathf.Abs(volume - targetVolumeAngle) <= angleTolerance;

        if (toneMatch && dirMatch && volMatch)
        {
            isSolved = true;
            ThirdPersonController.instance.StateCharacter(true);
            game.DOFade(0, 0.5f).SetEase(Ease.OutCubic).OnComplete(() => game.gameObject.SetActive(false));
            WindyController.instance.FillWind(40);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            TaskManager.instance.tasks[4].CompleteTask();
            Debug.Log("END");
        }
        else if ((toneMatch && dirMatch) || (dirMatch && volMatch) || (toneMatch && volMatch))
        {
            //onNearMatch.Invoke(); // e.g. wind grows, sound shifts
        }
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f) angle += 360f;
        return angle;
    }
}
