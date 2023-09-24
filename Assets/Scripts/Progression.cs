using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Progression : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    [SerializeField] private Transform endTrans;
    [SerializeField] private Transform startTrans;
    [SerializeField] private TextMeshProUGUI text;

    private bool start =  false;
    private float fullDistance;
    private float currentDistance;

    private void Start()
    {
        fullDistance = Vector2.Distance(startTrans.position, endTrans.position);
    }
    private void Update()
    {
        Game.progression = 1f-Mathf.Clamp01(Vector2.Distance(playerTrans.position, endTrans.position) / fullDistance);
        text.text = Game.progression * 100f + "% Progression";
    }
}
