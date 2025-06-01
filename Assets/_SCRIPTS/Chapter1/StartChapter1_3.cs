using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartChapter1_3 : MonoBehaviour
{
    public static StartChapter1_3 instance {  get; private set; }

    [SerializeField] private Image black;
    [SerializeField] private TMP_Text text;
    [TextArea(10, 10)] public string[] textString;

    private bool endChapter = false;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        StartCoroutine(StartChapter(0));
        ThirdPersonController.instance.StateCharacter(false);
    }

    public void EndChapter()
    {
        text.text = "";
        black.DOFade(1, 2);
        text.DOFade(1, 2);
        StartCoroutine(StartChapter(2.5f));
        endChapter = true;
    }

    private IEnumerator StartChapter(float timeToExecute)
    {
        yield return new WaitForSeconds(timeToExecute);
        for (int j = 0; j < textString.Length; j++)
        {
            text.text = "";
            int i = 0;

            while (i < textString[j].Length)
            {
                if (textString[j][i] == '<')
                {
                    int tagEnd = textString[j].IndexOf('>', i);
                    if (tagEnd == -1)
                    {
                        break;
                    }

                    string tag = textString[j].Substring(i, tagEnd - i + 1);
                    text.text += tag;
                    i = tagEnd + 1;
                }
                else
                {
                    text.text += textString[j][i];
                    i++;
                    yield return new WaitForSeconds(0.05f);
                }
            }

            yield return new WaitForSeconds(2f);
        }
        if (!endChapter)
        {
            black.DOFade(0, 1);
            text.DOFade(0, 1);
            ThirdPersonController.instance.StateCharacter(true);
        }
        else
        {
            Loading.instance.StartLoading("Chapter2_2");
        }
    }
}
