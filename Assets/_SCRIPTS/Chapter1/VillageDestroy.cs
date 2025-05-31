using UnityEngine;

public class VillageDestroy : MonoBehaviour
{
    private bool entered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !entered)
        {
            TaskManager.instance.tasks[0].CompleteTask();

            TaskManager.instance.CheckTasks("���� ���� ���-�� ��������... � ������ ���... ���� �����", "��� �����... ���������... ���� ����...", "����� ����� ���...", "� ������ ����... �� ���� ������...", "������ ������, �� ���� �� ����� ������");
        }
    }
}
