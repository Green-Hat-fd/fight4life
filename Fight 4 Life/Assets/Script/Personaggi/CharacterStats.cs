using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //Statistiche
    float salute, fame, sete, stanchezza;

    //Variabili per cambio di statistiche
    [SerializeField]
    bool mangiato = false,
         bevuto = false;


    private void Update()
    {
        /*
         * ----Nota: mettere ancora il sistema che rileva quando hai mangiato o bevuto
         */

        //[...] se ha mangiato
        if (mangiato)
        {
            /* Fa quello che deve fare
             * per MANGIARE
             */
            print(gameObject.name + ": GNAM");

            mangiato = false;
        }

        //[...] se ha bevuto
        if (bevuto)
        {
            /* Fa quello che deve fare
             * per BERE
             */
            print(gameObject.name + ": GLU GLU GLU");

            bevuto = false;
        }
    }

    #region Variabili Set personali

    public void ScriviMangiato(bool m)
    {
        mangiato = m;
    }

    public void ScriviBevuto(bool b)
    {
        bevuto = b;
    }

    #endregion
}
