using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public Toggle toggle;
    public string currentText;
    private bool isCompleted = false;


    public void CompleteTask()
    {
        toggle.isOn = true;
    }
    public void ShowTask()
    {
        StartCoroutine(TaskBehaviour());
    }
    private IEnumerator TaskBehaviour()
    {
        for (int i = text.text.Length - 1; i >= 0; i--)
        {
            text.text = text.text.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
        toggle.isOn = false;
        
        if (currentText == "")
        {
            gameObject.SetActive(false);
        }

        foreach (char c in currentText)
        {
            text.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
