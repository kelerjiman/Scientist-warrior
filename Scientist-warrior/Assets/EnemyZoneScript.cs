using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZoneScript : MonoBehaviour
{
    private Transform m_PlayerTransform;
    [SerializeField] private Transform EnemyVisual;
    [SerializeField] float speed;
    [SerializeField] private Vector2 DefaultPosition = Vector2.zero;
    private bool playerInSight = false;
    private Vector2 localTransform;
    [Range(-1,1)]
    [SerializeField] private int LookDirection = 1;

    private static readonly int State = Animator.StringToHash("State");

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerInSight = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.transform.position.x > EnemyVisual.position.x && EnemyVisual.localScale.x > 0)
                localTransform = new Vector2(localTransform.x * -1, localTransform.y);
            if (other.transform.position.x < EnemyVisual.position.x && EnemyVisual.localScale.x < 0)
                localTransform = new Vector2(localTransform.x * -1, localTransform.y);
            EnemyVisual.localScale = localTransform;
            EnemyVisual.position = Vector2.Lerp(EnemyVisual.position, other.transform.position,
                speed * Time.fixedDeltaTime);
            EnemyVisual.gameObject.GetComponent<Animator>().SetInteger(State,1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerInSight = false;
        EnemyVisual.gameObject.GetComponent<Animator>().SetInteger(State,0);
    }

    private void Start()
    {
        DefaultPosition = EnemyVisual.position;
        localTransform = EnemyVisual.localScale;
        localTransform.x *= LookDirection;
    }

    private void Update()
    {
        if(playerInSight)
            return;
        if (DefaultPosition.x > EnemyVisual.position.x && EnemyVisual.localScale.x > 0)
            EnemyVisual.localScale = new Vector2(EnemyVisual.localScale.x * -1, EnemyVisual.localScale.y);
        if (DefaultPosition.x < EnemyVisual.position.x && EnemyVisual.localScale.x < 0)
            EnemyVisual.localScale = new Vector2(EnemyVisual.localScale.x * -1, EnemyVisual.localScale.y);
        
        EnemyVisual.position = Vector2.Lerp(EnemyVisual.position, DefaultPosition,
            speed * Time.fixedDeltaTime);
    }
}