using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindyController : MonoBehaviour
{
    public static WindyController instance { get; private set; }
    [SerializeField] private Image windFill;
    [SerializeField] private float currentFill;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void FillWind(float fillPlus)
    {
        currentFill += fillPlus;
        if (currentFill > 100) currentFill = 100;

        windFill.DOFillAmount(currentFill / 100, 0.6f).SetEase(Ease.OutBack);
    }
}
