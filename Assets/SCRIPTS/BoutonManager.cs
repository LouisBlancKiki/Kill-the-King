using UnityEngine;

public class BoutonManager : MonoBehaviour
{
    public GameManager GM;

    public void Bouton_Jouer()
    {
        GM.InitialiserNouvellePartie();
    }

    public void Bouton_FinDeTour()
    {
        
    }
}
