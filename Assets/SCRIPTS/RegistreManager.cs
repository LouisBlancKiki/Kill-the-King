using System.Collections.Generic;
using UnityEngine;
using static Elements;

public class RegistreManager : MonoBehaviour
{
    public List<Carte> registreCartes_P1 = new List<Carte>();
    public List<Carte> registreCartes_P2 = new List<Carte>();


    public List<Case> registreCases_Game = new List<Case>();
    public List<Case> registreCases_P1 = new List<Case>();
    public List<Case> registreCases_P2 = new List<Case>();

    public void AjouterCase(Joueur joueur, Case caseCible)
    {
        if (caseCible == null)
            return;

        List<Case> registre = ObtenirRegistreCases(joueur);

        if (registre != null && !registre.Contains(caseCible))
        {
            registre.Add(caseCible);
            caseCible.proprietaire_Case = joueur;
        }
    }

    public void SuprimerCase(Joueur joueur, Case caseCible)
    {
        if (caseCible == null)
            return;

        List<Case> registre = ObtenirRegistreCases(joueur);

        if (registre != null)
            registre.Remove(caseCible);
    }

    public void TransfererCase(Case caseCible, Joueur nouveauProprietaire)
    {
        if (caseCible == null)
            return;

        SuprimerCase(caseCible.proprietaire_Case, caseCible);
        AjouterCase(nouveauProprietaire, caseCible);
    }

    public List<Case> ObtenirRegistreCases(Joueur joueur)
    {
        switch (joueur)
        {
            case Joueur.Game:
                return registreCases_Game;
            case Joueur.P1:
                return registreCases_P1;
            case Joueur.P2:
                return registreCases_P2;
            default:
                return null;
        }
    }
    public void AjouterCarte(Joueur joueur, Carte carte)
    {
        if (carte == null || joueur == Joueur.Game)
            return;

        List<Carte> registre = ObtenirRegistreCartes(joueur);

        if (registre != null && !registre.Contains(carte))
        {
            registre.Add(carte);
            carte.proprietaire_Carte = joueur;
        }
    }

    public void SuprimerCarte(Joueur joueur, Carte carte)
    {
        if (carte == null)
            return;

        switch (joueur)
        {
            case Joueur.P1:
                registreCartes_P1.Remove(carte);
                break;

            case Joueur.P2:
                registreCartes_P2.Remove(carte);
                break;
        }
    }

    public void TransfererCarte(Carte carte, Joueur nouveauProprietaire)
    {
        if (carte == null || nouveauProprietaire == Joueur.Game)
            return;

        SuprimerCarte(carte.proprietaire_Carte, carte);
        AjouterCarte(nouveauProprietaire, carte);
    }

    public int CompterCartes(Joueur joueur, NomCarte nomCarte)
    {
        int nombre = 0;
        List<Carte> registre = ObtenirRegistreCartes(joueur);

        if (registre == null)
            return nombre;

        foreach (Carte carte in registre)
        {
            if (carte != null && carte.data != null && carte.data.nom == nomCarte)
                nombre++;
        }

        return nombre;
    }

    private List<Carte> ObtenirRegistreCartes(Joueur joueur)
    {
        switch (joueur)
        {
            case Joueur.P1:
                return registreCartes_P1;
            case Joueur.P2:
                return registreCartes_P2;
            default:
                return null;
        }
    }
}