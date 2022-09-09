using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public class RilevaSaveFile : MonoBehaviour
{
    [SerializeField]
    UnityEvent ev_seEsisteIlSalvat, ev_seNonEsisteNessunFile;

    string percorso_file;

    bool fileDiSalvatEsiste;


    private void Awake()
    {
        //Prende la posizione del file
        percorso_file = Application.dataPath + "/salvFile.txt";

        fileDiSalvatEsiste = File.Exists(percorso_file);
    }

    public void RilevaFileDiSalvataggio()
    {
        if (fileDiSalvatEsiste)
            ev_seEsisteIlSalvat.Invoke();
        else
            ev_seNonEsisteNessunFile.Invoke();
    }
}
