using System.Collections;
using TMPro;
using UnityEngine;

public class StartChapter1_3 : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [TextArea(100, 10)][SerializeField] private string textString;

    private void Start()
    {
        StartCoroutine(StartChapter());
    }
    private IEnumerator StartChapter() 
    {
        int i = 0;
        while (i < textString.Length)
        {
            if (textString[i] == '<') // начало тега
            {
                int tagEnd = textString.IndexOf('>', i);
                if (tagEnd == -1)
                {
                    break;
                }

                string tag = textString.Substring(i, tagEnd - i + 1);
                text.text += tag;
                i = tagEnd + 1;
            }
            else
            {
                text.text += textString[i];
                i++;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
