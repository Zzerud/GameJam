using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RedVignetteManager : MonoBehaviour
{
    public static RedVignetteManager instance {  get; private set; }

    [SerializeField] private Image redVignette;
    private float maxIntensity = 0;

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
    }

    
    
}
