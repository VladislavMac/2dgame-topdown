using UnityEngine;
using UnityEngine.AI;

using CustomInterface;

public abstract class BaseEnemy : MonoBehaviour, IShooter, IEntity
{
    public EnemyStatus Status;
    public Vector3 LastPlayerPosition;
    public float Hp;

    protected GameObject _player;
    protected NavMeshAgent _aiAgent;

    [SerializeField] protected int _aim = 1;

    [SerializeField] protected GameObject _body;
    [SerializeField] protected GameObject _hands;
    [SerializeField] protected GameObject _trigger;
    //[SerializeField] protected GameObject _bloodEffect;

    [SerializeField] private bool _enemyPatrol = false;
    [SerializeField] protected LayerMask _layersWhichRaycastSee;


    protected float _visionLength = 13f;
    protected float _distanceFromPlayer;

    protected HandController _handsController;

    private float _cooldown = 0;
    private float _cooldownAfterSeePlayer = 0.4f;

    private float _idleCooldown = 0;
    private float _idleCooldownStart = 1;

    private Vector3 _patrolPosition;
    public float _patrolRotation;

    private Transform _huntObject;

    /* ---------------- Set Settings ---------------- */

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _handsController = _hands.GetComponent<HandController>();
        _cooldown = _cooldownAfterSeePlayer;
        _idleCooldown = UnityEngine.Random.Range(0, _idleCooldownStart);

        _trigger.GetComponent<EnemyTrigger>().EnemyGameObject = this.gameObject;

        SetHandsOwner(_handsController, this.gameObject);

        if (_enemyPatrol)
        {
            _patrolPosition = transform.position;
            _patrolRotation = transform.rotation.eulerAngles.z;

            Status = EnemyStatus.Patrol;
        }
        else
        {
            Status = EnemyStatus.Idle;
        }

        AISetSettings();
    }

    protected void AISetSettings()
    {
        _aiAgent = GetComponent<NavMeshAgent>();
        _aiAgent.enabled = true;
        _aiAgent.updateRotation = false;
        _aiAgent.updateUpAxis = false;
    }

    protected void UpdateDistanceFromPlayer()
    {
        _distanceFromPlayer = Vector2.Distance(transform.position, _player.transform.position);
    }

    /* ---------------- Realization Interface ---------------- */

    public void SetHandsOwner(HandController handsController, GameObject shooter)
    {
        handsController.Owner = shooter;
    }

    public void Die()
    {
        Destroy(gameObject);
        _player.GetComponent<PlayerController>().AddKill();
    }

    public void Shoot()
    {
        int aim = _aim * 2;

        if (_cooldown <= 0)
        {
            if (UnityEngine.Random.Range(0, aim + 1) == aim)
            {
                _handsController.HandWeapon.Shoot();
            }
        }
        else
        {
            _cooldown -= Time.deltaTime;
        }
    }

    public void HitEntity(float damage)
    {
        Hp -= damage;
        if (Hp <= 0) Die();
        //Instantiate(_bloodEffect, transform.position, Quaternion.identity, transform);
    }

    /* ---------------- Controll Methods ---------------- */

    protected void GoToPosition(Vector3 needPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, needPosition, _aiAgent.speed * Time.deltaTime);
    }

    protected void FollowLookAtPosition(Vector3 needPosition)
    {
        Vector3 direction = needPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        this.GetComponent<Rigidbody2D>().rotation = angle;
    }
    protected void FollowLookAtPosition(float needRotate)
    {
        this.GetComponent<Rigidbody2D>().rotation = needRotate;
    }

    /* ---------------- AI Methods ---------------- */

    protected void AIGoToPosition(Vector3 needPosition)
    {
        _aiAgent.destination = needPosition;

        if (_aiAgent.remainingDistance <= _aiAgent.stoppingDistance)
        {
            _aiAgent.isStopped = true;
            Status = EnemyStatus.Idle;
        }
    }

    /* ---------------- AI Logic ---------------- */

    protected void AILogic()
    {
        if (Status != EnemyStatus.Hunt) AIHaveLookout();

        switch (Status)
        {
            case EnemyStatus.Idle:
                AIIdle();
                break;

            case EnemyStatus.Hunt:
                AIHunt();
                break;

            case EnemyStatus.Patrol:
                AIIdle();
                break;

            case EnemyStatus.Search:
                AISearch();
                break;
        }
    }

    private void AIHaveLookout()
    {
        RaycastHit2D rayCastToPlayer;

        // Если игрок игрок за спиной
        if (_distanceFromPlayer < _visionLength / 2)
        {
            // Игрок наглеет! Включить интуицию
            rayCastToPlayer = Physics2D.Raycast(transform.position, _player.transform.position - transform.position, _visionLength, _layersWhichRaycastSee);
            FollowLookAtPosition(_player.transform.position);
        }
        else
        {
            // Игрок не за спиной
            rayCastToPlayer = Physics2D.Raycast(_hands.transform.position, _player.transform.position - transform.position, _visionLength);
        }

        // Если видимый объект не пустота
        if (rayCastToPlayer.collider != null)
        {
            // Если есть зрительный контакт с игроком
            if (rayCastToPlayer.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Если дистанция до игрока в пределах видимости
                if (_distanceFromPlayer < _visionLength)
                {
                    Status = EnemyStatus.Hunt;
                    _huntObject = rayCastToPlayer.collider.transform;
                }
            }
        }
    }

    /* ---------------- AI State ---------------- */

    private void AIIdle()
    {
        float rotate = transform.eulerAngles.z;
        _cooldown = _cooldownAfterSeePlayer;

        // кулдаун перед поворотом
        if (_idleCooldown <= 0)
        {
            // Если бот - патрульный
            if (_patrolPosition != Vector3.zero)
            {
                if (transform.position.x != _patrolPosition.x && transform.position.y != _patrolPosition.y)
                {
                    _aiAgent.isStopped = false;
                    AIGoToPosition(_patrolPosition);
                    FollowLookAtPosition(_patrolPosition);
                }
                else
                {
                    FollowLookAtPosition(_patrolRotation);
                }
                return;
            }

            // небольшая проверка - пялиться ли бот на что то 
            RaycastHit2D rayCastToPlayer;
            rayCastToPlayer = Physics2D.Raycast(_hands.transform.position, _player.transform.position - transform.position, 4);

            if (rayCastToPlayer.collider != null)
            {
                rotate += 15;
            }
            else
            {
                rotate += UnityEngine.Random.Range(-15, 15);
            }

            _idleCooldown = UnityEngine.Random.Range(0, _idleCooldownStart);
        }
        else
        {
            _idleCooldown -= Time.deltaTime;
        }

        this.GetComponent<Rigidbody2D>().rotation = rotate;
    }

    private void AIHunt()
    {

        RaycastHit2D rayCastToPlayer;
        rayCastToPlayer = Physics2D.Raycast(_hands.transform.position, _player.transform.position - transform.position, _visionLength);
        
        // Если последняя позиция игрока не задана - создать
        LastPlayerPosition = _huntObject.position;
        //_trigger.GetComponent<EnemyTrigger>().Warning(LastPlayerPosition);

        // Если видимый объект не пустота
        if (rayCastToPlayer.collider != null)
        {
            // Если есть зрительный контакт с игроком
            if (rayCastToPlayer.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Если дистанция до игрока в пределах видимости
                if (_distanceFromPlayer < _visionLength)
                {
                    _huntObject = rayCastToPlayer.collider.transform;

                    FollowLookAtPosition(_player.transform.position);

                    // Охота на игрока в прямом эфире 
                    if (_distanceFromPlayer >= 3f)
                    {
                        _aiAgent.isStopped = true;
                        GoToPosition(_player.transform.position);
                    }

                    Shoot();
                }
            }
            else
            {
                Status = EnemyStatus.Search;
            }
        }
    }

    private void AISearch()
    {

        _aiAgent.isStopped = false;
        AIGoToPosition(LastPlayerPosition);
        FollowLookAtPosition(LastPlayerPosition);
    }
}
