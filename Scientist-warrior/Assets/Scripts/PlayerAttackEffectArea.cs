using Script.CombatSystem;
using UnityEngine;
using Standard_Assets.CrossPlatformInput.Scripts;

public class PlayerAttackEffectArea : MonoBehaviour
{
    [SerializeField] private new Collider2D collider;
    [SerializeField] private int damage = 1;

    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            ContactFilter2D filter2D = new ContactFilter2D
            {
                layerMask = LayerMask.NameToLayer("Intractable")
            };
            int hitNums = collider.Cast(Vector2.zero,filter2D, hits, 0);
            for (int i = 0; i < hitNums; i++)
            {
                if (hits[i].transform.gameObject.GetComponent<CombatBase>() != null)
                    hits[i].transform.gameObject.GetComponent<CombatBase>().GetDamage(damage);
            }
        }
    }
}