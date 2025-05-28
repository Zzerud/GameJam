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

    [Header("Scripts")]
    [SerializeField] private AttackOne attackOne;
    [HideInInspector] public FriendNPCBehaviour currentNPC;

    public enum InteractStates { Talks, Game }
    public InteractStates interactsState = InteractStates.Game;

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
            switch (interactsState)
            {
                case InteractStates.Talks:
                    currentNPC.StartTalk();
                    break;
                case InteractStates.Game:
                    attackOne.StartAttack();
                    break;
            }
        }
    }

    public void InteractState(bool active)
    {
        interactText.DOKill();
        interactText.DOFade(active ? 1 : 0, interactDur);
        isEnabledTointeract = active;
    }
}
