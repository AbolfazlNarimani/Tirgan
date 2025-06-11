using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    
    
    public static TurnSystem Instance { get; private set; }
    
    public event EventHandler OnTurnNumberChanged;
    
    private bool _isPlayerTurn = true;
    private bool _isPlayerShooting = true;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void NextTurn()
    {
        _isPlayerTurn = !_isPlayerTurn;
        OnTurnNumberChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public bool IsPlayerTurn() => _isPlayerTurn;
    public bool IsPlayerShooting() => _isPlayerShooting;
    public void SetPlayerShooting(bool playerShooting) => _isPlayerShooting = playerShooting;
}
