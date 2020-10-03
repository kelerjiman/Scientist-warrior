using System;
using UnityEngine;
using DG.Tweening;

namespace Script
{
    public class MovementBase : MonoBehaviour
    {
        [SerializeField] private int MoveSpeed = 1;
        public Action<Vector2> OnMoveEvent;

        private void Start()
        {
            OnMoveEvent += MoveToTarget;
        }

        public virtual void MoveToTarget(Vector2 pos)
        {
            
            transform.DOMove(pos, 1);
            
        }

        private void LookDirection()
        {
//            var visualTransform = visual.transform;
//            var localScale = visualTransform.localScale;
//            if (visual.transform.position.x > target.x && Math.Abs(Mathf.Sign(localScale.x) * 1 - 1) < 0.1)
//            {
//                localScale = new Vector2(-localScale.x, localScale.y);
//                visualTransform.localScale = localScale;
//            }
//
//            if (visual.transform.position.x < target.x && Math.Abs(Mathf.Sign(localScale.x) * 1 - 1) > 0.1)
//            {
//                localScale = new Vector2(-localScale.x, localScale.y);
//                visualTransform.localScale = localScale;
//            }
//
//            var position = visualTransform.position;
//            position = Vector2.Lerp(position, target, moveSpeed * Time.deltaTime);
//            visualTransform.position = position;
        }
    }
}
