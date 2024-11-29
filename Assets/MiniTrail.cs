using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTrail : MonoBehaviour {
    public LineRenderer originalLine;
    public LineRenderer thisLine;

    void Update(){
        int positionCount = originalLine.positionCount;
        thisLine.positionCount = positionCount;
        Vector3[] positions = new Vector3[positionCount];
        originalLine.GetPositions(positions);
        thisLine.SetPositions(positions);
        
        // thisLine.startWidth = CameraFollow.fullSystemZoom * 0.01f;
        // thisLine.endWidth = CameraFollow.fullSystemZoom * 0.01f;
    }
}
