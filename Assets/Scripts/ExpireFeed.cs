using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpireFeed : MonoBehaviour
{
    float expireTime = 5f;
    
    void OnEnable()
    {
        Destroy(gameObject, expireTime);
    }
}
