using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]

public class CRTShaderScript : MonoBehaviour {
    public Shader shader;
    public float Distortion = 0.1f;
    public float BleedDistance = 0.05f;
    public float BleedWeightRed = 0.6f;
    public float BleedWeightBlue = 0.6f;
    private Material _material;

    protected Material material
    {
        get
        {
            if (_material == null)
            {
                _material = new Material(shader);
                _material.hideFlags = HideFlags.HideAndDontSave;
            }
            return _material;
        }
    }

    void Start()
    {
        if(!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (shader != null)
        {
            material.SetFloat("_Distortion", Distortion);
            material.SetFloat("_BleedDistance", BleedDistance);
            material.SetFloat("_BleedWeightRed", BleedWeightRed);
            material.SetFloat("_BleedWeightBlue", BleedWeightBlue);
            material.SetVector("_TextureSize", new Vector2(512.0f, 512.0f));
            Graphics.Blit(source, destination, material);
        } else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void OnDisable()
    {
        if (_material)
        {
            DestroyImmediate(_material);
        }
    }
}
