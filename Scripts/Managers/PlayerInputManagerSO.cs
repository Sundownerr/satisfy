using System;
using Satisfy.Attributes;
using Satisfy.Variables;
using UniRx;
using UnityEngine;

namespace Satisfy.Managers
{
    [CreateAssetMenu(fileName = "PlayerInputManagerSO", menuName = "Managers/Player Input")]
    [Serializable]
    public class PlayerInputManagerSO : ScriptableObjectSystem
    {
        [SerializeField, Variable_R] private Satisfy.Bricks.Event tap;
        [SerializeField, Variable_R] private Satisfy.Bricks.Event release;
        [SerializeField, Variable_R] private Vector2Variable pointerStartPos;
        [SerializeField, Variable_R] private Vector2Variable pointerCurrentPos;
        [SerializeField, Variable_R] private Vector2Variable pointerDeltaPos;
        [SerializeField, Tweakable] private float dpi = 100;

        public override void Initialize()
        {
            var update = Observable.EveryUpdate();
            
            update.Subscribe(_ =>
            {
                pointerCurrentPos.SetValue(Input.mousePosition);
            });

            update.Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    pointerStartPos.SetValue(Input.mousePosition);
                    pointerCurrentPos.SetValue(Input.mousePosition);
                    tap.Raise();

                    update.TakeUntil(release.Raised).Subscribe(_ =>
                    {
                        pointerDeltaPos.SetValue(new Vector2(Input.GetAxis("Mouse X") * dpi,
                                                            Input.GetAxis("Mouse Y") * dpi));
                    });
                });

            update.Where(_ => Input.GetMouseButtonUp(0))
                .Subscribe(_ =>
                {
                    release.Raise();
                });
        }
    }
}