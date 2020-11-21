using System.Collections;
using CombatSystem;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    private bool m_TimeToAttack = false;

    public bool TimeToAttack
    {
        get { return m_TimeToAttack; }
        set
        {
            m_TimeToAttack = value;
            if (value)
            {
                StartCoroutine(AttackFunction());
            }
            else
            {
                StopCoroutine(AttackFunction());
            }
        }
    }

    public Animator animator;
    public int damage;
    [SerializeField] private GameObject partical;
    [SerializeField] private int health;
    [SerializeField] private float AttackSpeed = 0.2f;
    [SerializeField] private SliderScript HealthSlider;

    private void Start()
    {
        HealthSlider.MaxAmount = health;
        HealthSlider.CurrentAmount = health;
    }

    private int Health
    {
        get { return health; }
        set
        {
            health = value;
            HealthSlider.CurrentAmount = health;
            if (health <= 0)
            {
                Instantiate(partical, transform.position, quaternion.identity);
//                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
    private PlayerCombat m_Playerhealth;
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Playerhealth = other.GetComponent<PlayerCombat>();
            if (!TimeToAttack)
                TimeToAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Playerhealth = null;
            TimeToAttack = false;
        }
    }

    void AttackAnimation()
    {
        animator.SetTrigger(Attack);
//        Invoke(nameof(DealDamage), AttackSpeed);
    }


    private IEnumerator AttackFunction()
    {
        yield return new WaitForSecondsRealtime(AttackSpeed);
        AttackAnimation();
        DealDamage();
        StartCoroutine(AttackFunction());
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