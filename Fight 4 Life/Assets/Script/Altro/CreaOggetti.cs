using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaOggetti : MonoBehaviour
{
    /* 
     * Nota: NP (o np) sta per "Nuova Partita)
     */

    
    //Crea un oggetto (col nome nelle parentesi),
    //lo fa rimanere quando cambia scena
    //e gli assegna un tag
    public void CreaOggettoSegnalatoreNP()
    {
        GameObject obj_np = new GameObject(">--Crea nuova partita--<");

        DontDestroyOnLoad(obj_np);

        obj_np.tag = "Nuova-Partita";
    }


    public void CreaOggettoSegnalatoreGoodEnd()
    {
        GameObject obj_fb = new GameObject(">--Finale buono--<");

        DontDestroyOnLoad(obj_fb);

        obj_fb.tag = "Nuova-Partita";
    }


    public void CreaOggettoSegnalatoreBadEnd()
    {
        GameObject obj_fc = new GameObject(">--Finale cattivo--<");

        DontDestroyOnLoad(obj_fc);

        obj_fc.tag = "Nuova-Partita";
    }
}
