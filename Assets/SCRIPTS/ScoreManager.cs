using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Drawing;
using UnityEngine;
using static Elements;

public class ScoreManager : MonoBehaviour
{
    // MANAGERS
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]
    public GameManager GM;

    [Space(10)]

    [Header("<b><color=#FFDC99>SCORES DES JOUEURS</color></b>")]
    public ScoreJoueur score_P1 = new ScoreJoueur();
    public ScoreJoueur score_P2 = new ScoreJoueur();


    [Space(10)]

    // STOCKS 
    [Header("<b><color=#FFDC99>STOCKS</color></b>")]
    public GameObject stock_Prefab;
    public float posInit_Stock;
    public float espacement_Stock;
    [Space(10)]
    public int nbDeStocksMax;
    public int nbDeSegmentsParStock;
    [Space(10)]
    private List<GameObject> liste_Stocks_Nourriture_P1 = new List<GameObject>();
    private List<GameObject> liste_Stocks_Bois_P1 = new List<GameObject>();
    private List<GameObject> liste_Stocks_Pierre_P1 = new List<GameObject>();
    private List<GameObject> liste_Stocks_Charbon_P1 = new List<GameObject>();
    private List<GameObject> liste_Stocks_Metal_P1 = new List<GameObject>();
    [Space(10)]
    private List<GameObject> liste_Stocks_Nourriture_P2 = new List<GameObject>();
    private List<GameObject> liste_Stocks_Bois_P2 = new List<GameObject>();
    private List<GameObject> liste_Stocks_Pierre_P2 = new List<GameObject>();
    private List<GameObject> liste_Stocks_Charbon_P2 = new List<GameObject>();
    private List<GameObject> liste_Stocks_Metal_P2 = new List<GameObject>();

    [Space(10)]

    // SEGMENTS
    [Header("<b><color=#FFDC99>SEGMENTS</color></b>")]
    public GameObject segment_Prefab;
    public float posInit_Segment;
    public float espacement_Segment;
    public Vector3 tailleSegment;
    [Space(10)]
    private List<GameObject> liste_Segments_Nourriture_P1 = new List<GameObject>();
    private List<GameObject> liste_Segments_Bois_P1 = new List<GameObject>();
    private List<GameObject> liste_Segments_Pierre_P1 = new List<GameObject>();
    private List<GameObject> liste_Segments_Charbon_P1 = new List<GameObject>();
    private List<GameObject> liste_Segments_Metal_P1 = new List<GameObject>();
    [Space(10)]
    private List<GameObject> liste_Segments_Nourriture_P2 = new List<GameObject>();
    private List<GameObject> liste_Segments_Bois_P2 = new List<GameObject>();
    private List<GameObject> liste_Segments_Pierre_P2 = new List<GameObject>();
    private List<GameObject> liste_Segments_Charbon_P2 = new List<GameObject>();
    private List<GameObject> liste_Segments_Metal_P2 = new List<GameObject>();
    [Space(10)]
    public float dureeAnimationSegment;
    public float delaiEntreSegments;
    public float taillePopSegment;
    [Space(10)]
    private Dictionary<List<GameObject>, Coroutine> animationsSegments = new Dictionary<List<GameObject>, Coroutine>();

    [Space(10)]

    // BARRE RESSOURCE
    [Header("<b><color=#FFDC99>BARRE RESSOURCE P1</color></b>")]

    public GameObject barreRessource_Nourriture_P1;
    public GameObject barreRessource_Bois_P1;
    public GameObject barreRessource_Pierre_P1;
    public GameObject barreRessource_Charbon_P1;
    public GameObject barreRessource_Metal_P1;

    [Space(10)]

    // BARRE RESSOURCE
    [Header("<b><color=#FFDC99>BARRE RESSOURCE P2</color></b>")]

    public GameObject barreRessource_Nourriture_P2;
    public GameObject barreRessource_Bois_P2;
    public GameObject barreRessource_Pierre_P2;
    public GameObject barreRessource_Charbon_P2;
    public GameObject barreRessource_Metal_P2;

    [Space(10)]

    // NOURRITURE
    [Header("<b><color=#FFDC99>NOURRITURE</color></b>")]
    public Color couleurRessource_Nourriture;
    public Sprite iconeRessource_Nourriture;

    [Space(10)]

    // BOIS
    [Header("<b><color=#FFDC99>BOIS</color></b>")]
    public Color couleurRessource_Bois;
    public Sprite iconeRessource_Bois;

    [Space(10)]

    // PIERRE
    [Header("<b><color=#FFDC99>PIERRE</color></b>")]
    public Color couleurRessource_Pierre;
    public Sprite iconeRessource_Pierre;

    [Space(10)]

    // CHARBON
    [Header("<b><color=#FFDC99>CHARBON</color></b>")]
    public Color couleurRessource_Charbon;
    public Sprite iconeRessource_Charbon;

    [Space(10)]

    // METAL
    [Header("<b><color=#FFDC99>METAL</color></b>")]
    public Color couleurRessource_Metal;
    public Sprite iconeRessource_Metal;

    void Start()
    {
        
    }
    public ScoreJoueur ObtenirScore(Joueur joueurCible)
    {
        switch (joueurCible)
        {
            case Joueur.P1:
                return score_P1;

            case Joueur.P2:
                return score_P2;

            default:
                return null;
        }
    }

    public void InitialiserBarreRessouurce()
    {
        AfficherIconeBarreRessource();
        InstancierTousLesStocks();
        MiseAJourNbStock(Joueur.P1);
        MiseAJourNbStock(Joueur.P2);
    }

    private void AfficherIconeBarreRessource()
    {
        ConfigurerIconesBarreRessource(barreRessource_Nourriture_P1, couleurRessource_Nourriture, iconeRessource_Nourriture);
        ConfigurerIconesBarreRessource(barreRessource_Bois_P1, couleurRessource_Bois, iconeRessource_Bois);
        ConfigurerIconesBarreRessource(barreRessource_Pierre_P1, couleurRessource_Pierre, iconeRessource_Pierre);
        ConfigurerIconesBarreRessource(barreRessource_Charbon_P1, couleurRessource_Charbon, iconeRessource_Charbon);
        ConfigurerIconesBarreRessource(barreRessource_Metal_P1, couleurRessource_Metal, iconeRessource_Metal);

        ConfigurerIconesBarreRessource(barreRessource_Nourriture_P2, couleurRessource_Nourriture, iconeRessource_Nourriture);
        ConfigurerIconesBarreRessource(barreRessource_Bois_P2, couleurRessource_Bois, iconeRessource_Bois);
        ConfigurerIconesBarreRessource(barreRessource_Pierre_P2, couleurRessource_Pierre, iconeRessource_Pierre);
        ConfigurerIconesBarreRessource(barreRessource_Charbon_P2, couleurRessource_Charbon, iconeRessource_Charbon);
        ConfigurerIconesBarreRessource(barreRessource_Metal_P2, couleurRessource_Metal, iconeRessource_Metal);
    }

    private void ConfigurerIconesBarreRessource(GameObject barre, Color couleur, Sprite icone)
    {
        barre.transform.Find("porteIcone_BarreRessource").GetComponent<SpriteRenderer>().color = couleur;
        barre.transform.Find("iconeRessource").GetComponent<SpriteRenderer>().sprite = icone;
    }

    private void InstancierTousLesStocks()
    {
        InstancierStocksEtSegments(barreRessource_Nourriture_P1, liste_Stocks_Nourriture_P1, liste_Segments_Nourriture_P1, couleurRessource_Nourriture);
        InstancierStocksEtSegments(barreRessource_Bois_P1, liste_Stocks_Bois_P1, liste_Segments_Bois_P1, couleurRessource_Bois);
        InstancierStocksEtSegments(barreRessource_Pierre_P1, liste_Stocks_Pierre_P1, liste_Segments_Pierre_P1, couleurRessource_Pierre);
        InstancierStocksEtSegments(barreRessource_Charbon_P1, liste_Stocks_Charbon_P1, liste_Segments_Charbon_P1, couleurRessource_Charbon);
        InstancierStocksEtSegments(barreRessource_Metal_P1, liste_Stocks_Metal_P1, liste_Segments_Metal_P1, couleurRessource_Metal);

        InstancierStocksEtSegments(barreRessource_Nourriture_P2, liste_Stocks_Nourriture_P2, liste_Segments_Nourriture_P2, couleurRessource_Nourriture);
        InstancierStocksEtSegments(barreRessource_Bois_P2, liste_Stocks_Bois_P2, liste_Segments_Bois_P2, couleurRessource_Bois);
        InstancierStocksEtSegments(barreRessource_Pierre_P2, liste_Stocks_Pierre_P2, liste_Segments_Pierre_P2, couleurRessource_Pierre);
        InstancierStocksEtSegments(barreRessource_Charbon_P2, liste_Stocks_Charbon_P2, liste_Segments_Charbon_P2, couleurRessource_Charbon);
        InstancierStocksEtSegments(barreRessource_Metal_P2, liste_Stocks_Metal_P2, liste_Segments_Metal_P2, couleurRessource_Metal);
    }

    private void InstancierStocksEtSegments(GameObject barreRessourceCible, List<GameObject> listeStocks, List<GameObject> listeSegments, Color couleurRessource)
    {
        for (int i = 0; i < nbDeStocksMax; i++)
        {
            GameObject stock = Instantiate(stock_Prefab, barreRessourceCible.transform);
            stock.transform.localPosition = new Vector3((i * espacement_Stock) + posInit_Stock, 0f, 0f);
            stock.SetActive(false);
            listeStocks.Add(stock);

            for (int j = 0; j < nbDeSegmentsParStock; j++)
            {
                GameObject segment = Instantiate(segment_Prefab, stock.transform);
                segment.transform.localPosition = new Vector3(posInit_Segment + (j * espacement_Segment), 0f, 0f);
                segment.GetComponent<SpriteRenderer>().color = couleurRessource;
                segment.SetActive(false);
                listeSegments.Add(segment);
            }
        }
    }

    public void InitialiserDonneesDebutDePartie(Joueur joueurCible)
    {
        ModifierScore(joueurCible, Ressource.Piece, GM.nbPieces_DebutDePartie);
        ModifierScore(joueurCible, Ressource.Nourriture, GM.nbNourritures_DebutDePartie);
        ModifierScore(joueurCible, Ressource.Bois, GM.nbBois_DebutDePartie);
        ModifierScore(joueurCible, Ressource.Pierre, GM.nbPierres_DebutDePartie);
        ModifierScore(joueurCible, Ressource.Charbon, GM.nbCharbons_DebutDePartie);
        ModifierScore(joueurCible, Ressource.Metal, GM.nbMetals_DebutDePartie);
    }

    private int CapaciteStockage(int nbEntrepots)
    {
        return (nbEntrepots + 1) * nbDeSegmentsParStock;
    }

    private void MiseAJourNbStock(Joueur joueurCible)
    {
        ScoreJoueur score = ObtenirScore(joueurCible);
        if (score == null) return;

        int nbEntrepots = score.nbEntrepots;
        int capaciteStockage = CapaciteStockage(nbEntrepots);

        List<GameObject>[] listesStocks;
        Ressource[] ressources = { Ressource.Nourriture, Ressource.Bois, Ressource.Pierre, Ressource.Charbon, Ressource.Metal };

        if (joueurCible == Joueur.P1)
        {
            listesStocks = new List<GameObject>[] { liste_Stocks_Nourriture_P1, liste_Stocks_Bois_P1, liste_Stocks_Pierre_P1, liste_Stocks_Charbon_P1, liste_Stocks_Metal_P1 };
        }
        else
        {
            listesStocks = new List<GameObject>[] { liste_Stocks_Nourriture_P2, liste_Stocks_Bois_P2, liste_Stocks_Pierre_P2, liste_Stocks_Charbon_P2, liste_Stocks_Metal_P2 };
        }

        for (int i = 0; i < ressources.Length; i++)
        {
            AfficherStocks(listesStocks[i], nbEntrepots);

            List<GameObject> segments = ObtenirListeSegments(joueurCible, ressources[i]);

            switch (ressources[i])
            {
                case Ressource.Nourriture:
                    AjusterScore(ref score.scoreNourriture, capaciteStockage, segments);
                    break;
                case Ressource.Bois:
                    AjusterScore(ref score.scoreBois, capaciteStockage, segments);
                    break;
                case Ressource.Pierre:
                    AjusterScore(ref score.scorePierre, capaciteStockage, segments);
                    break;
                case Ressource.Charbon:
                    AjusterScore(ref score.scoreCharbon, capaciteStockage, segments);
                    break;
                case Ressource.Metal:
                    AjusterScore(ref score.scoreMetal, capaciteStockage, segments);
                    break;
            }
        }
    }

    private void AjusterScore(ref int scoreRessource, int capaciteStockage, List<GameObject> listeSegments)
    {
        int ancienScore = scoreRessource;
        int nouveauScore = Mathf.Clamp(ancienScore, 0, capaciteStockage);

        if (ancienScore == nouveauScore) return;

        scoreRessource = nouveauScore;

        StartCoroutine(AnimerSegments(ancienScore, nouveauScore, listeSegments));
    }

    private void AfficherStocks(List<GameObject> listeStocks, int nbEntrepots)
    {
        int nbStocks = Mathf.Clamp(nbEntrepots + 1, 1, listeStocks.Count);

        for (int i = 0; i < listeStocks.Count; i++)
        {
            listeStocks[i].SetActive(i < nbStocks);
        }
    }

    public void ModifierScore(Joueur joueurCible, Ressource ressourceCible, int quantite)
    {
        ScoreJoueur score = ObtenirScore(joueurCible);

        if (score == null) return;

        Debug.Log($"<b><color=#FF5500> -> {joueurCible} modifie son score de {quantite} point(s) de {ressourceCible}</color></b>");

        if (ressourceCible == Ressource.Piece)
        {
            score.scorePiece = Mathf.Clamp(score.scorePiece + quantite, 0, GM.limiteDePiece);
            return;
        }

        int capaciteStockage = CapaciteStockage(score.nbEntrepots);
        int ancienScore;
        int nouveauScore;

        switch (ressourceCible)
        {
            case Ressource.Nourriture:
                ancienScore = score.scoreNourriture;
                nouveauScore = Mathf.Clamp(ancienScore + quantite, 0, capaciteStockage);
                score.scoreNourriture = nouveauScore;
                break;

            case Ressource.Bois:
                ancienScore = score.scoreBois;
                nouveauScore = Mathf.Clamp(ancienScore + quantite, 0, capaciteStockage);
                score.scoreBois = nouveauScore;
                break;

            case Ressource.Pierre:
                ancienScore = score.scorePierre;
                nouveauScore = Mathf.Clamp(ancienScore + quantite, 0, capaciteStockage);
                score.scorePierre = nouveauScore;
                break;

            case Ressource.Charbon:
                ancienScore = score.scoreCharbon;
                nouveauScore = Mathf.Clamp(ancienScore + quantite, 0, capaciteStockage);
                score.scoreCharbon = nouveauScore;
                break;

            case Ressource.Metal:
                ancienScore = score.scoreMetal;
                nouveauScore = Mathf.Clamp(ancienScore + quantite, 0, capaciteStockage);
                score.scoreMetal = nouveauScore;
                break;

            default:
                return;
        }

        List<GameObject> listeSegments = ObtenirListeSegments(joueurCible, ressourceCible);
        StartCoroutine(AnimerSegments(ancienScore, nouveauScore, listeSegments));

        GM.MM.DefinirAccessibiliteDesCarte();
    }

    private List<GameObject> ObtenirListeSegments(Joueur joueurCible, Ressource ressourceCible)
    {
        switch (joueurCible)
        {
            case Joueur.P1:
                switch (ressourceCible)
                {
                    case Ressource.Nourriture: return liste_Segments_Nourriture_P1;
                    case Ressource.Bois: return liste_Segments_Bois_P1;
                    case Ressource.Pierre: return liste_Segments_Pierre_P1;
                    case Ressource.Charbon: return liste_Segments_Charbon_P1;
                    case Ressource.Metal: return liste_Segments_Metal_P1;
                }
                break;

            case Joueur.P2:
                switch (ressourceCible)
                {
                    case Ressource.Nourriture: return liste_Segments_Nourriture_P2;
                    case Ressource.Bois: return liste_Segments_Bois_P2;
                    case Ressource.Pierre: return liste_Segments_Pierre_P2;
                    case Ressource.Charbon: return liste_Segments_Charbon_P2;
                    case Ressource.Metal: return liste_Segments_Metal_P2;
                }
                break;
        }

        return null;
    }

    private IEnumerator AnimerSegments(int ancienScore, int nouveauScore, List<GameObject> listeSegments)
    {
        if (nouveauScore > ancienScore)
        {
            for (int i = ancienScore; i < nouveauScore; i++)
            {
                GameObject segment = listeSegments[i];
                SpriteRenderer sprite = segment.GetComponent<SpriteRenderer>();
                Vector3 tailleFinale = tailleSegment;

                segment.SetActive(true);
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
                segment.transform.localScale = tailleFinale * 0.5f;

                yield return StartCoroutine(PopLeSegment(segment, sprite, tailleFinale, true));
                yield return new WaitForSeconds(delaiEntreSegments);
            }
        }
        else if (nouveauScore < ancienScore)
        {
            for (int i = ancienScore - 1; i >= nouveauScore; i--)
            {
                GameObject segment = listeSegments[i];
                SpriteRenderer sprite = segment.GetComponent<SpriteRenderer>();
                Vector3 tailleInitiale = tailleSegment;

                yield return StartCoroutine(PopLeSegment(segment, sprite, tailleInitiale, false));
                segment.SetActive(false);
                yield return new WaitForSeconds(delaiEntreSegments);
            }
        }
    }

    private IEnumerator PopLeSegment(GameObject segment, SpriteRenderer sprite, Vector3 tailleCible, bool apparition)
    {
        float progression = 0f;
        Color couleur = sprite.color;
        float alphaDepart = apparition ? 0f : 1f;
        float alphaFin = apparition ? 1f : 0f;
        Vector3 tailleDepart = apparition ? tailleCible * taillePopSegment : tailleCible;
        Vector3 tailleFin = apparition ? tailleCible : tailleCible * taillePopSegment;

        while (progression < 1f)
        {
            progression += Time.deltaTime / dureeAnimationSegment;
            float t = Mathf.Clamp01(progression);

            sprite.color = new Color(couleur.r, couleur.g, couleur.b, Mathf.Lerp(alphaDepart, alphaFin, t));
            segment.transform.localScale = Vector3.Lerp(tailleDepart, tailleFin, t);

            yield return null;
        }

        sprite.color = new Color(couleur.r, couleur.g, couleur.b, alphaFin);
        segment.transform.localScale = tailleFin;
    }
}
