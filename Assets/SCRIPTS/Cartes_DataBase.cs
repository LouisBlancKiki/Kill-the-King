using System.Collections.Generic;
using UnityEngine;
using static Elements;

[CreateAssetMenu(fileName = "Cartes_DataBase", menuName = "Cartes/nouvelle DataBase de cartes")]
public class Cartes_DataBase : ScriptableObject
{
    public List<Carte_Data> cartes_Liste;

    public Carte_Data TrouverCarte(NomCarte nomCible)
    {
        foreach (Carte_Data carteData in cartes_Liste)
        {
            if (carteData.nom == nomCible)
            {
                return carteData;
            }
        }
        Debug.LogError($"La carte {nomCible} n'existe pas dans la Database !");
        return null;
    }
}

