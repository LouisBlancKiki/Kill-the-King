using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouton_Fanion : MonoBehaviour
{
    public AccueilManager AM;
    public Elements.Joueur joueur;
    public int numFanion;

    private void OnMouseDown()
    {
        AM.DefinirFanionJoueur(joueur, numFanion);
    }
}
