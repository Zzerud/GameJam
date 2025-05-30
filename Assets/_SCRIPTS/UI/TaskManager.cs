using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance { get; private set; }

    public Task[] tasks;
    public bool isCompletedTasks1;


    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void CheckTasks(string task1 = "", string task2 = "", string task3 = "", string task4 = "", string task5 = "")
    {

        string[] taskTexts = { task1, task2, task3, task4, task5 };

        for (int i = 0; i < tasks.Length; i++)
        {
            if (!tasks[i].toggle.isOn && tasks[i].gameObject.activeSelf) return;
        }

        foreach (Task task in tasks)
        {
            task.gameObject.SetActive(false);
            task.currentText = "";
        }

        isCompletedTasks1 = true;

        for (int i = 0; i < tasks.Length && i < taskTexts.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(taskTexts[i]))
            {
                tasks[i].currentText = taskTexts[i];
                tasks[i].gameObject.SetActive(true); 
            }
        }

        foreach (Task task in tasks)
        {
            if (task.gameObject.activeSelf) 
                task.ShowTask();
        }
    }
}
