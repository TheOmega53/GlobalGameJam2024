using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTriggerEvent : MonoBehaviour
{

    public UnityEvent unityEvent;    

    // Update is called once per frame
    public void InvokeEvent()
    {
        unityEvent.Invoke();
    }
}
