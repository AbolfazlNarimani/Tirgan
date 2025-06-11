using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{

    [FormerlySerializedAs("_player")] [SerializeField] private Player player;
    private enum State
    {
        WaitingEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State _state;
    private float _timer;
    private int _currentEnemyIndex;
    private bool _hasEnemyActedThisCycle;

    private void Awake()
    {
        _state = State.WaitingEnemyTurn;
        _currentEnemyIndex = 0;
    }

    private void Start() 
    {
        GetComponent<HealthSystem>().OnDead += OnOnDead;
        TurnSystem.Instance.OnTurnNumberChanged += OnTurnChanged;
    }

    private void OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            _state = State.TakingTurn;
            _currentEnemyIndex = 0;
            _timer = 0.5f;
            _hasEnemyActedThisCycle = false;
        }
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;
        if (TurnSystem.Instance.IsPlayerShooting()) return; // prevent enemy moving while player is shooting
        switch (_state)
        {
            case State.WaitingEnemyTurn:
                break;

            case State.TakingTurn:
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    ProcessEnemyActions();
                }

                break;

            case State.Busy:
                // Waiting for current action to complete
                break;
        }
    }
    
    private void ProcessEnemyActions()
    {
        var enemyUnits = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        // If we've processed all enemies, check if any can still act
        if (_currentEnemyIndex >= enemyUnits.Length)
        {
            if (_hasEnemyActedThisCycle)
            {
                _currentEnemyIndex = 0;
                _hasEnemyActedThisCycle = false;
                _timer = 0.1f;
                return;
            }
            else
            {
                TurnSystem.Instance.NextTurn();
                _state = State.WaitingEnemyTurn;
                return;
            }
        }

        if (TryTakeEnemyAIAction(OnEnemyActionComplete))
        {
            _state = State.Busy;
            _hasEnemyActedThisCycle = true;
        }
        else
        {
            // This enemy can't act right now, try next one
            _currentEnemyIndex++;
            _timer = 0.1f;
        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAiActionComplete)
    {
        Enemy[] enemyList = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        bool anyActionTaken = false;

        foreach (Enemy enemy in enemyList)
        {
            if (enemy == null || enemy.GetComponent<HealthSystem>().IsDead()) continue;

            if (TryGettingToThePlayer())
            {
               enemy.transform.DOMove(player.transform.position, 2f);
                anyActionTaken = true;
            }
            else
            {
                AttackThePlayer();
                anyActionTaken = true;
            }
        }

        return anyActionTaken;
    }

    private bool TryGettingToThePlayer()
    {
        this.transform.DOMoveZ(3, 2f);
        return true;
    }

    private void AttackThePlayer()
    {
        transform.DOMove(new Vector3(0, 0, 3), 2f);
    }

    private void OnOnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
    
    private void OnEnemyActionComplete()
    {
        // Action complete, allow next enemy to act
        _timer = 0.1f;
        _state = State.TakingTurn;
    }
}