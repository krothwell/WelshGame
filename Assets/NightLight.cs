using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  GameUtilities.Display;
public class NightLight : MonoBehaviour {
    ParticleSystemRenderer lightingParticleEffect;
    Light lightSource;

    void Awake() {
        lightSource = GetComponentInChildren<Light>();
        lightingParticleEffect = GetComponentInChildren<ParticleSystemRenderer>();
        SetParticleLayer();
    }

    public void TurnOn() {
        lightingParticleEffect.gameObject.SetActive(true);
        lightSource.gameObject.SetActive(true);
    }

    public void TurnOff() {
        lightingParticleEffect.gameObject.SetActive(false);
        lightSource.gameObject.SetActive(false);
    }

    public void SetParticleLayer() {
        if (lightingParticleEffect != null) {
            ImageLayerOrder.SetOrderOnParticleSystem(lightingParticleEffect, ImageLayerOrder.GetOrderInt(gameObject));
        }
    }
}
