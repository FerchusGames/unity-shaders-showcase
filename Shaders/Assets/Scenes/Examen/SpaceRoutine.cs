using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpaceRoutine : MonoBehaviour {

    [SerializeField] Renderer _meshRenderer = default;
    Color _currentColor = default;
    
    float _hue = 0;
    
    bool _isToon = false;
    bool _isTransparent = false;
    bool _isVisible = false;
    
    bool _canTransition = true;
    
    private void Awake() {
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<Renderer>();
        
        _currentColor = Color.HSVToRGB(0, 1, 1);
        _isToon = _meshRenderer.sharedMaterial.GetFloat("_PBR_Toon_Lerp") > 0;
        _isTransparent = _meshRenderer.sharedMaterial.GetFloat("_Transition_Value") > 0;
        _isVisible = _meshRenderer.sharedMaterial.GetFloat("_Visibility") > 0;
    }

    private void Update() {
        ChangeHue();
    }

    private void ChangeHue() {
        _hue += Time.deltaTime*0.1f;
        if (_hue >= 1) _hue = 0;
        _currentColor = Color.HSVToRGB(_hue, 1, 1);
        _meshRenderer.sharedMaterial.SetColor("_Base_Color", _currentColor);
    }

    [ContextMenu("PBR Toon Switch")]
    public void PBRToonSwitch() {
        Transition(ref _isToon, "_PBR_Toon_Lerp");
    }
    
    [ContextMenu("Transition")]
    public void OpaqueTransparentSwitch() {
        Transition(ref _isTransparent, "_Transition_Value");
    }

    [ContextMenu("Invisibility Switch")]
    public void VisibilitySwitch() {
        Transition(ref _isVisible, "_Visibility");
    }
    
    private void Transition(ref bool state, string key) {
        if (_canTransition)
        {
            StartCoroutine(MaterialValueLerp(state, key));
            state = !state;
        }
    }
    private IEnumerator MaterialValueLerp(bool state, string key)
    {
        _canTransition = false;
        float elapsedTime = 0;
       
        int direction = state ? -1 : 1;
        int finalValue = state ? 0 : 1;
        
        while (elapsedTime < 1) {
            elapsedTime += Time.deltaTime;
            _meshRenderer.sharedMaterial.SetFloat(key,
                _meshRenderer.sharedMaterial.GetFloat(key) + Time.deltaTime * direction);
            yield return null;
        }
        _meshRenderer.sharedMaterial.SetFloat(key, finalValue);
        _canTransition = true;
    }
}
