using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Elements;

public class MainManager : MonoBehaviour
{
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]
    public GameManager GM;

    [Space(10)]

    [Header("<b><color=#FFDC99>TEMPS</color></b>")]
    public float tempsEntreChaquePioche;
    public float tempsEntreChaqueDefausse;
    public float tempsDeplacementDesCartes;

    [Space(10)]

    [Header("<b><color=#FFDC99>PRESENTATION DES CARTES</color></b>")]
    public float espacementDesCartes;
    public float hauteurCourbeMain;
    public float angleMaxCourbe;

    [Space(10)]

    [Header("<b><color=#FFDC99>SELECTION CARTE</color></b>")]
    public float hauteurSelection;
    public float facteurTailleSelection;
    public float tempsAnimationSelection;
    [Space(10)]

    [System.NonSerialized] public Carte carteSelectionnee;
     
    /*
        private Carte _carteSelectionnee;

        public Carte carteSelectionnee
        {
            get => _carteSelectionnee;
            set
            {
                Debug.Log($"Modification de carteSelectionnee : " +
                          $"{(_carteSelectionnee == null ? "null" : "non null")} " +
                          "-> " +
                          $"{(value == null ? "null" : "non null")}\n" +
                          System.Environment.StackTrace);

                _carteSelectionnee = value;
            }
        }*/

    [Space(10)]

    [Header("<b><color=#FFDC99>COULEUR CARTE</color></b>")]

    public Color couleur_Carte_Accessible;
    public Color couleur_Carte_Inaccessible;

    private void Awake()
    {
        carteSelectionnee = null;
    }
    // ============================================================
    // DEBUT DE TOUR
    // ============================================================
    public void DemanderDistributionDesCartes()
    {
        StartCoroutine(DistributionDesCartes_Coroutine());
    }
    private IEnumerator DistributionDesCartes_Coroutine()
    {
        yield return StartCoroutine(PiocherCartesDeDebutDeTour_Coroutine());
        
        GM.DebuterTourDuJoueur();
    }

    private IEnumerator PiocherCartesDeDebutDeTour_Coroutine()
    {
        int nombreDeCartes = GM.nbCartePiocheesAuDebutDuTour;

        Debug.Log($"<b><color=#80D8FF> -> {GM.joueurActif} pioche {nombreDeCartes} carte(s) ! </color></b>");

        for (int i = 0; i < nombreDeCartes; i++)
        {
            yield return StartCoroutine(PiocherCarte_Coroutine());

            yield return new WaitForSeconds(tempsEntreChaquePioche);
        }
    }

    // ============================================================
    // PIOCHE D'UNE CARTE
    // ============================================================

    private IEnumerator PiocherCarte_Coroutine()
    {
        Carte cartePiochee;
        Vector3 positionDepart;
        
        if (GM.DM.liste_Deck_Actif.Count == 0)
        {
            Debug.Log($"<b><color=#80D8FF> -> Recyclage de la défausse de {GM.joueurActif} !</color></b>");

            yield return StartCoroutine(GM.DM.DemnderRecyclageDefausse_Coroutine());

            if (GM.DM.liste_Deck_Actif.Count == 0)
            {
                Debug.LogWarning($"<b><color=#FFFF00>!!! Impossible de piocher : le deck et la défausse de {GM.joueurActif} sont vides !</color></b>");
                yield break;
            }
        }

        cartePiochee = GM.DM.liste_Deck_Actif[GM.DM.liste_Deck_Actif.Count - 1];
        GM.DM.liste_Deck_Actif.RemoveAt(GM.DM.liste_Deck_Actif.Count - 1);
        GM.DM.liste_Main_Actif.Add(cartePiochee);
        cartePiochee.gameObject.transform.SetParent(GM.DM.main_Actif.transform);
        positionDepart = GM.DM.zone_Deck_Actif.position;


        SpriteRenderer sr = cartePiochee.gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = cartePiochee.data.sprite_Carte;
        sr.sortingLayerName = "CartePremierPlan";
        sr.sortingOrder = (GM.DM.liste_Main_Actif.Count - 1) * 10;
        cartePiochee.gameObject.transform.position = positionDepart;

        Dictionary<Carte, Carte_Presentation> dico_presentationFinale = CalculerPositionsDesCartes(GM.DM.liste_Main_Actif);

        foreach (Carte carte in GM.DM.liste_Main_Actif)
        {
            Vector3 positionActuelle = carte.gameObject.transform.position;
            Carte_Presentation presentationFinale = dico_presentationFinale[carte];
            Vector3 positionFinale = presentationFinale.position;

            StartCoroutine(DeplacerCarteVersLaMain_Coroutine(carte, positionActuelle, positionFinale, presentationFinale.rotation));
        }

        yield return new WaitForSeconds(tempsDeplacementDesCartes);
    }

    // ============================================================
    // CALCUL DES POSITIONS
    // ============================================================

    private Dictionary<Carte, Carte_Presentation> CalculerPositionsDesCartes(List<Carte> main)
    {
        Dictionary<Carte, Carte_Presentation> dico_presentationsFinales = new Dictionary<Carte, Carte_Presentation>();

        int nombreDeCartes = main.Count;

        for (int i = 0; i < nombreDeCartes; i++)
        {
            Carte carte = main[i];

            float positionX = (i - (nombreDeCartes - 1) / 2f) * espacementDesCartes; 
            
            float progression = 0.5f;

            if (nombreDeCartes > 1)
            {
                progression = (float)i / (nombreDeCartes - 1);
            }

            float hauteur = Mathf.Sin(progression * Mathf.PI) * hauteurCourbeMain;

            Vector3 positionLocale = new Vector3(positionX, hauteur, 0f);

            Vector3 positionMonde = GM.DM.zone_Main_Actif.TransformPoint(positionLocale);

            float angle = Mathf.Lerp(angleMaxCourbe, -angleMaxCourbe, progression);

            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            Carte_Presentation presentation = new Carte_Presentation(positionMonde, rotation);

            dico_presentationsFinales.Add(carte, presentation);
        }

        return dico_presentationsFinales;
    }

    // ============================================================
    // DEPLACEMENT CARTE
    // ============================================================

    private IEnumerator DeplacerCarteVersLaMain_Coroutine(Carte carteCible, Vector3 positionDepart, Vector3 positionArrivee, Quaternion rotationArrivee)
    {
        float temps = 0f;

        Quaternion rotationDepart = carteCible.gameObject.transform.rotation;

        while (temps < tempsDeplacementDesCartes)
        {
            temps += Time.deltaTime;

            float progression = temps / tempsDeplacementDesCartes;

            carteCible.gameObject.transform.position = Vector3.Lerp(positionDepart, positionArrivee, progression);

            carteCible.gameObject.transform.rotation = Quaternion.Lerp(rotationDepart, rotationArrivee, progression);

            yield return null;
        }

        Vector3 positionFinale = positionArrivee;

        if (carteCible == carteSelectionnee)
        {
            positionFinale += Vector3.up * hauteurSelection;
        }

        carteCible.gameObject.transform.position = positionFinale;

        carteCible.gameObject.transform.rotation = rotationArrivee;
        carteCible.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Main";

    }

    // ============================================================
    // DEFAUSSER MAIN DU JOUEUR
    // ============================================================
    
    public  IEnumerator DefausserToutesLesCartesDeLaMain_Coroutine()
    {
        if (carteSelectionnee != null)
        {
            yield return StartCoroutine(DeselectionnerCarte_Coroutine(carteSelectionnee));
            carteSelectionnee = null;
        }

        foreach (Carte carte in GM.DM.liste_Main_Actif)
        {
            carte.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        while (GM.DM.liste_Main_Actif.Count > 0)
        {
            Carte carte = GM.DM.liste_Main_Actif[0];
            
            Vector3 positionDepart = carte.gameObject.transform.position;

            carte.gameObject.GetComponent<SpriteRenderer>().sortingOrder = GM.DM.liste_Defausse_Actif.Count * 10;
            
            yield return StartCoroutine(DeplacerCarteVersDefausse_Coroutine(carte, positionDepart, GM.DM.zone_Defaussse_Actif.position));

            yield return new WaitForSeconds(tempsEntreChaqueDefausse);
        }
    }

    // ============================================================
    // DEFAUSSER CARTE SELECTIONNEE
    // ============================================================
    
    public IEnumerator DefausserCarte_Coroutine(Carte carteCible)
    {
        yield return StartCoroutine(DeselectionnerCarte_Coroutine(carteCible));

        carteCible.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        Vector3 positionDepart = carteCible.gameObject.transform.position;

        yield return StartCoroutine(DeplacerCarteVersDefausse_Coroutine(carteCible, positionDepart, GM.DM.zone_Defaussse_Actif.position));

        yield return StartCoroutine(ReorganiserMain_Coroutine());
    }

    // ============================================================
    // REORGANISER LA MAIN
    // ============================================================

    private IEnumerator ReorganiserMain_Coroutine()
    {
        Dictionary<Carte, Carte_Presentation> dico_presentationsFinales = CalculerPositionsDesCartes(GM.DM.liste_Main_Actif);

        foreach (Carte carte in GM.DM.liste_Main_Actif)
        {
            Vector3 positionActuelle = carte.gameObject.transform.position;
            Carte_Presentation presentationFinale = dico_presentationsFinales[carte];

            StartCoroutine(DeplacerCarteVersLaMain_Coroutine(carte, positionActuelle, presentationFinale.position, presentationFinale.rotation));
        }

        yield return new WaitForSeconds(tempsDeplacementDesCartes);
    }
    // ============================================================
    // DEPLACER CARTE VERS LA DEFAUSSE
    // ============================================================

    private IEnumerator DeplacerCarteVersDefausse_Coroutine(Carte carteCible, Vector3 positionDepart, Vector3 positionArrivee)
    {
        float temps = 0f;
        SpriteRenderer sr = carteCible.gameObject.GetComponent<SpriteRenderer>();
        Quaternion rotationDepart = carteCible.gameObject.transform.rotation;
        Quaternion rotationArrivee = Quaternion.identity;

        sr.sortingLayerName = "CartePremierPlan"; 

        while (temps < tempsDeplacementDesCartes)
        {
            temps += Time.deltaTime;

            float progression = temps / tempsDeplacementDesCartes;

            carteCible.gameObject.transform.position = Vector3.Lerp(positionDepart, positionArrivee, progression);

            carteCible.gameObject.transform.rotation = Quaternion.Lerp(rotationDepart, rotationArrivee, progression);

            yield return null;
        }

        carteCible.gameObject.transform.position = positionArrivee;
        carteCible.gameObject.transform.rotation = rotationArrivee;
        
        DegriserCarte(carteCible);
        carteCible.gameObject.transform.SetParent(GM.DM.defausse_Actif.transform);
        
        sr.sortingLayerName = "Defausse";
        sr.sortingOrder = (GM.DM.liste_Defausse_Actif.Count - 1) * 10;

        GM.DM.liste_Main_Actif.Remove(carteCible);
        GM.DM.liste_Defausse_Actif.Add(carteCible);
    }

    // ============================================================
    // SELECTION DES CARTE 
    // ============================================================
    
    public void SelectionnerCarte(Carte carte)
    {
        // Cliquer sur la carte déjà sélectionnée
        if (carteSelectionnee == carte)
        {
            DeselectionnerCarte(carte);
            carteSelectionnee = null;
            return;
        }
        
        // Désélectionner l'ancienne
        if (carteSelectionnee != null)
        {
            DeselectionnerCarte(carteSelectionnee);
        }

        // Sélectionner la nouvelle
        carteSelectionnee = carte;

        StartCoroutine(AnimerSelectionCarte_Coroutine(carte, true));

        Debug.Log($"<b><color=#80D8FF> -> Carte sélectionnée : {carte.data.nom}</color></b>");

        GM.PAM.AfficherPanneauAction(carte);
    }
    
    public void DeselectionnerCarte(Carte carte)
    {
        //
        if (carte == null)
        {
            Debug.LogError("DeselectionnerCarte : la carte reçue est null !");
            return;
        }

        StartCoroutine(DeselectionnerCarte_Coroutine(carte));
        
        
    }

    private IEnumerator DeselectionnerCarte_Coroutine(Carte carte)
    {
        GM.PAM.MasquerPanneauAction();
        yield return StartCoroutine(AnimerSelectionCarte_Coroutine(carte, false));
    }

    private IEnumerator AnimerSelectionCarte_Coroutine(Carte carte, bool selection)
    {
        Transform transformCarte = carte.gameObject.transform;

        Vector3 positionDepart = transformCarte.position;
        Vector3 echelleDepart = transformCarte.localScale;

        Vector3 positionArrivee = positionDepart;
        Vector3 echelleArrivee = echelleDepart;

        if (selection)
        {
            transformCarte.GetComponent<SpriteRenderer>().sortingOrder = 1000;

            positionArrivee += Vector3.up * hauteurSelection;
            echelleArrivee *= facteurTailleSelection;
        }
        else
        {
            transformCarte.GetComponent<SpriteRenderer>().sortingOrder = GM.DM.liste_Main_Actif.IndexOf(carte) * 10;
            positionArrivee -= Vector3.up * hauteurSelection;
            echelleArrivee /= facteurTailleSelection;
        }
        
        float temps = 0f;

        while (temps < tempsAnimationSelection)
        {
            temps += Time.deltaTime;

            float progression = temps / tempsAnimationSelection;

            transformCarte.position = Vector3.Lerp(
                positionDepart,
                positionArrivee,
                progression
            );

            transformCarte.localScale = Vector3.Lerp(
                echelleDepart,
                echelleArrivee,
                progression
            );

            yield return null;
        }

        transformCarte.position = positionArrivee;
        transformCarte.localScale = echelleArrivee;

    }

    // ============================================================
    // DEFINIR ACCESSIBILITEE DES CARTES
    // ============================================================

    public void DefinirAccessibiliteDesCarte()
    {
        foreach (Carte carteCible in GM.DM.liste_Main_Actif)
        {
            if (CarteAccessible(carteCible))
            {
                carteCible.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                DegriserCarte(carteCible);
            }
            else
            {
                carteCible.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                GriserCarte(carteCible);
            }
        }
    }

    public bool CarteAccessible(Carte carteCible)
    {
        // Verification du score de nourriture
        if (carteCible.data.coutEnNourriture > GM.SM.ObtenirScore(GM.joueurActif).scoreNourriture)
        {
            Debug.Log($"<b><color=#80D8FF>!!! carte {carteCible.data.nom } désactivée : pas assez de Nourriture </color></b>");
            return false;
        }
        if (carteCible.data.coutEnBois > GM.SM.ObtenirScore(GM.joueurActif).scoreBois)
        {
            Debug.Log($"<b><color=#80D8FF>!!! carte {carteCible.data.nom} désactivée : pas assez de Bois </color></b>");
            return false;
        }
        if (carteCible.data.coutEnPierre > GM.SM.ObtenirScore(GM.joueurActif).scorePierre)
        {
            Debug.Log($"<b><color=#80D8FF>!!! carte {carteCible.data.nom} désactivée : pas assez de Pierre </color></b>");
            return false;
        }
        if (carteCible.data.coutEnCharbon > GM.SM.ObtenirScore(GM.joueurActif).scoreCharbon)
        {
            Debug.Log($"<b><color=#80D8FF>!!! carte {carteCible.data.nom} désactivée : pas assez de Charbon </color></b>");
            return false;
        }
        if (carteCible.data.coutEnMetal > GM.SM.ObtenirScore(GM.joueurActif).scoreMetal)
        {
            Debug.Log($"<b><color=#80D8FF>!!! carte {carteCible.data.nom} désactivée : pas assez de Metal </color></b>");
            return false;
        }

        return true;
        /*
        // Verification du nombre de taverne
        if (carteCible.template.nom == CarteTemplate.NomCarte.Tavernier && PM.NombreDeTuilesDuType(Case.TypeTuile.Taverne, GM.joueurActif) < GD2P.nbMinTavernePourActiver)
        {
            Debug.Log("--> carte " + carte.template.nom + " : bloquée - pas de Taverne -");
            selectionnable = false;
        }

        // Verification du nombre de marché
        if (carteCible.template.nom == CarteTemplate.NomCarte.Marchand && PM.NombreDeTuilesDuType(Case.TypeTuile.Marche, GM.joueurActif) < GD2P.nbMinMarchePourActiver)
        {
            Debug.Log("--> carte " + (carte.template.nom + " : bloquée - pas de Marché -"));
            selectionnable = false;
        }

        // Verification du nombre de forge
        if (carteCible.template.nom == CarteTemplate.NomCarte.Forgeron && PM.NombreDeTuilesDuType(Case.TypeTuile.Forge, GM.joueurActif) < GD2P.nbMinForgePourActiver)
        {
            Debug.Log("--> carte " + carte.template.nom + " : bloquée - pas de Forge -");
            selectionnable = false;
        }

        // Verification du nombre de canon
        if (carteCible.template.nom == CarteTemplate.NomCarte.Artilleur && PM.NombreDeTuilesDuType(Case.TypeTuile.Canon, GM.joueurActif) < GD2P.nbMinCanonPourActiver)
        {
            Debug.Log(carte.template.nom + " : bloquée - pas de Canon -");
            selectionnable = false;
        }

        // Definir le grisage de la carte 
        if (selectionnable && CRG.txt_CoutEnNourritureCarte != null)
        {
            CRG.GetComponent<SpriteRenderer>().color = GD2P.couleur_CarteAccessible;
            CRG.txt_CoutEnNourritureCarte.GetComponent<TMPro.TMP_Text>().color = GD2P.couleur_CarteAccessible;
        }
        else if (CRG.txt_CoutEnNourritureCarte != null)
        {
            CRG.GetComponent<SpriteRenderer>().color = GD2P.couleur_CarteInaccessible;
            CRG.txt_CoutEnNourritureCarte.GetComponent<TMPro.TMP_Text>().color = GD2P.couleur_CarteInaccessible;
        }
        */
        
    }

    public void GriserCarte(Carte carteCible)
    {
        carteCible.gameObject.GetComponent<SpriteRenderer>().color = couleur_Carte_Inaccessible;
    }

    public void DegriserCarte(Carte carteCible)
    {
        carteCible.gameObject.GetComponent<SpriteRenderer>().color = couleur_Carte_Accessible;
    }
}