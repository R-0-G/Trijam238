using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Animator animator;
    [SerializeField] string namein;
    [SerializeField] string nameout;

    private void Awake()
    {
        Game.onGameStart += HanleGameStart;
        Game.onGameStop += HandleGameStop;
    }


    private void Start()
    {
        animator.Play(namein);
    }
    private void OnDestroy()
    {
        Game.onGameStart -= HanleGameStart;
        Game.onGameStop -= HandleGameStop;
    }
    private void HandleGameStop()
    {
        animator.Play(namein);
    }

    private void HanleGameStart()
    {
        animator.Play(nameout);
    }
}
