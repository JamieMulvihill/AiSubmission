using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[ExecuteInEditMode]
public class MeshGEN : MonoBehaviour
{

    public Mesh replace;
    public Rigidbody2D rb;
    public PolygonCollider2D col;
    public int SEGS = 6;
    Vector3[] verts;
    Vector2[] verts2d;
    Vector2[] uv;
    int[] ind;
    public float scale = 1;
    //public float SPEED = 10000;
    float start = 0;


    public void MeshGen(){
        col = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        start = Random.Range(0, 1000);
        replace = new Mesh();
        verts = new Vector3[SEGS];
        verts2d = new Vector2[SEGS];
        uv = new Vector2[SEGS];
        ind = new int[(SEGS - 2) * 3];
        for (int i = 0; i < SEGS; i++)
        {
            float rad = (float)i * ((Mathf.PI * 2) / SEGS);
            float x = Mathf.Sin(rad);
            float y = Mathf.Cos(rad);
            verts[i] = new Vector3(x, y) * scale * ScalingFunct(x+0.5f,y+0.5f);
            verts2d[i] = new Vector2(verts[i].x, verts[i].y);
            uv[i] = new Vector2((x+1)/2,(y+1f)/2);
        }
        int place = 0;
        for (int i = 1; i < SEGS - 1; i++)
        {
            ind[place++] = 0;
            ind[place++] = i;
            ind[place++] = i + 1;
        }
        replace.vertices = verts;
        replace.triangles = ind;
        replace.uv = uv;
        col.points = verts2d;
        GetComponent<MeshFilter>().mesh = replace;
    }

    private float ScalingFunct(float x, float y)
    {
        return 1f;// +(Mathf.PerlinNoise(start+x, start+y))*0.25f;
    }

    private void Update()
    {
        //rb.angularVelocity = (-SPEED);
    }
}
