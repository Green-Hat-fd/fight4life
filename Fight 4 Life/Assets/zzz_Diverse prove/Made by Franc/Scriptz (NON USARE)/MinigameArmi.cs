using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameArmi : MonoBehaviour
{
    [SerializeField]
    Animator animGrande;
    Animator animBarra;


    private void Awake()
    {
        animBarra = GetComponent<Animator>();
    }

    void Update()
    {
        float posizFinale = transform.position.x;


    #region ----DEBUG----
        //Entra ed esci
        if (Input.GetKeyDown(KeyCode.I) && !animGrande.GetBool("Attivato")) //In//
        {
            animGrande.SetBool("Attivato", true);
            animBarra.SetBool("Muoversi", true);
        }

        if (Input.GetKeyDown(KeyCode.O)) //Out//
            animGrande.SetBool("Attivato", false);
    #endregion

        //Controlla se l'input viene dato 
        if (Input.GetKeyDown(KeyCode.Space) && animGrande.GetBool("Attivato"))
        {
            animBarra.SetBool("Muoversi", false);
            StartCoroutine(TimerFinito());
    #region ----DEBUG----
            print("FINE = " + Mathf.Abs(posizFinale));
    #endregion
            StopCoroutine(TimerFinito());
        }
    }

    //Il timer che serve solo per far vedere il risultato quando premi spazio
    IEnumerator TimerFinito()
    {
        yield return new WaitForSeconds(1.5f);

        animGrande.SetBool("Attivato", false);
    }
}
