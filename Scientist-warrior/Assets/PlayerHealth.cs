using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour,Health
{
    [SerializeField] private int health;
    [SerializeField] private GameObject partical;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health<=0)
            {
                Instantiate(partical, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    public void GetDamage(int i)
    {
        Health -= i;
    }
}
