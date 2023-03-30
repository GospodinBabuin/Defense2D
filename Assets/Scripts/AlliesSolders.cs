using System.Collections;
using UnityEngine;

public class AlliesSolders : Unit
{
    private void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();

        gameObject.name = Name;
        WorldManager.Instance.AlliesSoldersList.Add(this);
        
        PlayAppearOrDisappearSound(spawnSound);

        IsOnBorder = false;
        lastPosition = new Vector3(transform.position.x, transform.position.y);
    }
    private void Update()
    {
        SetWalkingAnimation();
        if (onBorder != OnBorder.UNSELECTED)
        {
            if (!IsOnBorder)
            {
                GoToBorder();
                Debug.Log("goToBorder");
            }
            else
            {
                if (TargetsArray.Length == 0 && !IsBeyondGreenZoneBorders())
                { 
                    GoToGreenZone();
                    Debug.Log("gotoGZ");
                }
                else 
                {
                    Behaviour();
                    Debug.Log("behaviour");
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (!IsBeyondGreenZoneBorders() || IsInDanger)
            FindTarget("Enemy"); 
        if (gameObject.layer == LayerMask.NameToLayer("GoesToPosition") && IsOnBorder)
            gameObject.layer = LayerMask.NameToLayer(Name);
    }
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer == LayerMask.NameToLayer("GoesToPosition"))
        {
            if (collision == Border.GetComponent<Collider2D>())
            {
                IsOnBorder = true;
                StartCoroutine(ChangeLayer(gameObject));
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.layer == LayerMask.NameToLayer("GoesToPosition"))
        {
            if (collision == Border.GetComponent<Collider2D>())
            {
                IsOnBorder = true;
                StartCoroutine(ChangeLayer(gameObject));
            }
        }
    } 
    
    private IEnumerator ChangeLayer(GameObject go)
    {
        yield return new WaitForSeconds(1);

        go.layer = LayerMask.NameToLayer(Name);
        yield break;
    }
}