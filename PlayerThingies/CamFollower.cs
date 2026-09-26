using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    public Transform target;

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 5f * Time.deltaTime);
    }
}
