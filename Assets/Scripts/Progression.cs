using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Progression : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    [SerializeField] private Transform endTrans;
    [SerializeField] private Transform startTrans;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float threshold = 0.01f;
    [SerializeField] private TMP_Text message;
    [SerializeField] private TMP_Text complain;

    private readonly List<string> messageList = new List<string> { "You are wasting your time", "You are doomed to fail", "Stop already..", "Stop embarrassing yourself", "This will not end well for you", "Why do you even bother?", "You are beyond help", "Looser.." };
    private readonly List<string> complainList = new List<string> { "I'm exhausted", ". . .", "Wish it'd stop", "No happiness", "Just going through motions", "Always tired", "I can't deal", "No point anymore", "Just want to escape", "I'm so alone", "No light in sight", "Why me?", "I'm just existing", "It's driving me crazy", "I'm a mess inside", "It's so unfair!" };
    private bool start = false;
    private float fullDistance;
    private float currentDistance;
    private Stopwatch timer = new Stopwatch();


    private void Awake()
    {
        Game.onGameStart += HandleGameStart;
    }
    private void Start()
    {
        fullDistance = Vector2.Distance(startTrans.position, endTrans.position);
    }
    private void Update()
    {
        Game.progression = 1f - Mathf.Clamp01(Vector2.Distance(playerTrans.position, endTrans.position) / fullDistance);
        text.text = Mathf.FloorToInt(Game.progression * 100f) + "% Progression";

        if (Mathf.Abs(Game.progression - 1f) < threshold)
        {
            Game.TriggerGameWin();
        }
        if (timer.Elapsed.Seconds > 9 && timer.Elapsed.Seconds < 10)
        {
            int index = UnityEngine.Random.Range(0, complainList.Count);
            complain.text = complainList[index];
        }
        if (timer.Elapsed.Seconds >= 30)
        {

            int index = UnityEngine.Random.Range(0, messageList.Count);
            message.text = messageList[index];
            timer.Restart();
        }
    }

    private void OnDestroy()
    {
        Game.onGameStart -= HandleGameStart;
    }

    private void HandleGameStart()
    {
        timer.Start();
    }
}
