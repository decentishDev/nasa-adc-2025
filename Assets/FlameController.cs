using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem particleSystem;
    public Rigidbody rb; // The Rigidbody of the object

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SimVars.flamer)
        {
            particleSystem.Play(); 
        }
        else
        {
            particleSystem.Stop();  
        }
    }
}
