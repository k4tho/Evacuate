using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbobSystem : MonoBehaviour {

    [Range(0.005f, 0.05f)]
    public float Amount = 0.0075f;
    [Range(1f, 30f)]

    public float Frequency = 10.0f;

    [Range(10f, 100f)]
    public float Smooth = 10.0f;

    Vector3 StartPos;

    void Start()
    {
        StartPos = transform.localPosition;
    }

    void Update()
    {
        StopHeadbob();
    }

    public Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
        transform.localPosition += pos;

        return pos;
    }

    private void StopHeadbob()
    {
        if (transform.localPosition == StartPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);
    }

    public void FastBob()
    {
        Amount = 0.018f;
        Frequency = 16.0f;
        Smooth = 10.0f;
    }

    public void NormBob()
    {
        Amount = 0.01f;
        Frequency = 9.0f;
        Smooth = 10.0f;
    }

    public void SlowBob()
    {
        Amount = 0.0075f;
        Frequency = 6.0f;
        Smooth = 10.0f;
    }
}