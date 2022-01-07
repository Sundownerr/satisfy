using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Satisfy.Attributes;
using Satisfy.Variables;

namespace Satisfy.Entities
{
    [HideMonoScript]
    public class LerpTo : MonoBehaviour
    {
        [SerializeField, Editor_R, HideIf("@targetPointVariable != null")] Transform targetPoint;
        [SerializeField, Editor_R, HideIf("@targetPoint != null")] GameObjectVariable targetPointVariable;
        [SerializeField, Editor_R] Transform moveObject;
        [SerializeField, Editor_R] Transform startPoint;
        [SerializeField, Tweakable, Range(0f, 1f), ShowIf("@startPoint != null")] float fromStartToTargetPercent = 1f;
        [SerializeField, Tweakable] float speed = 12f;
        [SerializeField, Tweakable] float accelerationTime = 0f;
        [SerializeField, Tweakable] bool doAtStart;

        bool isActive;
        float realSpeed;

        void OnValidate()
        {
            if (moveObject == null)
                moveObject = transform;
        }

        void Start()
        {
            var update = Observable.EveryGameObjectUpdate().Where(_ => isActive && enabled);

            update.Select(x => GetTargetPoint())
                .Where(x => x != null)
                .Subscribe(targetPoint =>
                {
                    moveObject.position = Vector3.Lerp(moveObject.position,
                                                       GetTargetPosition(),
                                                       Time.deltaTime * realSpeed);
                }).AddTo(this);

            if (doAtStart)
                EnableLerp();
        }

        Vector3 GetTargetPosition()
        {
            if (startPoint == null)
            {
                return GetTargetPoint().position;
            }

            return Vector3.Lerp(startPoint.position,
                                GetTargetPoint().position,
                                fromStartToTargetPercent);
        }

        Transform GetTargetPoint()
        {
            var isTargetVariableNotValid = targetPointVariable == null
                                           || targetPointVariable.Value == null;

            if (isTargetVariableNotValid)
            {
                return targetPoint;
            }

            if (targetPoint == null)
            {
                return targetPointVariable.Value.transform;
            }

            throw new UnassignedReferenceException();
        }

        public void EnableLerp()
        {
            isActive = true;

            if (accelerationTime == 0)
            {
                realSpeed = speed;
                return;
            }

            DOTween.To(() => realSpeed, val => { realSpeed = val; }, speed, accelerationTime);
        }

        public void DisableLerp()
        {
            isActive = false;
        }

        public void SetTargetPoint(Transform transform)
        {
            targetPoint = transform;
        }

        public void SetTargetPoint(TransformVariable variable) =>
            SetTargetPoint(variable.Value);

        public void SetTargetPoint(GameObjectVariable variable) =>
            SetTargetPoint(variable.Value.transform);


        public void SetMoveObject(GameObject target)
        {
            moveObject = target.transform;
        }
    }
}
