using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance {  get; private set; }
    public Image blackScreen;

    [Header("Interaction with NPC")]
    [SerializeField] private TMP_Text interactText;
    public TMP_Text interactQText;
    [SerializeField] private float interactDur = 0.6f;
    private bool isEnabledTointeract = false;

    [Header("Scripts")]
    [SerializeField] private AttackOne attackOne;
    [HideInInspector] public FriendNPCBehaviour currentNPC;
    [HideInInspector] public LightFires currentCamp;
    [HideInInspector] public Barrel barrel;

    public enum InteractStates { Talks, Game, Camp, Barrel }
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
                case InteractStates.Camp:
                    currentCamp.Activated();
                    break;
                case InteractStates.Barrel:
                    barrel.BarrelAnim();
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
