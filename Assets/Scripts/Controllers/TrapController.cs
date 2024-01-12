using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TrapController : InteractableObject
{
    public static readonly string TAG_STRING = "Trap"; 
    [SerializeField] private ParticleSystem _particleSystemPrefab;

    public int maxRotationSpeed = 10000; // Maximum rotation speed
    public float changeInterval = 1.0f; // Interval in seconds to change rotation speed

    private float currentRotationSpeed;
    private float timer;

    protected override void Start()
    {
        base.Start();
        ChangeRotationSpeed();
    }
    public override string GetTag()
    {
        return TAG_STRING;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Check if it's time to change the rotation speed
        if (timer >= changeInterval)
        {
            ChangeRotationSpeed();
            timer = 0;
        }

        // Apply the rotation
        transform.Rotate(0f, currentRotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    private void ChangeRotationSpeed()
    {
        // Randomly change the rotation speed
        currentRotationSpeed = Random.Range(100, maxRotationSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(PlayerController.TAG_STRING))
        {
            Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity);
        }
    }
}
