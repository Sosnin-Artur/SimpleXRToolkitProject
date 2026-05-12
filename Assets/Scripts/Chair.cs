using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField] private List<Joint> _joints;

    private bool _flag;
    
    private void OnValidate()
    {
        _joints.Clear();
        
        foreach (var child in transform.GetComponentsInChildren<Transform>())
        {
            var joints = child.GetComponents<Joint>();

            foreach (var joint in joints)
            {
                _joints.Add(joint);
            }
        }
    }

    private void Update()
    {
        if (!_flag)
        {
            if (_joints.All(j => j == null))
            {
                Debug.Log($"Win");
                _flag = true;
            }
        }
    }
}
