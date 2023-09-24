using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField] private Transform ballRoot;
    [SerializeField] private Transform chainEnd;
    [SerializeField] private Transform chainEnd2;

    [SerializeField] private float minDist = 0.5f;
    [SerializeField] private float maxDist = 2f;
    
    private void Update()
    {
        Transform target = chainEnd.gameObject.activeInHierarchy ? chainEnd : chainEnd2;
        transform.position = target.position;
        transform.LookAt(ballRoot);
        float size = Vector2.Distance(ballRoot.position, target.position);

        if (size > maxDist)
        {
            if (Game.gameActive)
            {
                Game.TriggerBeginGameStop();
            }
        }
        
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,size);
        
    }
}
