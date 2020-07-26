using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour, Health
{
    [SerializeField] private Collider2D AreaDetector;
    [SerializeField] private int health = 1;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    public int damage;
    public bool LookAtLeft;
    private static readonly int State = Animator.StringToHash("State");
    private Vector2 m_LocalScale;
    private Vector2 m_DefaultPosition;

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
    }
    void DetectorHandle()
    {
        
        RaycastHit2D[] hits = new RaycastHit2D[10];
        ContactFilter2D filter2D = new ContactFilter2D()
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
            }
            else
            {
                if (!LookAtLeft && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }
            }
            transform.position = Vector3.Lerp(transform.position, hits[i].transform.position, 1 * Time.deltaTime);
//            if (hits[i].transform.gameObject.GetComponent<Health>() != null)
//                hits[i].transform.gameObject.GetComponent<Health>().GetDamage(damage);
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
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
}