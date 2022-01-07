using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UniRx;
using Satisfy.Attributes;

namespace Satisfy.Entities
{
    [HideMonoScript]
    public class RigidbodyPusher : MonoBehaviour
    {
        [Serializable]
        public enum PushType { Force, Torque };

        [SerializeField, Editor_R] TriggerObject trigger;
        [SerializeField, Editor_R, LabelText("Target")] Rigidbody rb;
        [SerializeField, Tweakable] bool local = true;
        [SerializeField, HorizontalGroup("Settings/2"), ValueDropdown("dirList"), HideLabel] bool random = false;
        [SerializeField, Tweakable, ShowIf("@trigger != null")] bool pushOnStay = false;
        [SerializeField, HorizontalGroup("Settings/1"), HideLabel] PushType pushType = PushType.Force;
        [SerializeField, HorizontalGroup("Settings/1"), HideLabel] ForceMode mode = ForceMode.Impulse;
        [SerializeField, HorizontalGroup("Settings/2"), ShowIf("@random == false"), HideLabel] Vector3 direction;
        [SerializeField, HorizontalGroup("Settings/2"), ShowIf("@random == true"), HideLabel] float randomForce = 3f;

        ValueDropdownList<bool> dirList = new ValueDropdownList<bool>()
        {
            {"Direction", false},
            {"Random", true}
        };

        [Button("Push", ButtonSizes.Large), PropertyOrder(-1), GUIColor(0.85f, 1, 0.85f), HideInEditorMode]
        public void Push()
        {
            rb.isKinematic = false;

            var dir = random ? UnityEngine.Random.insideUnitSphere * randomForce : direction;

            if (local)
                if (pushType == PushType.Force)
                    rb.AddRelativeForce(dir, mode);
                else rb.AddRelativeTorque(dir, mode);

            else if (pushType == PushType.Force)
                rb.AddForce(dir, mode);
            else rb.AddTorque(dir, mode);
        }

        public void PushFor(float time)
        {

            Observable.EveryFixedUpdate()
                .Take(TimeSpan.FromSeconds(time))
                .Subscribe(_ =>
                {
                    Push();
                }).AddTo(this);
        }



        void Start()
        {
            if (trigger != null)
            {
                trigger.Entered.Select(x =>
                {
                    var body = x.GetComponent<Rigidbody>();
                    return body == null ? x.transform.parent.GetComponent<Rigidbody>() : body;
                })
                .Subscribe(x =>
                {
                    SetTarget(x);
                    Push();

                    if (pushOnStay)
                        Observable.EveryUpdate().TakeWhile(_ => rb != null)
                            .Subscribe(_ => { Push(); }).AddTo(this);

                }).AddTo(this);

                trigger.Exit.Select(x =>
                {
                    var body = x.GetComponent<Rigidbody>();
                    return body == null ? x.transform.parent.GetComponent<Rigidbody>() : body;
                })
                .Where(x => x == rb)
                .Subscribe(x => { rb = null; }).AddTo(this);
            }
        }

        public void SetTarget(Rigidbody target)
        {
            rb = target;
        }

        void OnDrawGizmosSelected()
        {
            if (pushType == PushType.Torque)
                return;

            if (random)
                return;

            Gizmos.color = Color.cyan;

            var source = rb == null ? transform.position : rb.transform.position;

            Gizmos.DrawSphere(source, 0.15f);
            Gizmos.DrawLine(source, source + direction);
            Gizmos.DrawSphere(source + direction, 0.1f);
        }
    }
}
