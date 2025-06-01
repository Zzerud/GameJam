using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RedVignetteManager : MonoBehaviour
{
    public static RedVignetteManager instance {  get; private set; }

    [SerializeField] private Image redVignette;
    public float maxIntensity = 0;
    public bool retry = false;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        redVignette.CrossFadeAlpha(0, 0, true);
    }
    private void LateUpdate()
    {
        redVignette.CrossFadeAlpha(maxIntensity, 0, true);
        maxIntensity = 0;
    }
    public void RegisterIntensity(float intensity)
    {
        maxIntensity = Mathf.Max(maxIntensity, intensity);
        if (maxIntensity >= 1 && !retry)
        {
            StartStealthAgain.instance.GotYa();
            retry = true;
        }
    }

    public void Retry()
    {
        maxIntensity = 0;
        redVignette.CrossFadeAlpha(0, 0, false);
        retry = false;
    }
    
}
