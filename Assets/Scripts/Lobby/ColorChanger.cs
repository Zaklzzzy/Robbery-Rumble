using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private Color _color;

    public void ChangeColor()
    {
        _material.color = _color;
    }
}
