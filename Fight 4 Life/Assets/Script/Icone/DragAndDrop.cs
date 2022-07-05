using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    bool isDragging;
    Vector3 posizOrig;


    private void Awake()
    {
        posizOrig = transform.position;
    }

    void Update()
    {
        Color mioCol = Color.white;  //Variabile temporanea che serve a cambiare trasparenza

        if (isDragging)
        {
            //Prende la posizione del mouse
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            //Muove l'oggetto
            transform.Translate(mousePosition);

            //Riduce la trasparenza
            mioCol.a = 0.6f;
        }
        else
        {
            //Torna alla posizione iniziale se non sta venendo trascinato
            transform.position = posizOrig;
        }

        GetComponent<SpriteRenderer>().color = mioCol;
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    public bool LeggiDrag()
    {
        return isDragging;
    }
}
