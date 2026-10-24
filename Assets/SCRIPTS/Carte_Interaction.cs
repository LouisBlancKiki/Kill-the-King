using UnityEngine;

public class CarteInteraction : MonoBehaviour
{
    public Carte carte;
    public MainManager MM;

    private void OnMouseDown()
    {
        MM.SelectionnerCarte(carte);
    }
}