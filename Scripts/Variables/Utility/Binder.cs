using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using System.Linq;
using System.Reflection;
using Satisfy.Variables;
using System;
using System.Data;
using Microsoft.CSharp;

namespace Satisfy.Utility
{
    [HideMonoScript]
    public class Binder : MonoBehaviour
    {
        [SerializeField, HorizontalGroup("1"), LabelText("From"), OnValueChanged(nameof(UpdatePropertiyList)), LabelWidth(35)]
        private UnityEngine.Object source;

        [SerializeField, HorizontalGroup("1"), ValueDropdown(nameof(membersSource)), HideLabel, LabelWidth(5)]
        private string selectedMemberSource;

        [SerializeField, HorizontalGroup("2"), LabelText("To"), OnValueChanged(nameof(UpdatePropertiyList)), LabelWidth(35)]
        private UnityEngine.Object target;

        [SerializeField, HorizontalGroup("2"), ValueDropdown(nameof(membersTarget)), HideLabel, LabelWidth(5)]
        private string selectedMemberTarget;

        private static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private ValueDropdownList<string> membersSource = new ValueDropdownList<string>();
        private ValueDropdownList<string> membersTarget = new ValueDropdownList<string>();

        private dynamic setter;
        private dynamic getter;

        private void Start()
        {
            var targetProperty = target.GetType().GetProperty(selectedMemberTarget, flags);
            dynamic runtimeTargetValue = targetProperty.GetValue(target);
            setter = GetSetter(target, runtimeTargetValue, targetProperty.GetSetMethod());

            var sourceProperty = source.GetType().GetProperty(selectedMemberSource, flags);
            dynamic runtimeSourceValue = sourceProperty.GetValue(source);
            getter = GetGetter(source, runtimeSourceValue, sourceProperty.GetGetMethod());

            // Observable.EveryUpdate()
            //     .Subscribe(_ =>
            //     {
            //         setter(getter());
            //     }).AddTo(this);
        }

        [Button]
        public void UpdateTargetValue()
        {
            setter(getter());
        }

        private void UpdateData(ref string selectedMember, ref ValueDropdownList<string> members, UnityEngine.Object target)
        {
            selectedMember = "";
            members.Clear();

            if (target == null)
                return;

            var properties = target.GetType().GetProperties(flags);

            foreach (var property in properties)
            {
                members.Add($"{property.Name}  [{property.PropertyType.Name}]", property.Name);
            }
        }

        [Button]
        private void UpdatePropertiyList()
        {
            UpdateData(ref selectedMemberSource, ref membersSource, source);
            UpdateData(ref selectedMemberTarget, ref membersTarget, target);
        }

        private Action<V> GetSetter<T, V>(T source, V valueSample, MethodInfo methodInfo)
        {
            return (Action<V>)Delegate.CreateDelegate(typeof(Action<V>), source, methodInfo);
        }

        private Func<V> GetGetter<T, V>(T source, V valueSample, MethodInfo methodInfo)
        {
            return (Func<V>)Delegate.CreateDelegate(typeof(Func<V>), source, methodInfo);
        }
    }
}