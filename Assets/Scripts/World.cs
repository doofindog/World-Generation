using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    private Sprite m_sprite;
    private Texture2D m_worldTexture;


    public void Awake()
    {
        m_sprite = GetComponent<SpriteRenderer>().sprite;
        m_worldTexture = m_sprite.texture;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleOnMouseDown();
        }
    }


    private void HandleOnMouseDown()
    {
        if (Camera.main == null)
        {
           return;
        }
        
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(mouseWorldPosition);
    }

    private void DrawPixel()
    {
    }
}
