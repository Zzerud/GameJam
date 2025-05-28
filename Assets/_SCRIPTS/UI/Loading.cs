using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public static Loading instance {  get; private set; }
    [SerializeField] private RectTransform loadingSprite;
    [SerializeField] private GameObject loadingCanvas;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(loadingCanvas);
    }

    public void StartLoading(string sceneToLoad)
    {
        loadingSprite.DOAnchorPosY(2000, 0);
        StartCoroutine(loader(sceneToLoad));
    }
    private IEnumerator loader(string sceneToLoad)
    {
        loadingSprite.DOAnchorPosY(0, .8f).SetEase(Ease.OutBounce);

        yield return new WaitForSeconds(1);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < .9f)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
        loadingSprite.DOAnchorPosY(-2000, .7f).SetEase(Ease.InQuint);

        yield return new WaitForSeconds(1.75f);

        

    }
}
