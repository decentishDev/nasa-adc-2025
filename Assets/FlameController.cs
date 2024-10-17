using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject flame;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SimVars.enlargedProportions) {
            flame.SetActive(true);   
        } else {
            flame.SetActive(false);   

        }
    }
}
