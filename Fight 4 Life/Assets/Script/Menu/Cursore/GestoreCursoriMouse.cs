using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestoreCursoriMouse : MonoBehaviour
{
    //Il vettore dove andranno tutti i cursori (0 = default)
    //[SerializeField]
    public static Texture2D[] vettCursori = new Texture2D[50];
    //Il vettore 2D dove andranno tutti i cursori (0 = default)
    [SerializeField]
    public static Vector2[] vettHotspot = new Vector2[50];

    [SerializeField]
    Texture2D[] elencoDeiCursori;
    [SerializeField]
    Vector2[] hotspotDeiCursori;


    private void Awake()
    {
        for (int i = 0; i < elencoDeiCursori.Length; i++)
            vettCursori[i] = elencoDeiCursori[i];

        for (int i = 0; i < hotspotDeiCursori.Length; i++)
            vettHotspot[i] = hotspotDeiCursori[i];

        //Mette all'inizio della scena la freccia (default) come cursore 
        CambiaCursore_Default();
    }

    #region Tutte le funzioni che cambiano il cursore del mouse
    
    public void CambiaCursore_Default()
    {
        Cursor.SetCursor(vettCursori[0], vettHotspot[0], CursorMode.Auto);
    }

    public void CambiaCursore_Selezione()
    {
        Cursor.SetCursor(vettCursori[1], vettHotspot[1], CursorMode.Auto);
    }

    public void CambiaCursore_Prendi()
    {
        Cursor.SetCursor(vettCursori[2], vettHotspot[2], CursorMode.Auto);
    }

    public void CambiaCursore_Caricamento()
    {
        Cursor.SetCursor(vettCursori[3], vettHotspot[3], CursorMode.Auto);
    }

    #endregion

    
    public static void CambiaCursoreScelto(int c)
    {
        Cursor.SetCursor(vettCursori[c], vettHotspot[c], CursorMode.Auto);
    }
}
