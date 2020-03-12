using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterSprite : MonoBehaviour
{
    public SpriteRenderer m_SpriteRenderer;
    public Sprite resetValue;
    Sprite reset;
    public PolygonCollider2D collider;
    public float change = 2;

    // Start is called before the first frame update
        Sprite sprite;
    void Start() {
        sprite = m_SpriteRenderer.sprite;
        reset = resetValue;
        ChangeSprite();
    }


    private void Update()
    {
        
    }
    void ChangeSprite(){

        ////Fetch the Sprite and vertices from the SpriteRenderer
        Vector2[] spriteVertices = sprite.vertices;
        Vector2[] resetVertices = sprite.vertices;

        for (int i = 0; i < spriteVertices.Length; i++)
        {
            spriteVertices[i].x = Mathf.Clamp((sprite.vertices[i].x - sprite.bounds.center.x -
                    (sprite.textureRectOffset.x / sprite.texture.width) + sprite.bounds.extents.x) /
                (2.0f * sprite.bounds.extents.x) * sprite.rect.width,
                0.0f, sprite.rect.width);

            spriteVertices[i].y = Mathf.Clamp(
                (sprite.vertices[i].y - sprite.bounds.center.y -
                    (sprite.textureRectOffset.y / sprite.texture.height) + sprite.bounds.extents.y) /
                (2.0f * sprite.bounds.extents.y) * sprite.rect.height,
                0.0f, sprite.rect.height);

            // Make a small change to the second vertex
            if (i == 3)
            {
                // spriteVertices[i] *= 0.2f;
                //Make sure the vertices stay inside the Sprite rectangle
                //if (spriteVertices[i].x < sprite.rect.size.x - 2)
                //{
                spriteVertices[i].y = spriteVertices[i - 2].y;
                //}
                //else spriteVertices[i].x = 0;
            }

            else if (i == 4) {
                spriteVertices[i].y = spriteVertices[0].y;
            }
        }
        
        //Override the geometry with the new vertices
        sprite.OverrideGeometry(spriteVertices, sprite.triangles);
        
    }
}
