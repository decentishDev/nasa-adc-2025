using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellites : MonoBehaviour {
    public Transform DSS24;
    public Transform DSS34;
    public Transform DSS54;
    public Transform WPSA;

    public Vector3[] sphericalPositions = new Vector3[4]{
        new Vector3(35.3399f, -116.875f, 0.951499f),
        new Vector3(-35.3985f, 148.982f, 0.69202f),
        new Vector3(40.4256f, -4.2541f, 0.837051f),
        new Vector3(37.9273f, -75.475f, -0.019736f)
    };

    private const float EarthRadius = 6378.137f;

    void Start(){
        DSS24.localPosition = SphericalToCartesian(sphericalPositions[0]) / 1000f;
        DSS34.localPosition = SphericalToCartesian(sphericalPositions[1]) / 1000f;
        DSS54.localPosition = SphericalToCartesian(sphericalPositions[2]) / 1000f;
        WPSA.localPosition = SphericalToCartesian(sphericalPositions[3]) / 1000f;
    }

    private Vector3 SphericalToCartesian(Vector3 spherical){
        float latRad = Mathf.Deg2Rad * spherical.x;
        float lonRad = Mathf.Deg2Rad * spherical.y;
        float radius = EarthRadius + spherical.z;

        float x = radius * Mathf.Cos(latRad) * Mathf.Cos(lonRad);
        float y = radius * Mathf.Cos(latRad) * Mathf.Sin(lonRad);
        float z = radius * Mathf.Sin(latRad);

        return new Vector3(x, z, y);
    }
}
