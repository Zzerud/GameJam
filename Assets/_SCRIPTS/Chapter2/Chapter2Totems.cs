using System.Collections.Generic;
using UnityEngine;

public class Chapter2Totems : MonoBehaviour
{
    [SerializeField] private GameObject totems;
    [TextArea(10, 10)] public List<string> textString = new List<string>();
    [SerializeField] private StartChapter2 ch2;
    [SerializeField] private GameObject wall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(true);
            CanvasControllerChapter1.instance.endTotems = this;
            CanvasControllerChapter1.instance.interactsState = CanvasControllerChapter1.InteractStates.EndTotems;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasControllerChapter1.instance.InteractState(false);
            CanvasControllerChapter1.instance.endTotems = null;
        }
    }

    public void InteractWithItems()
    {
        totems.SetActive(true);
        ThirdPersonController.instance.StateCharacter(false);
        ch2.textString.Clear();
        foreach (string item in textString)
        {
            ch2.textString.Add(item);
        }
        ch2.StartVriteTotems();
        wall.SetActive(false);

        for (int i = 0; i < TaskManager.instance.tasks.Length; i++)
        {
            TaskManager.instance.tasks[i].CompleteTask();
        }
        TaskManager.instance.CheckTasks("бегите к лесным духам");
        gameObject.SetActive(false);
    }
}
