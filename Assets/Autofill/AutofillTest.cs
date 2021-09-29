using UnityEngine;

namespace Fizz6
{
    public class AutofillTest : MonoBehaviour
    {
        [SerializeField, Autofill]
        private Transform _selfTransform;
        
        [SerializeField, Autofill(AutofillAttribute.Target.Parent)]
        private Transform _parentTransform;
        
        [SerializeField, Autofill(AutofillAttribute.Target.Children)]
        private Transform _childTransform;
        
        [SerializeField, Autofill(AutofillAttribute.Target.Self | AutofillAttribute.Target.Parent | AutofillAttribute.Target.Children)]
        private Transform[] _allTransforms;
        
        [SerializeField, Autofill(AutofillAttribute.Target.Parent)]
        private Transform[] _parentTransforms;
        
        [SerializeField, Autofill(AutofillAttribute.Target.Children)]
        private Transform[] _childTransforms;

    }
}