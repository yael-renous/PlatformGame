using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : InteractableObject
{
    public static readonly string TAG_STRING = "Key";
    [SerializeField] private ParticleSystem _particleSystemPrefab;
    

    public override string GetTag()
    {
        return TAG_STRING;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(PlayerController.TAG_STRING))
        {
            Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
