using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasControllerChapter1 : MonoBehaviour
{
    public static CanvasControllerChapter1 instance {  get; private set; }
    public Image blackScreen;

    [Header("Interaction with NPC")]
    public TMP_Text interactText;
    public TMP_Text interactQText;
    [SerializeField] private float interactDur = 0.6f;
    private bool isEnabledTointeract = false;

    [Header("Scripts")]
    [SerializeField] private AttackOne attackOne;
    [SerializeField] private SopkaTrigger sopkaEnd;
    [HideInInspector] public FriendNPCBehaviour currentNPC;
    [HideInInspector] public LightFires currentCamp;
    [HideInInspector] public Barrel barrel;
    [HideInInspector] public TriggerBoats boats;
    [HideInInspector] public ResonanceTrigger resonance;
    [HideInInspector] public TotemBehaviour currentTotem;
    [HideInInspector] public EndChapter1_3 end;
    [HideInInspector] public Chapter2Totems endTotems;

    public enum InteractStates { Talks, Game, Camp, Barrel, Boats, Resonance, Sopka, Totems, End, EndTotems }
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
                case InteractStates.Boats:
                    boats.DestroyBoats();
                    break;
                 case InteractStates.Resonance:
                    resonance.StartGame();
                    break;
                case InteractStates.Sopka:
                    sopkaEnd.EndGame();
                    break;
                case InteractStates.Totems:
                    currentTotem.TakeTotem();
                    break;
                case InteractStates.End:
                    end.InteractWithItems();
                    break;
                case InteractStates.EndTotems:
                    endTotems.InteractWithItems();
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
