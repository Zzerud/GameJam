using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindyController : MonoBehaviour
{
    public static WindyController instance { get; private set; }
    [SerializeField] private Image windFill;
    public float currentFill;
    [SerializeField] private GameObject sopkaTrigger;

    public bool ended = false;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (currentFill == 100 && !ended) 
            FinalTask();
    }

    private void FinalTask()
    {
        ended = true;
        for (int i = 0; i < TaskManager.instance.tasks.Length; i++)
        {
            TaskManager.instance.tasks[i].CompleteTask();
        }

        TaskManager.instance.CheckTasks("Поднимитесь на сопку у берега");
        CanvasControllerChapter1.instance.interactText.text = "Нажмите \"E\" чтобы подняться на вершину СОПКИ";
        sopkaTrigger.SetActive(true);
    }

    public void FillWind(float fillPlus)
    {
        currentFill += fillPlus;
        if (currentFill > 100) currentFill = 100;

        windFill.DOFillAmount(currentFill / 100, 0.6f).SetEase(Ease.OutBack);
    }
}
