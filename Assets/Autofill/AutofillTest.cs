using UnityEngine;

namespace Fizz6
{
    public class AutofillTest : MonoBehaviour
    {
        [SerializeField, Autofill]
        private Transform selfTransform;
        
        [SerializeField, Autofill(AutofillAttribute.Target.Parent)]
        private Transform parentTransform;
        
        [SerializeField, Autofill(AutofillAttribute.Target.Children)]
        private Transform childTransform;
        
        [SerializeField, Autofill(AutofillAttribute.Target.Self | AutofillAttribute.Target.Parent | AutofillAttribute.Target.Children)]
        private Transform[] allTransforms;
        
        [SerializeField, Autofill(AutofillAttribute.Target.Parent)]
        private Transform[] parentTransforms;
        
        [SerializeField, Autofill(AutofillAttribute.Target.Children)]
        private Transform[] childTransforms;

    }
}