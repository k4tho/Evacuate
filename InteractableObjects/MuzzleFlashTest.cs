using UnityEngine;

public class MuzzleFlashTest : MonoBehaviour
{
    public ParticleSystem flashEffect;
    public ParticleSystem smokeEffect;

    
    public void PlayEffect()
    {
        flashEffect.Play();
        smokeEffect.Play();
    }
    
}