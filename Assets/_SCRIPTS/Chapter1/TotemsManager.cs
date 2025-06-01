using UnityEngine;

public class TotemsManager : MonoBehaviour
{
    public static TotemsManager instance { get; private set; }

    public int totalTotems = 0;
    [SerializeField] private GameObject totemItems;
    [SerializeField] private int neededTotems = 3;
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckTotems()
    {
        totalTotems++;
        if (totalTotems == neededTotems)
        {
            totemItems.SetActive(true);
        }
    }
}
