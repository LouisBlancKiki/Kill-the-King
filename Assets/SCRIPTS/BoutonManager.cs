using UnityEngine;

public class BoutonManager : MonoBehaviour
{
    public GameManager GM;

    public Color couleur_Bout_Accessible;
    public Color couleur_Bout_Inaccessible;

    public void Bouton_Jouer()
    {
        GM.InitialiserNouvellePartie();
    }

    public void Bouton_DebutDeTour()
    {
        GM.ConfirmationJoueurPret();
    }

    public void Bouton_FinDeTour()
    {
        GM.DemanderLaFinDeTour();
    }
}
