using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GenerateSprite : MonoBehaviour
{
    public SpriteRenderer temp;
    //public Sprite spriteA;
    //public Sprite spriteB;
    Vector2[] spritVertices;
    public PolygonCollider2D collider;
    bool A = true;
    float delay = 0f;
    // Start is called before the first frame update
    void Start()
    {

        //collider.points[0] = spriteVertices[0];
        //collider.points[1] = spriteVertices[1];
        //collider.points[2] = spriteVertices[2];
        //collider.points[3] = spriteVertices[3];
        //collider.points[4] = spriteVertices[4];
        DrawDebug();
        collider = transform.GetChild(0).GetComponent<PolygonCollider2D>();
        RefreshCollider(temp.sprite);

    }

    void RefreshCollider(Sprite sprite) {
        List<Vector2> tempPath = new List<Vector2>();

        //for (int i = 0; i < collider.pathCount; i++)
        //{
        //    collider.SetPath(i, tempPath);
        //}
        //collider.pathCount = sprite.GetPhysicsShapeCount();


        //List<Vector2> path = new List<Vector2>();
        //for (int i = 0; i < collider.pathCount; i++)
        //{
        //    path.Clear();
        //    sprite.GetPhysicsShape(i, path);
        //    collider.SetPath(i, path.ToArray());
        //}
       Vector2[] path = new Vector2[sprite.vertices.Length] ;
       // for (int i = sprite.vertices.Length -1; i >= 0; i--) {
            path = new Vector2[8] { sprite.vertices[0], sprite.vertices[2], sprite.vertices[1], sprite.vertices[3], sprite.vertices[6], sprite.vertices[7], sprite.vertices[5], sprite.vertices[4] };
       // }
        collider.SetPath(0, path);

        collider.points = path;
        //Vector2[] spriteVertices = sprite.vertices;
        // spritVertices[3].y = spritVertices[6].y;
        //spritVertices[4].y = spritVertices[5].y;
        //sprite.OverrideGeometry(spriteVertices, sprite.triangles);

    }
    void DrawDebug()
    {
        Sprite sprite = temp.sprite;

        ushort[] triangles = sprite.triangles;
        Vector2[] vertices = sprite.vertices;
        int a, b, c;

        // draw the triangles using grabbed vertices
        for (int i = 0; i < triangles.Length; i = i + 3)
        {
            a = triangles[i];
            b = triangles[i + 1];
            c = triangles[i + 2];

            //To see these you must view the game in the Scene tab while in Play mode
            Debug.DrawLine(vertices[a], vertices[b], Color.red, 2.5f);
            Debug.DrawLine(vertices[b], vertices[c], Color.red, 2.5f);
            Debug.DrawLine(vertices[c], vertices[a], Color.red, 2.5f);
        }
    }

    //void ChangeSprite()
    //{
    //    //Fetch the Sprite and vertices from the SpriteRenderer
    //    Sprite sprit = temp.sprite;
    //    Vector2[] spriteVertices = sprit.vertices;

    //    for (int i = 0; i < spriteVertices.Length; i++)
    //    {
    //        spriteVertices[i].x = Mathf.Clamp(
    //            (sprit.vertices[i].x - sprit.bounds.center.x -
    //                (sprit.textureRectOffset.x / sprit.texture.width) + sprit.bounds.extents.x) /
    //            (2.0f * sprit.bounds.extents.x) * sprit.rect.width,
    //            0.0f, sprit.rect.width);

    //        spriteVertices[i].y = Mathf.Clamp(
    //            (sprit.vertices[i].y - sprit.bounds.center.y -
    //                (sprit.textureRectOffset.y / sprit.texture.height) + sprit.bounds.extents.y) /
    //            (2.0f * sprit.bounds.extents.y) * sprit.rect.height,
    //            0.0f, sprit.rect.height);

    //        // Make a small change to the second vertex
    //        if (i == 2)
    //        {
    //            //Make sure the vertices stay inside the Sprite rectangle
    //            if (spriteVertices[i].x < sprit.rect.size.x - 5)
    //            {
    //                spriteVertices[i].x = spriteVertices[i].x + 5;
    //            }
    //            else spriteVertices[i].x = 0;
    //        }
    //    }
    //    //Override the geometry with the new vertices
    //    sprit.OverrideGeometry(spriteVertices, sprit.triangles);
    //}


    //// Update is called once per frame
    //void Update()
    //{
    //    //spriteVertices[0] = new Vector2(spriteVertices[0].x, spriteVertices[0].y - (.01f * Time.deltaTime));
    //    //temp.sprite.OverrideGeometry(spriteVertices, temp.sprite.triangles);

    //    if (A && Time.time - delay > 2.5f) {
    //        temp.sprite = spriteB;
    //        delay = Time.time;
    //        A = false;
    //        //DrawDebug();
    //        RefreshCollider(temp.sprite);
    //    }
    //    else if (!A && Time.time - delay > 2.5f) {
    //        temp.sprite = spriteA;
    //        delay = Time.time;
    //        A = true;
    //        //RefreshCollider(temp.sprite);
    //    }
    //    //ChangeSprite();
    //}
}
