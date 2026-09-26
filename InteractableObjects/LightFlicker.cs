using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light myLight;
    //public Light redLight;
    
    public float maxWait = 1;
    public float maxFlicker = 0.1f;

    //float Redinterval = 1;
    //float Redtimer;
    
    float timer;
    float interval;

    void Update()
    {
        //Redtimer += Time.deltaTime;
        //if (Redtimer > Redinterval)
        //{
           // redLight.enabled = !redLight.enabled;
           // Redinterval = Random.Range(0.5f, 0.5f);
           // Redtimer = 0;
        //}
        timer += Time.deltaTime;
        if (timer > interval)
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        myLight.enabled = !myLight.enabled;
        if (myLight.enabled)
        {
            interval = Random.Range(0, maxWait);
        }
        else 
        {
            interval = Random.Range(0, maxFlicker);
        }
    
        timer = 0;
    }
    

}