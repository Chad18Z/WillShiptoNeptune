using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{

    public ParticleSystem system;
    public float rate;
    ParticleSystem.EmissionModule emModule;


	// Use this for initialization
	void Start ()
    {
        emModule = system.emission;
	}
	
	// Update is called once per frame
	void Update ()
    {
        emModule.rateOverTime = rate;
    }
}
