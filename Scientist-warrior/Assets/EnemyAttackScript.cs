using Unity.Mathematics;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour, Health
{
    public Animator animator;
    public int damage;
    [SerializeField] private GameObject partical;
    [SerializeField] private int health;
    
    private int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health <= 0)
            {
                Instantiate(partical, transform.position, quaternion.identity);
                gameObject.SetActive(false);
            }
        }
    }

    private Health m_Playerhealth;
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Playerhealth = other.GetComponent<Health>();
            AttackAnimation();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Playerhealth = null;
            CancelInvoke(nameof(AttackAnimation));
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InvokeRepeating(nameof(AttackAnimation), 0.2f, 0.4f);
        }
    }

    void AttackAnimation()
    {
        animator.SetTrigger(Attack);
        Invoke(nameof(DealDamage), 0.2f);
    }

    void DealDamage()
    {
        m_Playerhealth?.GetDamage(damage);
    }

    public void GetDamage(int i)
    {
        Health -= i;
    }
}
/*
 *
    [SerializeField] private Collider2D AreaDetector;
    [SerializeField] private Collider2D AttackingAreaDetector;
    
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    public int damage;
    public bool LookAtLeft;
    private static readonly int State = Animator.StringToHash("State");
    private Vector2 m_LocalScale;
    private Vector2 m_DefaultPosition;
    private RaycastHit2D hit;
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Start()
    {
        m_LocalScale = transform.localScale;
        m_DefaultPosition = transform.position;
    }

    private void FixedUpdate()
    {
        DetectorHandle();
    }

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (value <= 0)
            {
                Instantiate(deathParticle, transform.position, quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    public void GetDamage(int i)
    {
        Health -= i;
    }

    void DetectorHandle()
    {
        RaycastHit2D[] hits = new RaycastHit2D[10];
        ContactFilter2D filter2D = new ContactFilter2D
        {
            layerMask = LayerMask.NameToLayer("Player")
        };
        int hitNums = AreaDetector.Cast(Vector2.zero, filter2D, hits, 0);
        if (hitNums == 0)
        {
            m_Animator.SetInteger(State, 0);
        }

        for (int i = 0; i < hitNums; i++)
        {
            m_Animator.SetInteger(State, 1);
            if (hits[i].transform.position.x > gameObject.transform.position.x)
            {
                if (LookAtLeft && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }

                if (!LookAtLeft && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }
            }
            else
            {
                if (!LookAtLeft && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }

                if (LookAtLeft && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }
            }

            transform.position = Vector3.Lerp(transform.position, hits[i].transform.position, 1 * Time.deltaTime);
            if (AttackingAreaDetector.Distance(hits[i].collider).distance <= 0)
            {
                m_Animator.SetTrigger(Attack);
                hit = hits[i];
                Invoke(nameof(HitThePlayer),0.5f);
                Debug.Log(m_Animator.GetCurrentAnimatorClipInfo(m_Animator.GetLayerIndex("Attack")));
            }
        }
    }

    private void HitThePlayer()
    {
        if (hit.transform.gameObject.GetComponent<Health>() != null)
            hit.transform.gameObject.GetComponent<Health>().GetDamage(damage);
    }
 * 
 */


//    public void LookDirectionHandle(Vector2 target)
//    {
//        if (target.x > transform.position.x)
//        {
//            if (LookAtLeft)
//            {
//                if (transform.localScale.x > 0)
//                {
//                    m_LocalScale.x *= -1;
//                }
//            }
//            else
//            {
//                if (transform.localScale.x < 0)
//                    m_LocalScale.x *= -1;
//            }
//        }
//        else if (target.x < transform.position.x)
//        {
//            if (LookAtLeft)
//            {
//                if (transform.localScale.x < 0)
//                {
//                    m_LocalScale.x *= -1;
//                }
//            }if (!LookAtLeft)
//            {
//                if (transform.localScale.x > 0)
//                {
//                    m_LocalScale.x *= -1;
//                }
//            }
//        }
//
//        transform.localScale = m_LocalScale;
//    }

#region OldCode

//    public void GetDamage(int i)
//    {
//        if (Health > 0)
//            Health -= i;
//    }
//
//    void DetectorHandle()
//    {
//        
//        RaycastHit2D[] hits = new RaycastHit2D[10];
//        ContactFilter2D filter2D = new ContactFilter2D()
//        {
//            layerMask = LayerMask.NameToLayer("Player")
//        };
//        int hitNums = AreaDetector.Cast(Vector2.zero, filter2D, hits, 0);
//        if (hitNums == 0)
//        {
//            m_Animator.SetInteger(State, 0);
//        }
//
//        for (int i = 0; i < hitNums; i++)
//        {
//            Debug.Log(hitNums);
//            m_Animator.SetInteger(State, 1);
//            if (hits[i].transform.position.x > gameObject.transform.position.x)
//            {
//                if (LookAtLeft && transform.localScale.x > 0)
//                {
//                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
//                }
//            }
//            else
//            {
//                if (!LookAtLeft && transform.localScale.x < 0)
//                {
//                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
//                }
//            }
//            transform.position = Vector3.Lerp(transform.position, hits[i].transform.position, 1 * Time.deltaTime);
////            if (hits[i].transform.gameObject.GetComponent<Health>() != null)
////                hits[i].transform.gameObject.GetComponent<Health>().GetDamage(damage);
//        }
//    }
//    

#endregion