using Script.CombatSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float destroyDlyTime = 0.2f;
    public Vector2 velocity = Vector2.zero;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] ParentType parentType;
    public int damage = 1;
    private void Start()
    {
        rb.velocity = velocity;
        Destroy(gameObject, destroyDlyTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (parentType == ParentType.Player)
        {
            if (!collision.CompareTag("Player"))
            {
                var coll = collision.gameObject.GetComponent<IINteractable>();
                if (coll != null)
                    coll.GetDamage(damage);
                Destroy(gameObject);
            }

        }
        else if (parentType == ParentType.Blast)
        {
            var coll = collision.gameObject.GetComponent<IINteractable>();
            if (coll != null)
                coll.GetDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            if (!collision.CompareTag("Other"))
            {
                var coll = collision.gameObject.GetComponent<IINteractable>();
                if (coll != null)
                    coll.GetDamage(damage);
                
            }
            Destroy(gameObject);
        }
        Debug.Log(collision.name);
        

    }
    enum ParentType
    {
        Player,
        Other,
        Blast
    }
}
