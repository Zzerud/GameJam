using DG.Tweening;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private CanvasGroup canvasGroup;
    public void StartGame()
    {
        Loading.instance.StartLoading("Chapter1_1");
        Settings.isGame = true;
    }

    public void SettingsOpen()
    {
        settingsPanel.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0;
        settingsPanel.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(settingsPanel.transform.DOScale(new Vector3(1.05f, 1.05f, 1), 0.4f)
            .SetEase(Ease.OutBack))
           .Join(canvasGroup.DOFade(1, 0.4f))
           .Append(settingsPanel.transform.DOScale(Vector3.one, 0.2f));

        seq.Play();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
