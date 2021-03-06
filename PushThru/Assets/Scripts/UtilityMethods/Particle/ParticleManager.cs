using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager particleManager;

    private void Awake()
    {
        if (particleManager == null)
            particleManager = this;
    }


    //Event system for player particles
    public event Action<String> playParticleEvent;
    public void PlayParticle(string _id)
    {
        playParticleEvent?.Invoke(_id);
    }

    public event Action<String> stopParticleEvent;
    public void StopParticle(string _id)
    {
        stopParticleEvent?.Invoke(_id);
    }

    public event Action<String, bool> setParticleActiveEvent;
    public void SetParticleActive(string _id, bool val)
    {
        if (setParticleActiveEvent != null)
        {
            setParticleActiveEvent(_id, val);
        }
    }

    public event Action<String, float> setParticleArcEvent;
    public void SetParticleArc(string _id, float val)
    {
        if (setParticleActiveEvent != null)
        {
            setParticleArcEvent(_id, val);
        }
    }

    public event Action<String, Vector3> setParticlePositionEvent;
    public void SetParticlePosition(string _id, Vector3 val)
    {
        if (setParticleActiveEvent != null)
        {
            setParticlePositionEvent(_id, val);
        }
    }
}