using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartChapter2 : MonoBehaviour
{

    [SerializeField] private Image black;
    [SerializeField] private TMP_Text text;
    [TextArea(10, 10)] public List<string> textString = new List<string>();

    [SerializeField] private AudioClip music;

    private void Start()
    {
        StartCoroutine(StartChapter(0));
        MusicManager.instance.ChangeMusic(0.5f, 0, 0.5f, music);
    }
    public void StartVriteTotems()
    {
        text.text = "";
        black.DOFade(1, 1);
        text.DOFade(1, 1);
        StartCoroutine(StartChapter(1.5f));
    }
    private IEnumerator StartChapter(float timeToExecute)
    {
        yield return new WaitForSeconds(timeToExecute);
        for (int j = 0; j < textString.Count; j++)
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
        black.DOFade(0, 1);
        text.DOFade(0, 1);
        ThirdPersonController.instance.StateCharacter(true);
    }
}
