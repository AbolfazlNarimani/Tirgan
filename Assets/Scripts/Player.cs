using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Arrow Settings")] public GameObject arrowPrefab;
    public Transform arrowNockPoint;
    public float shootPower = 40f;
    public static Player Instance { private set; get; }

    public float predictionFactor = 0.5f;
    public float heightOffset = 1.5f;

    public event EventHandler OnEnemiesDied;
    public event EventHandler StopMoving;
    public event EventHandler ShootStage;
    
    private List<Enemy> _targetEnemyList = new List<Enemy>();
    private HealthSystem _healthSystem;
    private float _stopMovingDistance;
    private GameObject _currentArrow;
    private int _currentTargetIndex = 0;
    private bool _isShooting = false;

    private enum State
    {
        WaitingPlayerTurn,
        Walking,
        Shooting,
    }

    private State _state;

    private void Awake()
    {
        Instance = this;
        _state = State.Walking;
    }

    private void Start()
    {
        Debug.Log("player start");
        if (TryGetComponent(out MovePlayerToNextWave movePlayerToNextWave))
        {
            movePlayerToNextWave.InitialMove();
            _stopMovingDistance = 0;
        }

        _healthSystem = GetComponent<HealthSystem>();
        Arrow.EnemyHit += ArrowOnEnemyHit;
    }

    private void Update()
    {
        if (transform.position.z == _stopMovingDistance)
        {
            StopMoving?.Invoke(this, EventArgs.Empty);
        }

        switch (_state)
        {
            case State.WaitingPlayerTurn:
            {
                if(TurnSystem.Instance.IsPlayerTurn())
                {
                    _state = State.Shooting;
                    Debug.Log("State is changed form wating player turn to shooting");
                }
                
                break;
            }
            case State.Walking:
                if (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length == 0)
                {
                    _stopMovingDistance = 24;
                    OnEnemiesDied?.Invoke(this, EventArgs.Empty);
                    ResetTargetingSystem();
                }
                else if (!_isShooting)
                {
                    Debug.Log("state has changed from walking to shooting");
                    _state = State.Shooting;
                  
                }

                break;

            case State.Shooting:
                if (!_isShooting)
                {
                    TurnSystem.Instance.SetPlayerShooting(true);
                    StartShootingSequence();
                }
                Debug.Log("state is set to shooting");
                break;
        }
    }

    private void StartShootingSequence()
    {
        _isShooting = true;
        _targetEnemyList.Clear();
        foreach (Enemy enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            if (enemy != null && !enemy.GetComponent<HealthSystem>().IsDead())
            {
                _targetEnemyList.Add(enemy);
            }
        }
        ShootStage?.Invoke(this, EventArgs.Empty);
    }

    public void NockArrow()
    {
        if (_currentArrow == null && _currentTargetIndex < _targetEnemyList.Count)
        {
            Quaternion arrowRotation = arrowNockPoint.rotation * Quaternion.Euler(90, 0, 0);
            _currentArrow = Instantiate(arrowPrefab, arrowNockPoint.position, arrowRotation);
            _currentArrow.transform.SetParent(arrowNockPoint);
            _currentArrow.transform.localPosition = Vector3.zero;
            _currentArrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void ReleaseArrow()
    {
        if (_currentArrow == null || _currentTargetIndex >= _targetEnemyList.Count) return;

        Enemy targetEnemy = _targetEnemyList[_currentTargetIndex];
        if (targetEnemy == null)
        {
            _state = State.Walking;
            return;
        }

        _currentArrow.transform.SetParent(null);

        Vector3 enemyVelocity = targetEnemy.GetComponent<Rigidbody>()?.linearVelocity ?? Vector3.zero;
        Vector3 predictedPos = targetEnemy.transform.position + Vector3.up * heightOffset + enemyVelocity * predictionFactor;

        _currentArrow.transform.DOMove(predictedPos, 0.5f).OnComplete(() =>
        {
            if (!_currentArrow.TryGetComponent<Arrow>(out _))
            {
                _currentArrow.AddComponent<Arrow>();
            }
        });

        _currentArrow = null;
        _isShooting = false;
    }

    private void ArrowOnEnemyHit(HealthSystem enemy)
    {
        enemy.TakeDamage(50);

        if (_currentTargetIndex < _targetEnemyList.Count - 1)
        {
            _currentTargetIndex++;
        }
        else
        {
            TurnSystem.Instance.SetPlayerShooting(false);
            TurnSystem.Instance.NextTurn();
            _state = State.WaitingPlayerTurn;
            ResetTargetingSystem();
        }
    }


    private void ResetTargetingSystem()
    {
        _currentTargetIndex = 0;
        _isShooting = false;
        _targetEnemyList.Clear();
    }

    public bool HasValidTarget()
    {
        return _currentTargetIndex < FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
    }
}