using System;
using System.Collections;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    
    
    public static TurnSystem Instance { get; private set; }
    
    public event EventHandler OnTurnNumberChanged;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(Turn());
    }

    public void NextTurn()
    {
        OnTurnNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator Turn()
    {
        while (true)
        {
            yield return ExecutePlayerTurn();
            yield return ExecuteEnemyTurn();
            
            // check if last room
            if (true)
                yield return WaitForUpgrade();
            
            yield return null;
        }
    }

    private IEnumerator ExecutePlayerTurn()
    {
        yield return Player.Instance.StartShootingSequence();
    }

    private IEnumerator ExecuteEnemyTurn()
    {
        yield break;
    }

    private IEnumerator WaitForUpgrade()
    {
        yield break;
    }
}
