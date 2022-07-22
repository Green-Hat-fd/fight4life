using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoreCursoriMouse : MonoBehaviour
{
    //Il vettore dove andranno tutti i cursori (0 = default)
    public static Texture2D[] vettCursori;

    [SerializeField]
    Texture2D[] elencoDeiCursori;


    private void Awake()
    {
        for (int i = 0; i < elencoDeiCursori.Length; i++)
        {
            vettCursori[i] = elencoDeiCursori[i];
        }
    }

    //Tutte le funzioni che cambiano il cursore del mouse
    public static void CambiaCursore_Default()
    {
        Cursor.SetCursor(vettCursori[0], Vector2.zero, CursorMode.Auto);
    }
    
    public static void CambiaCursore_Selezione()
    {
        Cursor.SetCursor(vettCursori[1], Vector2.zero, CursorMode.Auto);
    }
}
