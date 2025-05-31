using UnityEngine;

public class VillageDestroy : MonoBehaviour
{
    private bool entered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !entered)
        {
            TaskManager.instance.tasks[0].CompleteTask();

            TaskManager.instance.CheckTasks("Если хоть что-то осталось... Я соберу это... Даже щепки", "Она стоит... Наблюдает... Дает силу...", "Всего нужно три...", "Я вырежу лицо... Не хочу забыть...", "Пустой костер, но руки всё равно кладут");
        }
    }
}
