using DG.Tweening;
using Script.CharacterStatus;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteResorter))]
public class Mount : MonoBehaviour
{
    [SerializeField] Animator animator;
    public Transform RiderTransform;
    bool rideOn = false;
    [SerializeField] bool LookRight = false;
    private Vector2 DefaultLocalScale;
    public SpriteResorter spriteResorter;
    [SerializeField] Rigidbody2D rig;
    public List<State> states;
    public float Speed = 1;
    private void Start()
    {
        DefaultLocalScale = transform.localScale;
        spriteResorter = GetComponent<SpriteResorter>();
    }
    private void Update()
    {
        if (!rideOn)
        {
            animator.SetInteger("State", 0);
            return;
        }
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
        {
            Debug.Log("move horse");
            animator.SetInteger("State", 1);
        }
        else
        {
            Debug.Log("stop Horse");
            animator.SetInteger("State", 0);
        }
    }
    public void RideOn(Transform target, int RightDirection,Transform ResorterParent)
    {
        directionHandler(RightDirection);
        //if (!rideOn)
        //{
        //    rig.DOMove(target.position, 1f, false);
        //}
        transform.parent = target;
        transform.position = target.position;
        RiderTransform = target;
        rideOn = true;
        spriteResorter.ChangeParent(ResorterParent);
        spriteResorter.ChangeSortingLayer(SpriteResorter.LayerType.Front);
    }
    public void RideOff()
    {
        transform.parent = null;
        spriteResorter.ChangeParent(transform.parent);
        spriteResorter.ChangeSortingLayer(spriteResorter.layerType);
        rideOn = false;
    }
    void directionHandler(int RightDirection)
    {
        if (RightDirection > 0)
        {
            if (LookRight)
            {
                if (DefaultLocalScale.x > 0)
                    DefaultLocalScale.x *= -1;
            }
            else if (!LookRight)
            {
                if (DefaultLocalScale.x < 0)
                    DefaultLocalScale.x *= -1;
            }
        }
        else if (RightDirection < 0)
        {
            if (!LookRight)
            {
                if (DefaultLocalScale.x > 0)
                    DefaultLocalScale.x *= -1;
            }
            else if (LookRight)
            {
                if (DefaultLocalScale.x < 0)
                    DefaultLocalScale.x *= -1;
            }
        }
        transform.localScale = DefaultLocalScale;
    }
}
