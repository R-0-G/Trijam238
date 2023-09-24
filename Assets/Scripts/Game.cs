using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public delegate void GameEvent();
    public delegate void MessageEvent(string msg);
    public delegate void MessageAudioEvent(string msg, AudioClip clip);

    public static event GameEvent onGameStart;
    public static event GameEvent onGameStop;
    public static event GameEvent onBeginGameStop;
    public static event GameEvent onGameWin;
    public static event MessageEvent onMessage;
    public static event MessageAudioEvent onComplain;
    
    public static bool gameActive = false;
    public static float progression = 0f;
    
    public static void TriggerGameWin()
    {
        TriggerGameStop();
        PlayerPrefs.SetInt("WON" ,1);
        onGameWin?.Invoke();
    }

    public static void TriggerGameStart()
    {
        gameActive = true;
        progression = 0f;
        onGameStart?.Invoke();
    }
    
    public static void TriggerGameStop()
    {
        gameActive = false;
        onGameStop?.Invoke();
    }
    
    public static void TriggerBeginGameStop()
    {
        // gameActive = false;
        onBeginGameStop?.Invoke();
    }

    public static void TriggerMessage(string message)
    {
        onMessage?.Invoke(message);
    }
    
    public static void TriggerComplaint(string message, AudioClip clip)
    {
        onComplain?.Invoke(message, clip);
    }
}
