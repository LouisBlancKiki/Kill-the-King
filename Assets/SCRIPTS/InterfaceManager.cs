using UnityEngine;
using TMPro;
using static Elements;

public class InterfaceManager : MonoBehaviour
{
    public GameManager GM;

    public TMP_Text txt_nbTourDeJeu;
    public int nbTourDeJeu;

    public GameObject environnement_Joueur;

    public GameObject imageProfil_P1;
    public TMP_Text txt_nomProfil_P1;

    public GameObject imageProfil_P2;
    public TMP_Text txt_nomProfil_P2;


    void Start()
    {
        nbTourDeJeu = 0;
        Afficher_nbTourDeJeu();
        
    }

    public void Afficher_ProfilDesJoueurs()
    {
        imageProfil_P1.GetComponent<SpriteRenderer>().sprite = GM.imageJoueur_P1;
        txt_nomProfil_P1.text = GM.nomJoueur_P1;

        imageProfil_P2.GetComponent<SpriteRenderer>().sprite = GM.imageJoueur_P2;
        txt_nomProfil_P2.text = GM.nomJoueur_P2;
    }

    public void Afficher_nbTourDeJeu()
    {
        txt_nbTourDeJeu.text = nbTourDeJeu.ToString();
    }

    public void AfficherEnvironnementJoueur(Joueur joueurCible)
    {
        if (joueurCible == Joueur.P1)
        {
            environnement_Joueur.SetActive(true);
            environnement_Joueur.GetComponent<SpriteRenderer>().color = GM.couleur_P1;
        }
        else if (joueurCible == Joueur.P2)
        {
            environnement_Joueur.SetActive(true);
            environnement_Joueur.GetComponent<SpriteRenderer>().color = GM.couleur_P2;
        }
        else if (joueurCible == Joueur.Game)
        {
            environnement_Joueur.SetActive(false);
        }
    }
}
