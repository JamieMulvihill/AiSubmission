using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class terrainBuilder : MonoBehaviour
{

    public Mesh replace;
    public PolygonCollider2D col;
    public int seed = 15;
    public float Length = 6;
    public Vector2 scale = Vector2.one;
    public float scaleNoise = 3;
    Vector3[] verts;
    Vector2[] verts2d;
    Vector2[] uv;
    int[] ind;

    void Start()
    {
        col = GetComponent<PolygonCollider2D>();
        int POLYS = (Mathf.FloorToInt(Length/scale.x)+1)*2;//amount of vertical slices

        replace = new Mesh();
        verts = new Vector3[POLYS];
        verts2d = new Vector2[(POLYS/2)+2];
        uv = new Vector2[POLYS];
        ind = new int[((POLYS) - 2) * 3];//(POLYS-2) *3
        for (int i = 0; i < POLYS; i+=2)
        {
            float x = ((float)i / 2.0f) * scale.x;
            float y = 0;
            if (i < 10 / scale.x)
            {
                y = 0.5f * scale.y;
            }
            else {
                y = Mathf.PerlinNoise(x * scaleNoise, 0) * scale.y;
            }

            verts[i] = new Vector3(x, 0);
            verts[i+1] = new Vector3(x, y);
            verts2d[(i/2)+1] = new Vector2(x, y);
            uv[i] = new Vector2(x / scale.x * ((i / 2) % 2), 0);
            uv[i+1] = new Vector2(x / scale.x*((i/2)%2), 1);
        }
        verts2d[0] = new Vector3(0, 0);
        verts2d[(POLYS/2)+1] = new Vector3(Length, 0);
        int place = 0;
        for (int i = 0; i < (POLYS/2) - 1; i++)
        {
            int start = i * 2;
            //polygon a
            ind[place++] = start;
            ind[place++] = start + 1;
            ind[place++] = start + 2;
            //polygon b
            ind[place++] = start + 1;
            ind[place++] = start + 3;
            ind[place++] = start + 2;
        }

        replace.vertices = verts;
        replace.triangles = ind;
        replace.uv = uv;
        col.points = verts2d;
        GetComponent<MeshFilter>().mesh = replace;
    }

}
