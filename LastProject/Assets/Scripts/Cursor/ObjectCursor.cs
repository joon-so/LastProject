using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCursor : MonoBehaviour
{
    public Texture2D cursorArrow;
    public Texture2D cursorObject;

    void Start()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorObject, Vector2.zero, CursorMode.ForceSoftware);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }
}