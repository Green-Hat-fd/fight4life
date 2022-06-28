using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public bool isDragging;

    Vector3 posizOrig;
    GameObject personaggio;


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


            //Se entra a contatto con un personaggio
            /* if()
             * 
             */
        }
        else
        {
            transform.position = posizOrig;
        }

        GetComponent<SpriteRenderer>().color = mioCol;
    }

    public void OnMouseDown()
    {
        isDragging = true;
    }

    public void OnMouseUp()
    {
        isDragging = false;
    }

    public void DareDaMangiare(GameObject person)
    {
        personaggio = person;
    }
}
