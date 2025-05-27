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
    private float defaultRotation;

    private void Start()
    {
        defaultRotation = transform.rotation.y;
        player = ThirdPersonController.instance.transform;
    }
    private void Update()
    {
        if (interaction) WatchWhileTalk();
    }
    public void StartTalk()
    {
        interaction = true;
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
        isTalked = true;
    }
    private void WatchWhileTalk()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
