using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CoinController : InteractableObject
{
    public static readonly string TAG_STRING = "Coin";
    
    [SerializeField] private ParticleSystem _particleSystemPrefab;
    private float _rotationSpeed = 50.0f;

    public override string GetTag()
    {
        return TAG_STRING;
    }

    void Update()
    {
        float rotationThisFrame = _rotationSpeed * Time.deltaTime;

        // Apply the rotation around the X-axis
        transform.Rotate(rotationThisFrame, 0f, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(PlayerController.TAG_STRING))
        {
            Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

   
    
    
}
