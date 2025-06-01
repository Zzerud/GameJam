using UnityEngine;
using UnityEngine.Playables;

public class EndGame : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private AudioClip clip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            director.Play();
            ThirdPersonController.instance.StateCharacter(false);
        }
    }

    public void End()
    {
        Loading.instance.StartLoading("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        MusicManager.instance.ChangeMusic(1, 0, 1, clip);
    }
}
