using DG.Tweening;
using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance {  get; private set; }

    [Header("Interaction with NPC")]
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private float interactDur = 0.6f;
    private bool isEnabledTointeract = false;
    [HideInInspector] public FriendNPCBehaviour currentNPC;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (isEnabledTointeract && Input.GetKeyDown(KeyCode.E))
        {
            InteractState(false);
            currentNPC.StartTalk();
        }
    }

    public void InteractState(bool active)
    {
        interactText.DOKill();
        interactText.DOFade(active ? 1 : 0, interactDur);
        isEnabledTointeract = active;
    }
}
