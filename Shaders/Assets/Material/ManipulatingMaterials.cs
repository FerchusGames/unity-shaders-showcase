using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulatingMaterials : MonoBehaviour
{
    MaterialPropertyBlock mtp;
    public Renderer meshRenderer = default;
    // renderer.sharedmaterial.SetFloat o SetColor o SetTexture modifica el material principal
    // renderer.material.Set_ modifica la instancia pero crea una copia del material automaticamente
    // material property block solo modifica los valores sin duplicar el material

    private void Awake()
    {
        if(meshRenderer==null)
            meshRenderer = GetComponent<Renderer>();

        mtp = new MaterialPropertyBlock();
    }

    [ContextMenu("Iniciar Transicion")]
    public void EnableTransition()
    {
        StartCoroutine(DoTransition(2.5f,true));
    }

    [ContextMenu("Deshacer Transicion")]
    public void DisableTransition()
    {
        StartCoroutine(DoTransition(2.5f, false));
    }

    public IEnumerator DoTransition(float _t, bool _forward)
    {
        float t = 0;
        float dt = 1 / _t; //1/10

        while (t < 1)
        {
            t += dt*Time.deltaTime;

            if (_forward)           
                mtp.SetFloat("_Transition_Value", t);                            
            else            
                mtp.SetFloat("_Transition_Value", 1-t);                
            
            mtp.SetFloat("_Transition_Gradient", 1 - t);
            meshRenderer.SetPropertyBlock(mtp);
            yield return null;
        }
    }


}