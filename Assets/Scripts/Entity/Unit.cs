using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Unit : MonoBehaviour
{
    public string Name;
    public int Health;
    public float Speed;
    public int AttackDamage;
    [SerializeField] protected bool IsDead = false;
    [SerializeField] protected bool IsInDanger = false;

    [SerializeField] protected AudioSource attackSound;
    [SerializeField] protected AudioSource footstepSound;
    [SerializeField] protected AudioSource deathSound;
    [SerializeField] protected AudioSource takeHitSound;
    [SerializeField] protected AudioSource spawnSound;
    [SerializeField] protected AudioSource disappearanceSound;
    
    //to find the speed of an object
    [SerializeField] protected Vector3 currentSpeed;
    [SerializeField] protected Vector3 lastPosition;
    protected bool needToCheckWalkingAnimation;

    public enum OnBorder { LEFT, RIGHT, UNSELECTED };
    [SerializeField] public OnBorder onBorder = OnBorder.UNSELECTED;
    [SerializeField] public bool IsOnBorder;
    [SerializeField] public GameObject Border;

    //attack
    public Transform AttackPoint;
    public LayerMask EnemyLayer;
    public LayerMask PlayerLayer;
    public LayerMask BuildingLayer;
    public LayerMask RoadLampLayer;
    [SerializeField] protected int AttackVariations;
    [SerializeField] protected float AttackRange;
    [SerializeField] protected float AttackRate;
    [SerializeField] protected float NextAttackTime = 0f;
    protected bool IsTakingDamage = false;

    //find targets
    [SerializeField] protected GameObject[] TargetsArray;
    [SerializeField] protected GameObject Target;
    [SerializeField] protected float StopRange;
    [SerializeField] protected float Directory = 0;

    protected Animator Animator;
    protected Rigidbody2D Rigidbody;

    [SerializeField] private byte _dropsCount;

    protected virtual void Initialize(int health, int attackDamage, float speed,
              float attackRange, float attackRate, float stopRange, string name)
    {
        Health = health;
        AttackDamage = attackDamage;
        Speed = speed;
        AttackRange = attackRange;
        AttackRate = attackRate;
        StopRange = stopRange;
    }
    protected virtual void Behaviour()
    {
        if (IsDead != true)
        {
            if (Target != null)
            {
                if (!Target.gameObject.CompareTag("Dead"))
                {
                    if (Vector2.Distance(transform.position, Target.transform.position) > StopRange)
                        transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);
                    
                    if (Vector2.Distance(transform.position, Target.transform.position) <= StopRange)
                    {
                        if (Time.time >= NextAttackTime && IsTakingDamage == false)
                        {
                            Attack();
                            NextAttackTime = Time.time + 1f / AttackRate;
                        }
                    }
                }
            }
        }
    }

    protected void SetWalkingAnimation()
    {
        currentSpeed = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
                        
        if (Math.Abs(currentSpeed.x) >= 0.2f)
            Animator.SetBool("IsWalking", true);
        else
            Animator.SetBool("IsWalking", false);
    }
    public void Attack()
    {
        int variation = Random.Range(1, (AttackVariations + 1));
        switch (variation)
        {
            case 1:
                Animator.SetTrigger("Attack");
                break;
            case 2:
                Animator.SetTrigger("Attack2");
                break;
            default:
                Animator.SetTrigger("Attack");
                break;
        }
        PlaySound(attackSound);
        // to deal damage we have another function DealDamage below which we call in the animation event
    }
    public void DealDamage()
    {
        if (EnemyLayer != 0)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayer);

            foreach (Collider2D enemy in hitEnemies)
                enemy.GetComponent<Unit>().TakeDamage(AttackDamage);
        }
        if (PlayerLayer != 0)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, PlayerLayer);

            foreach (Collider2D enemy in hitPlayer)
                enemy.GetComponent<Unit>().TakeDamage(AttackDamage);
        }
        if (BuildingLayer != 0)
        {
            Collider2D[] hitBuilding = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, BuildingLayer);

            foreach (Collider2D building in hitBuilding)
                building.GetComponent<Buildings>().TakeDamage(AttackDamage);
        }
        if (RoadLampLayer != 0)
        {
            Collider2D[] hitRaodLamp = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, RoadLampLayer);

            foreach (Collider2D roadLamp in hitRaodLamp)
                roadLamp.GetComponent<Buildings>().TakeDamage(AttackDamage);
        }
    }

    public void PlayFootstepSound()
    {
        footstepSound.volume = Random.Range(0.9f, 1);
        footstepSound.pitch = Random.Range(1.1f, 1.3f);
        footstepSound.Play();
    }
    private void PlaySound(AudioSource sound)
    {
        sound.volume = Random.Range(0.9f, 1);
        sound.pitch = Random.Range(0.9f, 1.2f);
        sound.Play();
    }
    protected void PlayAppearOrDisappearSound(AudioSource sound)
    {
        sound.volume = Random.Range(0.45f, 0.55f);
        sound.pitch = Random.Range(0.9f, 1.2f);
        sound.Play();
    }

    public void TakeDamage(int damage)
    {
        if (IsDead != true)
        {
            Animator.SetTrigger("TakeDamage");
            PlaySound(takeHitSound);

            Health -= damage;

            if (gameObject.name == "Player" && !gameObject.CompareTag("Dead"))
            {
                HealthBar healthBar = GameObject.Find("PlayerHealth").GetComponent<HealthBar>();
                healthBar.SetHealth(Health);
            }

            if (gameObject.CompareTag("Allies") && gameObject.name != "Player")
                StartCoroutine(IsInDangerCoroutine());

            if (Health <= 0) 
                Die();
        }
    }
    public bool TakesDamage()
    {
        if (IsTakingDamage == true)
        {
            NextAttackTime = Time.time;//belows to attack right after taking damage
            return IsTakingDamage = false;
        }
        else
            return IsTakingDamage = true;
    }
    protected void Die()
    {
        IsDead = true;
        Animator.SetBool("IsDead", IsDead);
        PlaySound(deathSound); 
        PlayAppearOrDisappearSound(disappearanceSound);
        
        Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
        if (gameObject.CompareTag("Enemy"))
        {
            GameObject DropItem = Resources.Load("DropItem", typeof(GameObject)) as GameObject;
            for (int i = 0; i < _dropsCount; i++)
            {
                Instantiate(DropItem, gameObject.transform.position + Random.insideUnitSphere * 0.5f, Quaternion.identity);
            }
            WorldManager.Instance.EnemyList.Remove(gameObject.GetComponent<Enemy>());
        }
        if (gameObject.name == "Player")
        {
            GameObject sprite = GameObject.FindGameObjectWithTag("Player");
            gameObject.tag = "Dead";
            GameObject.Find("DeathMusic").GetComponent<AudioSource>().Play();
        }
        if (gameObject.CompareTag("Allies"))
        {
            WorldManager.Instance.AlliesSoldersList.Remove(gameObject.GetComponent<AlliesSolders>());
        }
        gameObject.tag = "Dead";
        if (gameObject.name != "Player")
            Destroy(gameObject, 10);
        else
            WorldManager.Instance.ExitToMenu();
    }

    protected void FindTarget(string tag)
    {
        if (IsDead != true)
        {
            TargetsArray = GameObject.FindGameObjectsWithTag(tag);
            if (TargetsArray.Length != 0)
            {
                Target = TargetsArray[0];
                for (int i = 0; i < TargetsArray.Length; i++)
                {
                    if (Vector2.Distance(transform.position, TargetsArray[i].transform.position) < Vector2.Distance(transform.position, Target.transform.position))
                        Target = TargetsArray[i];
                }
                Directory = Target.transform.position.x - transform.position.x;

                if (Target != null)
                    transform.rotation = Target.transform.position.x < transform.position.x ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
            }
            else
                Target = null;
        }
    }
    protected bool IsBeyondGreenZoneBorders()
    {
        if (gameObject.transform.position.x > WorldManager.Instance.LeftBorder.transform.position.x - 2 && 
            gameObject.transform.position.x < WorldManager.Instance.RightBorder.transform.position.x + 2)
            return true;
        else
            return false;
    }
    protected void GoToGreenZone()
    {
        gameObject.layer = LayerMask.NameToLayer("GoesToPosition");
        transform.rotation = WorldManager.Instance.MiddleOfTheBorders.x < transform.position.x ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        transform.position = Vector2.MoveTowards(transform.position, WorldManager.Instance.MiddleOfTheBorders, Speed * Time.deltaTime);
    }

    public GameObject SelectBorder(GameObject border)
    {
        return Border = border;
    }
    protected void GoToBorder()
    {
        gameObject.layer = LayerMask.NameToLayer("GoesToPosition"); 
        transform.rotation = Border.transform.position.x < transform.position.x ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        transform.position = Vector2.MoveTowards(transform.position, Border.transform.position, Speed * Time.deltaTime);
    }

    protected IEnumerator IsInDangerCoroutine()
    {
        IsInDanger = true;
        yield return new WaitForSeconds(10);
        IsInDanger = false;
    }
    
    private void OnMouseDown()
    {
        InfoOnClick.instance.SelectTarget(this);
    }
}