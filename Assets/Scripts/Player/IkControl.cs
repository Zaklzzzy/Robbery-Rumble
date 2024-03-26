using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    private Animator _animator;

    bool ikActive = false;

    public Transform HandObj = null;

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
            if (HandObj != null)
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, HandObj.position);

                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, HandObj.position);
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
}
