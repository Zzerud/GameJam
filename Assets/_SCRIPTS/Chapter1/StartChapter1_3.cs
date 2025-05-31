using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartChapter1_3 : MonoBehaviour
{
    [SerializeField] private Image black;
    [SerializeField] private TMP_Text text;
    [TextArea(10, 10)][SerializeField] private string[] textString;

    private void Start()
    {
        StartCoroutine(StartChapter());
    }

    private IEnumerator StartChapter()
    {
        for (int j = 0; j < textString.Length; j++)
        {
            text.text = ""; // Очистка перед началом строки
            int i = 0;

            while (i < textString[j].Length)
            {
                if (textString[j][i] == '<')
                {
                    int tagEnd = textString[j].IndexOf('>', i);
                    if (tagEnd == -1)
                    {
                        Debug.LogWarning("Незакрытый тег в строке.");
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

        black.DOFade(0, 1);
        text.DOFade(0, 1);
    }
}
