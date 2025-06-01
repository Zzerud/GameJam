using UnityEngine;

public class TriggerVillage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TaskManager.instance.tasks[0].CompleteTask();
            TaskManager.instance.CheckTasks("О нет...", "Проведите ритуал... Опять...", "Теперь нужно 6 тотемов", "Центр поселения", "Я не смог их защитить...");
        }
    }
}
