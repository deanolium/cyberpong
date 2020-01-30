using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    Vector3 originalPosition;
    Quaternion originalRotation;
    float shakeIntensity;
    bool isShaking;
    Transform cameraTransform;

    public float shakeDecay = 0.006f;
    public float startingIntensity = 0.04f;

	// Use this for initialization
	void Start () {
        cameraTransform = Camera.main.transform; // Reference the transform so we can screw with it later	
	}
	
	// Update is called once per frame
	void Update () {
		if (isShaking)
        {
            HandleCameraShake();
        }
	}

    void HandleCameraShake()
    {
        if (shakeIntensity > 0f)
        {
            // keep shaking
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeIntensity;
            cameraTransform.localRotation = new Quaternion(
                originalRotation.x + Random.Range(-shakeIntensity, shakeIntensity) * .2f,
                originalRotation.y + Random.Range(-shakeIntensity, shakeIntensity) * .2f,
                originalRotation.z + Random.Range(-shakeIntensity, shakeIntensity) * .2f,
                originalRotation.w + Random.Range(-shakeIntensity, shakeIntensity) * .2f);
            shakeIntensity -= shakeDecay;
        } else
        {
            // shaking is finished
            isShaking = false;
            cameraTransform.localPosition = originalPosition;
            cameraTransform.localRotation = originalRotation;                
        }
    }

    public void Shake()
    {
        if (!isShaking)
        {
            originalPosition = cameraTransform.localPosition;
            originalRotation = cameraTransform.localRotation;
        }

        isShaking = true;
        shakeIntensity = startingIntensity;        
    }
}
