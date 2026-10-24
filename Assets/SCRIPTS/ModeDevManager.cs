using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elements;



public class ModeDevManager : MonoBehaviour
{
    public GameManager GM;

    public int nbRessources;


    public void DemanderModificationDeRessource(Joueur joueurCible, Ressource ressourceCible, int quantite)
    {
        Debug.Log($"<b><color=#9900FF> !!! CHEAT !!! </color></b>");
        GM.SM.ModifierScore(joueurCible, ressourceCible, quantite);
    }


    // NOURRITURE
    public void Ajouter_Nourriture_P1()
    {
        DemanderModificationDeRessource(Joueur.P1, Ressource.Nourriture, nbRessources);
    }
    public void Ajouter_Nourriture_P2()
    {
        DemanderModificationDeRessource(Joueur.P2, Ressource.Nourriture, nbRessources);
    }
    public void Retirer_Nourriture_P1()
    {
        DemanderModificationDeRessource(Joueur.P1, Ressource.Nourriture, -nbRessources);
    }
    public void Retirer_Nourriture_P2()
    {
        DemanderModificationDeRessource(Joueur.P2, Ressource.Nourriture, -nbRessources);
    }

    // BOIS
    public void Ajouter_Bois_P1()
    {
        DemanderModificationDeRessource(Joueur.P1, Ressource.Bois, nbRessources);
    }
    public void Ajouter_Bois_P2()
    {
        DemanderModificationDeRessource(Joueur.P2, Ressource.Bois, nbRessources);
    }
    public void Retirer_Bois_P1()
    {
        DemanderModificationDeRessource(Joueur.P1, Ressource.Bois, -nbRessources);
    }
    public void Retirer_Bois_P2()
    {
        DemanderModificationDeRessource(Joueur.P2, Ressource.Bois, -nbRessources);
    }

    // PIERRE
    public void Ajouter_Pierre_P1()
    {
        DemanderModificationDeRessource(Joueur.P1, Ressource.Pierre, nbRessources);
    }
    public void Ajouter_Pierre_P2()
    {
        DemanderModificationDeRessource(Joueur.P2, Ressource.Pierre, nbRessources);
    }
    public void Retirer_Pierre_P1()
    {
        DemanderModificationDeRessource(Joueur.P1, Ressource.Pierre, -nbRessources);
    }
    public void Retirer_Pierre_P2()
    {
        DemanderModificationDeRessource(Joueur.P2, Ressource.Pierre, -nbRessources);
    }

    // CHARBON
    public void Ajouter_Charbon_P1()
    {
        DemanderModificationDeRessource(Joueur.P1, Ressource.Charbon, nbRessources);
    }
    public void Ajouter_Charbon_P2()
    {
        DemanderModificationDeRessource(Joueur.P2, Ressource.Charbon, nbRessources);
    }
    public void Retirer_Charbon_P1()
    {
        DemanderModificationDeRessource(Joueur.P1, Ressource.Charbon, -nbRessources);
    }
    public void Retirer_Charbon_P2()
    {
        DemanderModificationDeRessource(Joueur.P2, Ressource.Charbon, -nbRessources);
    }
    // METAL
    public void Ajouter_Metal_P1()
    {
        DemanderModificationDeRessource(Joueur.P1, Ressource.Metal, nbRessources);
    }
    public void Ajouter_Metal_P2()
    {
        DemanderModificationDeRessource(Joueur.P2, Ressource.Metal, nbRessources);
    }
    public void Retirer_Metal_P1()
    {
        DemanderModificationDeRessource(Joueur.P1, Ressource.Metal, -nbRessources);
    }
    public void Retirer_Metal_P2()
    {
        DemanderModificationDeRessource(Joueur.P2, Ressource.Metal, -nbRessources);
    }




   

    
}
