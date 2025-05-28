using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance {  get; private set; }
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ChangeMusic(float durationFade, float durationWithoutMusic, float durationFadeIn, AudioClip music, float musicVol = 0.7f)
    {
        StartCoroutine(Changes(durationFade, durationWithoutMusic, durationFadeIn, music, musicVol));
    }
    private IEnumerator Changes(float durationFade, float durationWithoutMusic, float durationFadeIn, AudioClip music, float musicVol = 0.7f)
    {
        yield return audioSource.DOFade(0, durationFade).WaitForCompletion();
        audioSource.Stop();
        
        yield return new WaitForSeconds(durationWithoutMusic);

        audioSource.clip = music;
        audioSource.volume = 0;
        audioSource.Play();

        yield return audioSource.DOFade(musicVol, durationFadeIn).WaitForCompletion();
    }
}
