using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    private Animator _animator;

    public bool ikActive = false;

    public Transform HandObjLeft; //Left Hand Object
    public Transform HandObjRight; //Right Hand Object

    public float leftHandWeight = 1.0f;
    public float rightHandWeight = 1.0f;

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
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, HandObjRight.position);

                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
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
