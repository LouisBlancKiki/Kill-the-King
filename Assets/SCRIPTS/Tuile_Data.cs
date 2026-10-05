using UnityEngine;
using static Elements;

[CreateAssetMenu(fileName = "Tuile_Data_", menuName = "Tuiles/nouvelle tuile")]
public class Tuile_Data : ScriptableObject
{
    
    public TypeTuile type;
    public int pointDeVieMax;
    public int tuileNiveauMinimumRecoltable;
    public int tuileNiveauMaximum;
    public int nbRessourcesRecolte;
    public bool estEvolutif;

    [Header("<b><color=#FFDC99>VISUEL SELON LES NIVEAUX</color></b>")]
    public GameObject[] prefabsSelonNiveau;
}