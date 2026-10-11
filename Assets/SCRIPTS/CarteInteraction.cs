using UnityEngine;

public class Carte_Interaction : MonoBehaviour
{
    public Carte carte;


    private void OnMouseDown()
    {
        Debug.Log($"Carte sélectionnée : {carte.data.nom}");
    }
}