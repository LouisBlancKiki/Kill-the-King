using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carte 
{
    public Carte_Data data;

    public GameObject objetCarte;

    public Carte(Carte_Data cartedataCible)
    {
        this.data = cartedataCible;
    }
}
