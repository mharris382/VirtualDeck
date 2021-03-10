using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class BeizerController2 : MonoBehaviour
{
    public Transform start;

    public Transform end;

    public Transform controlStart;
    public Transform controlEnd;
    
    
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
        this.controlStart ??= start.CreateNewChild("CS");
        this.controlEnd ??= end.CreateNewChild("CE");
        
        Vector3 s = this.start.position;
        Vector3 e = this.end.position;
        Vector3 cs = this.controlStart.position;
        Vector3 ce = this.controlEnd.position;
        int n = 0;  
        var extentStart = s - cs;
        var extentEnd = e - ce;
        Vector3 s0 = s + (extentStart.normalized * startExtend);
        Vector3 e0 = e + (extentEnd.normalized * endExtend);
        points.Add(s0);
      
        float increment = 1 / (float) increments;
        for (float t = 0; t < 1; t +=  increment)
        {
            var ps1 = Vector3.Lerp(s, cs, t);
            var ps2 = Vector3.Lerp(cs, ce, t);
            var bs = Vector3.Lerp(ps1, ps2, t);
            
            var pe1 = Vector3.Lerp(cs, ce, t);
            var pe2 = Vector3.Lerp(ce, e, t);
            var be = Vector3.Lerp(pe1, pe2, t);

            var b = Vector3.Lerp(bs, be, t);
            points.Add(b);
            n++;
            if(n > increments+1) throw new Exception();
        }
        points.Add(e);
        points.Add(e0);
        line.positionCount = increments+4;
        line.SetPositions(points.ToArray());
        
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        this.start ??= transform.CreateNewChild("S");
        this.end ??= transform.CreateNewChild("E");
        this.controlStart ??= transform.CreateNewChild("C");
        Vector3 s = this.start.position;
        Vector3 e = this.end.position;
        Vector3 cs = this.controlStart.position;
        Vector3 ce = this.controlEnd.position;
        var clr = Color.blue;
        clr.a = 0.25f;
        Gizmos.color = clr;
        Gizmos.DrawSphere(s, .125f);
        Gizmos.DrawSphere(e, .125f);
        Gizmos.DrawLine(s, cs);    
        Gizmos.DrawLine(ce, e);
        Gizmos.DrawSphere(cs, .125f);
        Gizmos.DrawSphere(ce, .125f);
    }
}