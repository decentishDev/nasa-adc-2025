using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem particleSystem;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (SimVars.flamer) {
        //     Flame.SetActive(false);   
        // } else {
        //     Flame.SetActive(true);   

        // }
        if (SimVars.flamer)
        {
            particleSystem.Play();  // Start playing the particle system
        }
        else
        {
            particleSystem.Stop();  // Stop the particle system
        }
    }
}
