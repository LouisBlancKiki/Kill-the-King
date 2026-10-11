using UnityEngine;

public class CarteInteraction : MonoBehaviour
{
    public Carte carte;
    public MainManager mainManager;

    private void OnMouseDown()
    {
        mainManager.SelectionnerCarte(carte);
    }
}