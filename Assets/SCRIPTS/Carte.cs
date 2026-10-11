using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Carte 
{
    public Carte_Data data;

    public GameObject gameObject;

    public Carte(Carte_Data cartedataCible)
    {
        this.data = cartedataCible;
    }

}
