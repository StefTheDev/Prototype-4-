using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Timer timer;
    //private List<Player> players;

   
    public bool IsGameOver()
    {
        // return (timer.GetTime() < 0.0f) && players.Count < 0;
        return false;
    }
}
