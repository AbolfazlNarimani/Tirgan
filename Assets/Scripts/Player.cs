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

    private List<GameObject> _targetEnemyList;
    public float predictionFactor = 0.5f;
    public float heightOffset = 1.5f;

    public event EventHandler OnEnemiesDied;
    public event EventHandler StopMoving;
    public event EventHandler ShootStage;
    private HealthSystem _healthSystem;
    private float _stopMovingDistance;
    private GameObject _currentArrow;
    private int _currentTargetIndex = 0;
    private bool _isShooting = false;

    private enum State
    {
        Walking,
        Shooting,
        EnemyTurn
    }

    private State _state;

    private void Awake()
    {
        Instance = this;
        _state = State.Walking;
    }

    private void Start()
    {
        if (TryGetComponent(out MovePlayerToNextWave movePlayerToNextWave))
        {
            movePlayerToNextWave.InitialMove();
            _stopMovingDistance = 0;
        }

        _targetEnemyList = new List<GameObject> { GameObject.FindGameObjectWithTag("Enemy")};
        Debug.Log(_targetEnemyList);
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDead += HealthSystemOnOnDead;
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
            case State.Walking:
                if (_targetEnemyList.Count < 0)
                {
                    _stopMovingDistance = 24;
                    OnEnemiesDied?.Invoke(this, EventArgs.Empty);
                    ResetTargetingSystem();
                }
                else if (!_isShooting)
                {
                    _state = State.Shooting;
                }

                break;

            case State.Shooting:
                if (!_isShooting)
                {
                    _targetEnemyList.Clear();
                    _targetEnemyList = new List<GameObject> { GameObject.FindGameObjectWithTag("Enemy") };
                    StartShootingSequence();
                }

                break;

            case State.EnemyTurn:
                // Enemy turn logic here
                _state = State.Walking;
                break;
        }
    }

    private void StartShootingSequence()
    {
        _isShooting = true;
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
        GameObject targetEnemy = _targetEnemyList[_currentTargetIndex];
        if (targetEnemy == null)
        {
            _state = State.Walking;
            return;
        }
        _currentArrow.transform.SetParent(null);
       
        Vector3 enemyVelocity = targetEnemy.GetComponent<Rigidbody>()?.linearVelocity ?? Vector3.zero;
        Vector3 predictedPos = targetEnemy.transform.position +
                               Vector3.up * heightOffset +
                               enemyVelocity * predictionFactor;

        _currentArrow.transform.DOMove(predictedPos, 1f).OnComplete(() =>
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
        Debug.Log("enemy.name = " + enemy.name);
        enemy.TakeDamage(50);

        if (_currentTargetIndex < _targetEnemyList.Count - 1)
        {
            _currentTargetIndex++;
        }
        else
        {
            ResetTargetingSystem();
            _state = State.EnemyTurn;
        }
    }

    private void HealthSystemOnOnDead(object sender, EventArgs e)
    {
        if (true)
        {
            GameObject enemyToRemove = _targetEnemyList[_currentTargetIndex];
            _targetEnemyList.Remove(enemyToRemove);
            Destroy(enemyToRemove);

            _currentTargetIndex = Mathf.Clamp(_currentTargetIndex, 0, _targetEnemyList.Count - 1);
        }
    }

    private void ResetTargetingSystem()
    {
        _currentTargetIndex = 0;
        _healthSystem = null;
        _isShooting = false;


        // targetEnemyList = GetNewEnemies();
    }

    public bool HasValidTarget()
    {
        return _currentTargetIndex < _targetEnemyList.Count;
    }
}