using System;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

namespace Satisfy.Utility
{
    [HideMonoScript]
    [CreateAssetMenu(fileName = "AnimatorAction", menuName = "Data/Animator Action")]
    public class AnimatorAction : ScriptableObject
    {
        [Serializable]
        public enum Action
        {
            SetTrigger,
            ResetTrigger,
            PlayAnimation,
            SetFloat,
            SetBool,
            SetInt
        }

        [HideLabel] [SerializeField] private Action actionType;

        [HideLabel]
        [ValueDropdown(nameof(layers))]
        [HideIf("@usingAnimationStates == false")]
        [OnValueChanged("@SetStateNames(allLayers[layer])")]
        public int layer;

        [HideLabel] [ValueDropdown(nameof(triggers))] [HideIf("@usingTriggers == false")]
        public string triggerName;

        [HideLabel] [ValueDropdown(nameof(stateNames))] [HideIf("@usingAnimationStates == false")]
        public string stateName;

        [HideLabel] [ValueDropdown(nameof(floatNames))] [HideIf("@usingFloats == false")]
        public string floatName;

        [HideLabel] [ValueDropdown(nameof(boolNames))] [HideIf("@usingBools == false")]
        public string boolName;

        [HideLabel] [ValueDropdown(nameof(intNames))] [HideIf("@usingInts == false")]
        public string intName;

        [SerializeField] [HideIf("@usingBools == false")] [LabelText("Value")] [LabelWidth(40)]
        private bool boolParameter;

        [SerializeField] [HideIf("@usingFloats == false")] [LabelText("Value")] [LabelWidth(40)]
        private float floatParameter;

        [SerializeField] [HideIf("@usingInts == false")] [LabelText("Value")] [LabelWidth(40)]
        private int intParameter;

        private ValueDropdownList<string> boolNames;
        private ValueDropdownList<string> floatNames;
        private ValueDropdownList<string> intNames;
        private ValueDropdownList<int> layers;
        private ValueDropdownList<string> stateNames;
        private ValueDropdownList<string> triggers;
        public Action ActionType => actionType;

        private bool usingFloats => actionType == Action.SetFloat;
        private bool usingBools => actionType == Action.SetBool;
        private bool usingInts => actionType == Action.SetInt;
        private bool usingTriggers => actionType == Action.ResetTrigger || actionType == Action.SetTrigger;
        private bool usingAnimationStates => actionType == Action.PlayAnimation;
        public int IntParameter => intParameter;
        public float FloatParameter => floatParameter;
        public bool BoolParameter => boolParameter;

        public string Name()
        {
            if (usingTriggers) return triggerName;
            if (usingAnimationStates) return stateName;
            if (usingFloats) return floatName;
            if (usingBools) return boolName;
            if (usingInts) return intName;

            Debug.LogError("can't get animation data name");
            return "error";
        }

        public void Play(Animator animator)
        {
            var hash = Animator.StringToHash(Name());

            switch (ActionType)
            {
                case Action.SetTrigger:
                    animator.SetTrigger(hash);
                    break;
                case Action.ResetTrigger:
                    animator.ResetTrigger(hash);
                    break;
                case Action.PlayAnimation:
                    animator.Play(hash, layer);
                    break;
                case Action.SetFloat:
                    animator.SetFloat(hash, FloatParameter);
                    break;
                case Action.SetBool:
                    animator.SetBool(hash, BoolParameter);
                    break;
                case Action.SetInt:
                    animator.SetInteger(hash, IntParameter);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

#if UNITY_EDITOR
        private AnimatorControllerLayer[] allLayers;

        [HideInInlineEditors]
        [PropertySpace(20)]
        [Button(ButtonSizes.Large)]
        public void SetData(AnimatorController animatorController)
        {
            SetLayers(animatorController.layers);
            SetParameters(animatorController.parameters);
        }

        private void SetLayers(AnimatorControllerLayer[] layers)
        {
            allLayers = layers;
            this.layers = new ValueDropdownList<int>();

            for (var i = 0; i < layers.Length; i++)
            {
                this.layers.Add($"Layer {i}: {layers[i].name}", i);
                SetStateNames(layers[i]);
            }
        }

        private void SetStateNames(AnimatorControllerLayer layer)
        {
            stateNames = new ValueDropdownList<string>();
            var states = layer.stateMachine.states;

            foreach (var x in states) stateNames.Add(x.state.name, x.state.name);
        }

        private void SetParameters(AnimatorControllerParameter[] parameters)
        {
            floatNames = new ValueDropdownList<string>();
            intNames = new ValueDropdownList<string>();
            boolNames = new ValueDropdownList<string>();
            triggers = new ValueDropdownList<string>();

            foreach (var parameter in parameters)
            {
                var name = parameter.name;
                var type = parameter.type;

                switch (type)
                {
                    case AnimatorControllerParameterType.Float:
                        floatNames.Add($"{name} [{type}]", name);
                        break;
                    case AnimatorControllerParameterType.Int:
                        intNames.Add($"{name} [{type}]", name);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        boolNames.Add($"{name} [{type}]", name);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        triggers.Add($"{name} [{type}]", name);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


#endif
    }
}