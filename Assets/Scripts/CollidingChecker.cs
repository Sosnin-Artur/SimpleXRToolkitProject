using System;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CollidingChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _collidingMask;
    [SerializeField] private List<Joint> _joints;
    [SerializeField] private XRGrabInteractable _interactable;
    [SerializeField] private float _force = 100f;
    [SerializeField] private Collider _collider;
    [SerializeField] private float explosionForce = 10f;
    
    private void OnValidate()
    {
        var joints = GetComponents<Joint>();

        _joints.Clear();
        
        foreach (var joint in joints)
        {
            joint.breakForce = float.PositiveInfinity;
            _joints.Add(joint);
        }
        
        _interactable = GetComponent<XRGrabInteractable>();
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_collidingMask.Contains(other.gameObject.layer))
        {
            if (_joints.Count > 0 && other.impulse.magnitude >= _force)
            {
                foreach (var joint in _joints.ToArray())
                {
                    _joints.Remove(joint);
                    Destroy(joint);
                }

                PlayBreakEffects(other);
            }
        }
    }

    private void PlayBreakEffects(Collision other)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.AddExplosionForce(explosionForce, other.contacts[0].point, 1f);
    }
}
