using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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
        GameManager.Instance.LostLifeAction += ShowParticles;
    }

    private void OnDestroy()
    {
        GameManager.Instance.LostLifeAction += ShowParticles;
    }


    public override string GetTag()
    {
        return TAG_STRING;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            ChangeRotationSpeed();
            timer = 0;
        }

        transform.Rotate(0f, currentRotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    private void ChangeRotationSpeed()
    {
        currentRotationSpeed = Random.Range(100, maxRotationSpeed);
    }
    
    private void ShowParticles()
    {
        Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity);
    }
}
