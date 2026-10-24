using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Elements;

public class PanneauActionManager : MonoBehaviour
{
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]
    public GameManager GM;

    [Space(10)]

    [Header("<b><color=#FFDC99>ENTETE ACTION</color></b>")]
    public GameObject entete_Action_Actif;
    [Space(10)]
    public GameObject entete_Action_P1;
    public GameObject entete_Action_P2;

    [Space(10)]

    [Header("<b><color=#FFDC99>BOUTONS ACTION</color></b>")]
    public GameObject zone_Bouton_Action_Actif;
    [Space(10)]
    public GameObject zone_Bouton_Action_P1;
    public GameObject zone_Bouton_Action_P2;
    [Space(10)]
    public GameObject bouton_Action_Prefab;
    [Space(10)]
    public GameObject bouton_Action_Actif;
    [Space(10)]
    public GameObject bouton_Action_P1;
    public GameObject bouton_Action_P2;
    [Space(10)]
    public float espaceEntreBoutonAction;
    [Space(10)]
    private Bouton_Action boutonActionSelectionne;
    [Space(10)]
    public float taille_bouton_Action_Selectionne;
    [Space(10)]
    public float duree_Pop_Bouton_Action;
    public float taille_Max_Pop_Bouton_Action;
    public float delai_Entre_Pop_Bouton_Action;
    [Space(10)]
    public Action actionSelectionee;
    private Coroutine AffichageBoutons_Coroutine;
    [Space(10)]
    public Color couleur_Action_Accessible;
    public Color couleur_Action_Inaccessible;

    [Space(10)]

    [Header("<b><color=#FFDC99>INFOS ACTION</color></b>")]

    public GameObject info_Action_P1;
    public GameObject info_Action_P2;
    [Space(10)]
    public GameObject info_Action_Actif;

    // ============================================================
    // PANNEAU ACTION
    // ============================================================
    public void AfficherPanneauAction(Carte carteCible)
    {
        //Debug.Log($"<b><color=#FFFF00> -> Affichage du panneau action : {carteCible.data.nom}</color></b>");

        if (AffichageBoutons_Coroutine != null)
        {
            StopCoroutine(AffichageBoutons_Coroutine);
            AffichageBoutons_Coroutine = null;
        }

        DetruireTousLesBoutonsAction();
        AfficherEnteteAction(carteCible);

        AffichageBoutons_Coroutine = StartCoroutine(InstancierBoutonAction_Coroutine(carteCible));
    }

    public void MasquerPanneauAction()
    {
        if (AffichageBoutons_Coroutine != null)
        {
            StopCoroutine(AffichageBoutons_Coroutine);
            AffichageBoutons_Coroutine = null;
        }

        MasquerEnteteAction();
        DetruireTousLesBoutonsAction();
    }

    // ============================================================
    // ENETE ACTIONS
    // ============================================================
    public void AfficherEnteteAction(Carte carteCible)
    {
        entete_Action_Actif.GetComponent<SpriteRenderer>().sprite = carteCible.data.sprite_EnteteAction;
    }

    public void MasquerEnteteAction()
    {
        entete_Action_Actif.GetComponent<SpriteRenderer>().sprite = null;
    }

    // ============================================================
    // BOUTONS ACTIONS
    // ============================================================
    public IEnumerator InstancierBoutonAction_Coroutine(Carte carteCible)
    {
        int nbBoutons = carteCible.data.liste_Sprite_Bouton_Action.Count;
        Vector3 positionCentre = zone_Bouton_Action_Actif.transform.position;

        for (int i = 0; i < nbBoutons; i++)
        {
            float decalage = espaceEntreBoutonAction + carteCible.data.liste_Sprite_Bouton_Action[i].bounds.size.x;
            float decalageX = (i - (nbBoutons - 1) / 2f) * decalage;
            Vector3 position = positionCentre + new Vector3(decalageX, 0f, 0f);

            GameObject bouton = Instantiate(bouton_Action_Prefab, position, Quaternion.identity, bouton_Action_Actif.transform);

            Bouton_Action boutonAction = bouton.GetComponent<Bouton_Action>();
            boutonAction.sprite_Desselectionne = carteCible.data.liste_Sprite_Bouton_Action[i];
            boutonAction.sprite_Selectionne = carteCible.data.liste_Sprite_BoutonSelect_Action[i];
            boutonAction.sprite_Info_Action = carteCible.data.liste_Sprite_Info_Action[i];
            boutonAction.action = carteCible.data.liste_Action[i];
            boutonAction.PAM = this;
            
            boutonAction.Deselectionner();

            boutonsActionAccessible(boutonAction);
            
            boutonAction.DemarrerAnimationPop();

            yield return new WaitForSeconds(delai_Entre_Pop_Bouton_Action);
        }
    }

    public void DetruireTousLesBoutonsAction()
    {
        DeselectionnerBoutonAction();

        for (int i = bouton_Action_Actif.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(bouton_Action_Actif.transform.GetChild(i).gameObject);
        }
    }

    // ============================================================
    // DEFINIR ACCESSIBILITE DES BOUTONS ACTION
    // ============================================================

    public bool boutonsActionAccessible(Bouton_Action boutonActionCible)
    {
        if (ActionPossible(boutonActionCible.action))
        {
            boutonActionCible.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            DegriserBoutonAction(boutonActionCible);
            return true;
        }
        else
        {
            boutonActionCible.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            GriserBoutonAction(boutonActionCible);
            return false;
        }
    }

    public bool ActionPossible(Action actionCible)
    {
        switch (actionCible)
        {
            case Action.Semer:
            case Action.Planter:
            case Action.Construire_Canon:
            case Action.Construire_Chapelle:
            case Action.Construire_Entrepot:
            case Action.Construire_Forge:
            case Action.Construire_Maison:
            case Action.Construire_Marche:
            case Action.Construire_Monument:
            case Action.Construire_Moulin:
            case Action.Construire_Rempart:
            case Action.Construire_Taverne:
            case Action.Construire_Tribunal:

                for (int x = 0; x < GM.PM.grillePlateau.GetLength(0); x++)
                {
                    for (int y = 0; y < GM.PM.grillePlateau.GetLength(1); y++)
                    {
                        if (GM.PM.EstInoccupeeEtAdjacenteAUneCaseOccupee(GM.PM.grillePlateau, x, y, GM.joueurActif))
                            return true;
                    }
                }

                Debug.Log($"<b><color=#DDCC00>Action {actionCible} inaccessible : aucune case libre adjacente.</color></b>");
                return false;

            case Action.Creuser:
                // A DEFINIR
                break;

            case Action.Recolter:

                foreach (Case caseCible in GM.RM.ObtenirRegistreCases(GM.joueurActif))
                {
                    if (caseCible.case_TypeTuile == TypeTuile.Champ && caseCible.niveau >= GM.PM.GetTuileData(caseCible.case_TypeTuile).tuileNiveauMinimumRecoltable)
                    {
                        return true;
                    }
                }
                Debug.Log($"<b><color=#DDCC00>Action {actionCible} inaccessible : aucun champ recoltable.</color></b>");
                return false;

            case Action.Couper:
                 // A DEFINIR
                break;

            case Action.Extraire:
                // A DEFINIR
                break;

        }

        return true;
    }

                

    public void GriserBoutonAction(Bouton_Action boutonActionCible)
    {
        boutonActionCible.sr_Image.color = couleur_Action_Inaccessible;
    }

    public void DegriserBoutonAction(Bouton_Action boutonActionCible)
    {
        boutonActionCible.sr_Image.color = couleur_Action_Accessible;
    }

    // ============================================================
    // DEFINIR ACTION SELECTIONEE
    // ============================================================
    public void DefinirActionSelectionnee(Action actionCible)
    {
        actionSelectionee = actionCible;
    }

    // ============================================================
    // SELECTION ET DESELECTION DES BOUTONS ACTIONS
    // ============================================================

    public void SelectionnerBoutonAction(Bouton_Action boutonCible)
    {
        if (boutonCible == null) return;

        if (boutonActionSelectionne == boutonCible)
        {
            DeselectionnerBoutonAction();
            return;
        }

        DeselectionnerBoutonAction();

        boutonActionSelectionne = boutonCible;
        boutonActionSelectionne.Selectionner();

        DefinirActionSelectionnee(boutonCible.action);
        AfficherInfoAction(boutonActionSelectionne);
        GM.PM.DefinirCaseSelectionables();
    }

    public void DeselectionnerBoutonAction()
    {
        if (boutonActionSelectionne == null) return;

        boutonActionSelectionne.Deselectionner();
        boutonActionSelectionne = null;

        MasquerInfoAction();
        GM.PM.RendreToutesLesCases_NonSelectionables();
    }

    public void AfficherInfoAction(Bouton_Action boutonCible)
    {
        info_Action_Actif.GetComponent<SpriteRenderer>().sprite = boutonCible.sprite_Info_Action;
    }

    public void MasquerInfoAction()
    {
        info_Action_Actif.GetComponent<SpriteRenderer>().sprite = null;
    }


    public void DemanderAction(Case caseCible)
    {
        switch (actionSelectionee)
        {
            case Action.Semer:
                GM.PM.PlacerTuile(caseCible, TypeTuile.Champ);
                break;

            case Action.Recolter:
                if(caseCible.niveau >= GM.PM.GetTuileData(TypeTuile.Champ).tuileNiveauMinimumRecoltable)
                {
                    GM.SM.ModifierScore(GM.joueurActif, Ressource.Nourriture, GM.PM.GetTuileData(TypeTuile.Champ).nbRessourcesRecolte);
                    GM.PM.SuprimerTuile(caseCible);
                }
                break;

            case Action.Planter:
                GM.PM.PlacerTuile(caseCible, TypeTuile.Foret);
                break;

            case Action.Couper:
                
                break;

            case Action.Creuser:
                if (caseCible.case_TypeTerrain == TypeTerrain.GisementDePierre)
                {
                    GM.PM.PlacerTuile(caseCible, TypeTuile.MineDePierre);
                }
                else if (caseCible.case_TypeTerrain == TypeTerrain.GisementDeCharbon)
                {
                    GM.PM.PlacerTuile(caseCible, TypeTuile.MineDeCharbon);
                }
                else if (caseCible.case_TypeTerrain == TypeTerrain.GisementDeMetal)
                {
                    GM.PM.PlacerTuile(caseCible, TypeTuile.MineDeMetal);
                }
                else return;

                break;

            case Action.Extraire:

                break;
        }

        ValiderFinAction();
    }
    // ============================================================
    // FIN D'ACTION
    // ============================================================

    public void ValiderFinAction()
    {
        GM.PM.RendreToutesLesCases_NonSelectionables();
        MasquerPanneauAction();
        StartCoroutine(GM.MM.DefausserCarte_Coroutine(GM.MM.carteSelectionnee));

    }
}
