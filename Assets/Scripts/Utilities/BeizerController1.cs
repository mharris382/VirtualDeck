using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class BeizerController1 : MonoBehaviour
{
    public Transform start;

    public Transform end;

    public Transform control;
    
    public LineRenderer line;

    [Range(3, 200)]
    public int increments = 60;

    public float startExtend = 1;
    public float endExtend = 1;
    
    void Update()
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 origin = transform.position;
        this.line ??= GetComponent<LineRenderer>();
        this.start ??= transform.CreateNewChild("S");
        this.end ??= transform.CreateNewChild("E");
        this.control ??= transform.CreateNewChild("C");
        Vector3 s = this.start.position;
        Vector3 e = this.end.position;
        Vector3 c = this.control.position;
        int n = 0;
        var extentStart = s - c;
        var extentEnd = e - c;
        Vector3 s0 = s + (extentStart.normalized * startExtend);
        Vector3 e0 = e + (extentEnd.normalized * endExtend);
        points.Add(s0);
        float increment = 1 / (float) increments;
        for (float t = 0; t < 1; t +=  increment)
        {
            var p1 = Vector3.Lerp(s, c, t);
            var p2 = Vector3.Lerp(c, e, t);
            var b = Vector3.Lerp(p1, p2, t);
            points.Add(b);
            n++;
            if(n > increments+1) throw new Exception();
        }
        points.Add(e);
        points.Add(e0);
        line.positionCount = increments+3;
        line.SetPositions(points.ToArray());
        
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        this.start ??= transform.CreateNewChild("S");
        this.end ??= transform.CreateNewChild("E");
        this.control ??= transform.CreateNewChild("C");
        Vector3 s = this.start.position;
        Vector3 e = this.end.position;
        Vector3 c = this.control.position;
        var clr = Color.blue;
        clr.a = 0.25f;
        Gizmos.color = clr;
        Gizmos.DrawWireSphere(s, .125f);
        Gizmos.DrawWireSphere(e, .125f);
        Gizmos.DrawLine(s, c);
        Gizmos.DrawLine(c, e);
        Gizmos.DrawWireSphere(c, .125f);
    }
}


public static class TransformExtensions
{
    public static Transform CreateNewChild(this Transform t, string name)
    {
        var go = new GameObject(name);
        go.transform.parent = t;
        return go.transform;
    }
}