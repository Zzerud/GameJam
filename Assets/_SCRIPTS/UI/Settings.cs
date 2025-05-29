using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Settings : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject settingsPanel;

    [Space]
    [SerializeField] private Slider volume;
    [SerializeField] private Vector2 leftPos, rightPos;
    [SerializeField] private TMP_Text resCurrent, resNext;
    [SerializeField] private TMP_Text windowCurrent, windowNext;

    private Resolution[] availableResolutions;
    private int currentResolutionIndex = 0;
    private string[] win = { "В окне", "Полный" };
    private int winIndex = 0;

    private bool isAnimatingWindow = false, isAnimatingRes = false, openedSettings = false;
    public static bool isGame = false;
    private List<AudioSource> gameAudioSources = new List<AudioSource>();



    private void Start()
    {
        volume.value = PlayerPrefs.GetFloat("Volume", 1f);
        volume.onValueChanged.AddListener(SetVolume);
        AudioListener.volume = volume.value;


        availableResolutions = Screen.resolutions
            .GroupBy(r => new { r.width, r.height })
            .Select(g => g.First())
            .ToArray();

        int savedWidth = PlayerPrefs.GetInt("ResWidth", Screen.currentResolution.width);
        int savedheight = PlayerPrefs.GetInt("ResHeight", Screen.currentResolution.height);

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].width == savedWidth &&
                availableResolutions[i].height == savedheight)
            {
                currentResolutionIndex = i;
                break;
            }
        }
        resCurrent.text = $"{savedWidth}x{savedheight}";
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGame && !openedSettings)
        {
            Time.timeScale = 0;
            AudioSource[] audio = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            foreach (var audios in audio)
            {
                gameAudioSources.Add(audios);
                audios.Pause();
            }
            OpenSettings();
        }
    }
    private void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
    public void ChangeResolution(int direction)
    {
        if (isAnimatingRes) return;

        isAnimatingRes = true;
        currentResolutionIndex = (currentResolutionIndex + direction + availableResolutions.Length) % availableResolutions.Length;
        Resolution res = availableResolutions[currentResolutionIndex];

        if (direction == -1) // left
        {
            resNext.rectTransform.DOAnchorPos(leftPos, 0).SetUpdate(true);
            resNext.text = $"{res.width}x{res.height}";

            resCurrent.rectTransform.DOAnchorPos(rightPos, 0.35f).SetUpdate(true).SetEase(Ease.OutCubic);
            resNext.rectTransform.DOAnchorPos(Vector2.zero, 0.35f).SetUpdate(true).SetEase(Ease.OutBack).OnComplete(() =>
            {
                resCurrent.text = $"{res.width}x{res.height}";
                resCurrent.rectTransform.DOAnchorPos(Vector2.zero, 0).SetUpdate(true);
                resNext.text = string.Empty;
                isAnimatingRes = false;
            });
        }
        else
        {
            resNext.rectTransform.DOAnchorPos(rightPos, 0).SetUpdate(true);
            resNext.text = $"{res.width}x{res.height}";

            resCurrent.rectTransform.DOAnchorPos(leftPos, 0.35f).SetUpdate(true).SetEase(Ease.OutCubic);
            resNext.rectTransform.DOAnchorPos(Vector2.zero, 0.35f).SetUpdate(true).SetEase(Ease.OutBack).OnComplete(() =>
            {
                resCurrent.text = $"{res.width}x{res.height}";
                resCurrent.rectTransform.DOAnchorPos(Vector2.zero, 0).SetUpdate(true);
                resNext.text = string.Empty;
                isAnimatingRes = false;
            });
        }
        SaveSettings();
    }

    public void SaveSettings()
    {
        Resolution res = availableResolutions[currentResolutionIndex];
        Screen.SetResolution(res.width, res.height, true);

        PlayerPrefs.SetInt("ResWidth", res.width);
        PlayerPrefs.SetInt("ResHeight", res.height);
    }
    public void CloseSettings()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(settingsPanel.transform.DOScale(new Vector3(.05f, .05f, 0), 0.4f)
            .SetEase(Ease.InBack)).SetUpdate(true)
           .Join(canvasGroup.DOFade(0, 0.4f))
           .Append(settingsPanel.transform.DOScale(Vector3.zero, 0.2f));
       
        Time.timeScale = 1;
        seq.Play();
        openedSettings = false;

        if (isGame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            foreach (var audios in gameAudioSources)
            {
                audios.UnPause();
            }

        }
    }
    public void OpenSettings()
    {
        settingsPanel.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0;
        settingsPanel.SetActive(true);
        openedSettings = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(settingsPanel.transform.DOScale(new Vector3(1.05f, 1.05f, 1), 0.4f)
            .SetEase(Ease.OutBack)).SetUpdate(true)
           .Join(canvasGroup.DOFade(1, 0.4f))
           .Append(settingsPanel.transform.DOScale(Vector3.one, 0.2f));

        seq.Play();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
