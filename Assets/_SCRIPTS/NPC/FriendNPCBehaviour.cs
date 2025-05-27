using System.Collections;
using TMPro;
using UnityEngine;

public class FriendNPCBehaviour : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private string fullText;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float typeSpeed = 0.05f;

    private bool interaction = false;
    public bool isTalked = false;
    private Quaternion defaultRotation;

    private void Start()
    {
        defaultRotation = transform.rotation;
        player = ThirdPersonController.instance.transform;
    }
    private void Update()
    {
        if (interaction) WatchWhileTalk();
        else ReturnToDefault();
    }
    public void StartTalk()
    {
        interaction = true;
        isTalked = true;
        StartCoroutine(TypeText());
    }
    private IEnumerator TypeText()
    {
        text.text = "<mark=#8A613050>";
        foreach (char c in fullText)
        {
            text.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        yield return new WaitForSeconds(2);
        text.text = "";
        interaction = false;
        
    }
    private void WatchWhileTalk()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
    private void ReturnToDefault()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, Time.deltaTime * 10);
        if (Quaternion.Angle(transform.rotation, defaultRotation) < 1)
        {
            transform.rotation = defaultRotation;
        }
    }
}
