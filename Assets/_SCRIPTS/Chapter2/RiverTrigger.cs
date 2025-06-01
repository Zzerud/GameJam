using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RiverTrigger : MonoBehaviour
{
    [SerializeField] private bool isRiver = false;
    [SerializeField] private bool isRiverEnd = false;
    [SerializeField] private Transform riverStart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isRiver)
            {
                TaskManager.instance.tasks[0].CompleteTask();
                TaskManager.instance.CheckTasks("Перепрыгните по камням через речку");
                gameObject.SetActive(false);
            }
            else if (isRiverEnd)
            {
                TaskManager.instance.tasks[0].CompleteTask();
                TaskManager.instance.CheckTasks("Лесной дух впереди... Нужно к нему...");
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(River());
            }
        }
    }

    private IEnumerator River()
    {
        ThirdPersonController.instance.StateCharacter(false);
        CanvasControllerChapter1.instance.blackScreen.DOFade(1, 1);
        yield return new WaitForSeconds(1.5f);
        ThirdPersonController.instance.transform.position = riverStart.position;
        yield return new WaitForSeconds(0.5f);
        ThirdPersonController.instance.StateCharacter(true);
        CanvasControllerChapter1.instance.blackScreen.DOFade(0, 1);
    }
}
