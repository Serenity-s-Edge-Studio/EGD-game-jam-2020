﻿using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightDrop : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    [SerializeField]
    public new Light2D light;
    [SerializeField]
    private bool Decay;
    [SerializeField]
    private float decayTime;
    [SerializeField]
    private float decaySpeed;
    public float remainingTime;

    private float startingColliderRadius;
    private float startingLightRadius;
    private new CircleCollider2D collider;
    private List<Enemy> enemiesInRange = new List<Enemy>();

    protected virtual void Start()
    {
        remainingTime = decayTime;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        light = GetComponent<Light2D>();
        startingColliderRadius = collider.radius;
        startingLightRadius = light.pointLightOuterRadius;
        LightManager.instance.add(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemiesInRange.Add(enemy);
            enemy.influencingLights.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemiesInRange.Remove(enemy);
            enemy.influencingLights.Remove(this);
        }
    }

    protected virtual void OnDestroy()
    {
        foreach (Enemy enemy in enemiesInRange)
        {
            enemy.influencingLights.Remove(this);
        }
        LightManager.instance.remove(this);
    }

    public LightDropStruct toLightDropStruct()
    {
        return new LightDropStruct
        {
            canDecay = Decay,
            decayTime = decayTime,
            decaySpeed = decaySpeed,
            remainingTime = remainingTime,
            startingColliderRadius = startingColliderRadius,
            startingLightRadius = startingLightRadius,
            pointLightOuterRadius = light.pointLightOuterRadius,
            colliderRadius = collider.radius
        };
    }

    public void fromLightDropStruct(LightDropStruct lightDropStruct)
    {
        remainingTime = lightDropStruct.remainingTime;
        light.pointLightOuterRadius = lightDropStruct.pointLightOuterRadius;
        collider.radius = lightDropStruct.colliderRadius;
    }
}

public struct LightDropStruct
{
    public bool canDecay;
    public float decayTime;
    public float decaySpeed;
    public float remainingTime;
    public float startingColliderRadius;
    public float startingLightRadius;
    public float pointLightOuterRadius;
    public float colliderRadius;

    public void Decay(float deltaTime)
    {
        if (canDecay)
        {
            remainingTime = math.max(0, remainingTime - deltaTime * decaySpeed);
            float remaining = remainingTime / decayTime;
            pointLightOuterRadius = startingLightRadius * remaining;
            colliderRadius = startingColliderRadius * remaining;
            //remainingTime = remaining;
        }
    }
}
