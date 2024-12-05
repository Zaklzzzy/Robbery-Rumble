using UnityEngine;
using DG.Tweening;

public class VanMovment : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private Transform[] _transform;

    private void Start()
    {
        Sequence mySequence = DOTween.Sequence();
        /*        mySequence.Append(_objects[0].transform.DOScale(new Vector3(4.75f, 4.75f, 4.75f),0.35f));
                mySequence.Append(_objects[0].transform.DOScale(new Vector3(4.5f, 4.5f, 4.5f),0.35f));*/
        for (int i = 0; i < _objects.Length; i++)
        {
            mySequence.Append(_objects[i].transform.DOMove(_transform[i].position, 5.0f, false));
        }
    }
}
