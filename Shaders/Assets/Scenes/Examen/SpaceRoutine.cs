using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRoutine : MonoBehaviour
{

    public Renderer meshRenderer = default;
    Color current = default;
    float h = 0;
    bool isToon = false;
    private void Awake()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<Renderer>();

        current = Color.HSVToRGB(0, 1, 1);
        isToon = meshRenderer.sharedMaterial.GetFloat("_UseToon")>0?true:false;
        //meshRenderer.sharedMaterial.SetColor()
    }

    private void Update()
    {
        h += Time.deltaTime*0.1f;
        if (h >= 1) h = 0;
        current = Color.HSVToRGB(h, 1, 1);
        meshRenderer.sharedMaterial.SetColor("_MainColor", current);
    }

    [ContextMenu("Aparecer")]
    public void EnableTransition()
    {
        StartCoroutine(DoTransition(2.5f, true));
    }

    [ContextMenu("Desaparecer")]
    public void DisableTransition()
    {
        StartCoroutine(DoTransition(2.5f, false));
    }

    [ContextMenu("Transparent")]
    public void DoTransparency()
    {
        StartCoroutine(DoBlink(1.5f));
    }
    public IEnumerator DoTransition(float _t, bool _forward)
    {
        float t = 0;
        float dt = 1 / _t; //1/10

        while (t < 1)
        {
            t += dt * Time.deltaTime;

            if (_forward)
                meshRenderer.sharedMaterial.SetFloat("_Transition", t);
            else
                meshRenderer.sharedMaterial.SetFloat("_Transition", 1-t);

            yield return null;
        }

        if (!_forward)
        {
            isToon = !isToon;
            meshRenderer.sharedMaterial.SetFloat("_UseToon", isToon?1:0);
        }
    }
    public IEnumerator DoBlink(float _t) 
    {
        float t = 0;
        float dt = 1 / _t; //1/10

        float max = 0.4f;
        Color currentFresnel = meshRenderer.sharedMaterial.GetColor("_FresnelColor");        
        Color.RGBToHSV(currentFresnel, out float hue, out float sat, out _);
        while (t < 1)
        {
            t += dt * Time.deltaTime;
            
            Color newColor = Color.HSVToRGB(hue, sat, 1 - t);
            meshRenderer.sharedMaterial.SetFloat("_refractionStrength", max * (1-t));
            meshRenderer.sharedMaterial.SetColor("_FresnelColor", newColor);

            yield return null;
        }
        t = 0;
        while (t < 1)
        {
            t += dt * Time.deltaTime;
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += dt * Time.deltaTime;

            Color newColor = Color.HSVToRGB(hue, sat, t);
            meshRenderer.sharedMaterial.SetFloat("_refractionStrength", max * t);
            meshRenderer.sharedMaterial.SetColor("_FresnelColor", newColor);

            yield return null;
        }

    }
}
