using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    [Header("Weight Prefs")]
    [SerializeField] private float _leftHandWeight = 1.0f;
    [SerializeField] private float _rightHandWeight = 1.0f;

    [Header("Hand Objects (For Automaticly Use)")]
    public Transform HandObjLeft; //Left Hand Object
    public Transform HandObjRight; //Right Hand Object

    [Header("IK Activator (For Automaticly Use)")]
    public bool ikActive = false;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if (ikActive)
        {
            if (HandObjLeft != null && HandObjRight != null)
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightHandWeight);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, HandObjRight.position);

                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftHandWeight);
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, HandObjLeft.position);
            }
        }
        else
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }
        
    }

    public void SetIKActive(bool active)
    {
        ikActive = active;
    }

}
