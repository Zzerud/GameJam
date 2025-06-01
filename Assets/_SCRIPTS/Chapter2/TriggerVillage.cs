using UnityEngine;

public class TriggerVillage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TaskManager.instance.tasks[0].CompleteTask();
            TaskManager.instance.CheckTasks("� ���...", "��������� ������... �����...", "������ ����� 6 �������", "����� ���������", "� �� ���� �� ��������...");
        }
    }
}
