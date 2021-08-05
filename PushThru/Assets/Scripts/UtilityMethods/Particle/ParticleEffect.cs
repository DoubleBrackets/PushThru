using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public string particleId;

    ParticleSystem pSys;

    void Start()
    {
        pSys = gameObject.GetComponent<ParticleSystem>();
        ParticleManager.particleManager.setParticleActiveEvent += SetActive;
        ParticleManager.particleManager.playParticleEvent += Play;
        ParticleManager.particleManager.stopParticleEvent += Stop;
        ParticleManager.particleManager.setParticleArcEvent += SetArc;
    }


    void SetActive(string _id, bool val)
    {
        if (_id.CompareTo(particleId) == 0)
        {
            if (val)
            {
                var c = pSys.main;
                c.maxParticles = 1000;
            }
            else
            {
                var c = pSys.main;
                c.maxParticles = 0;
            }
        }
    }

    void Play(string _id)
    {
        if (_id.CompareTo(particleId) == 0)
        {
            pSys.Play();
        }
    }

    void Stop(string _id)
    {
        if (_id.CompareTo(particleId) == 0)
        {
            pSys.Stop();
        }
    }

    void SetArc(string _id, float arc)
    {
        if (_id.CompareTo(particleId) == 0)
        {
            var shape = pSys.shape;
            shape.arc = arc;
        }
    }

    private void OnDestroy()
    {
        ParticleManager.particleManager.setParticleActiveEvent -= SetActive;
        ParticleManager.particleManager.playParticleEvent -= Play;
        ParticleManager.particleManager.stopParticleEvent -= Stop;
        ParticleManager.particleManager.setParticleArcEvent -= SetArc;
    }

}