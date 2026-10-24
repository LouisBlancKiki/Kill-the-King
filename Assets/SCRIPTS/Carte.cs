using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elements;

[System.Serializable]
public class Carte 
{
    public Carte_Data data;

    public Joueur proprietaire_Carte;

    public GameObject gameObject;

    public Carte(Carte_Data cartedataCible, Joueur proprietaireCible)
    {
        this.data = cartedataCible;
        proprietaire_Carte = proprietaireCible;
    }

}
