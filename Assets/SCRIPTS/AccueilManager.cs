using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccueilManager : MonoBehaviour
{
    [Header("GAME OBJECTS")]
    public GameObject pageAcceuil;
    public GameObject[] liste_Fanion_P1;
    public GameObject[] liste_Fanion_P2;
    public Sprite[] listeBlasonsRois;

    [Space(10)]

    [Header("ANIMATORS")]
    public Animator animator_Fond;
    public Animator animator_Bout_Jouer;
    
    [Space(10)]

    [Header("POSITIONS DES FANIONS")]
    public float positionFanion_DefautY;
    public float positionFanion_SelectioneY;
    public float positionFanion_IndisponibleY;

    [Space(10)]

    [Header("FANIONS")]
    public int numCouleurSelectionee_P1;
    public int numCouleurSelectionee_P2;

    [Space(10)]

    [Header("BLASONS")]
    public GameObject blason_Roi_P1;
    public GameObject blason_Roi_P2;
    
    [Space(10)]

    [Header("NOMS")]
    public TMP_InputField champNom_P1;
    public TMP_InputField champNom_P2;


    void Start()
    {
        DefinirFanionJoueur(Elements.Joueur.P1, numCouleurSelectionee_P1);
        DefinirFanionJoueur(Elements.Joueur.P2, numCouleurSelectionee_P2);
    }

    public void OuvertureDesVoletsAcceuil()
    {
        animator_Fond.SetBool("ouvre", true);
        animator_Bout_Jouer.SetBool("disparait", true);

        Invoke("MasquerLesVoletsAcceuil", 3);
    }

    public void MasquerLesVoletsAcceuil()
    {
        pageAcceuil.SetActive(false);
    }

    public void DefinirFanionJoueur(Elements.Joueur joueurCible, int num)
    {
        GameObject fanion;
        GameObject fanionAdverse;

        if (joueurCible == Elements.Joueur.P1)
        {
            fanion = liste_Fanion_P1[numCouleurSelectionee_P1 - 1];
            fanion.transform.position = new Vector3(fanion.transform.position.x, positionFanion_DefautY, fanion.transform.position.z);

            fanionAdverse = liste_Fanion_P2[numCouleurSelectionee_P1 - 1];
            fanionAdverse.transform.position = new Vector3(fanionAdverse.transform.position.x, positionFanion_DefautY, fanionAdverse.transform.position.z);
            fanionAdverse.GetComponent<BoxCollider2D>().enabled = true;

            numCouleurSelectionee_P1 = num;
            fanion = liste_Fanion_P1[numCouleurSelectionee_P1 - 1];
            fanion.transform.position = new Vector3(fanion.transform.position.x, positionFanion_SelectioneY, fanion.transform.position.z);

            fanionAdverse = liste_Fanion_P2[numCouleurSelectionee_P1 - 1];
            fanionAdverse.transform.position = new Vector3(fanionAdverse.transform.position.x, positionFanion_IndisponibleY, fanionAdverse.transform.position.z);
            fanionAdverse.GetComponent<BoxCollider2D>().enabled = false;

            blason_Roi_P1.GetComponent<SpriteRenderer>().sprite = listeBlasonsRois[num - 1];
           // GM.imageRoi_P1.GetComponent<SpriteRenderer>().sprite = listeCadresRois[num - 1];
        }

        else if (joueurCible == Elements.Joueur.P2)
        {
            fanion = liste_Fanion_P2[numCouleurSelectionee_P2 - 1];
            fanion.transform.position = new Vector3(fanion.transform.position.x, positionFanion_DefautY, fanion.transform.position.z);

            fanionAdverse = liste_Fanion_P1[numCouleurSelectionee_P2 - 1];
            fanionAdverse.transform.position = new Vector3(fanionAdverse.transform.position.x, positionFanion_DefautY, fanionAdverse.transform.position.z);
            fanionAdverse.GetComponent<BoxCollider2D>().enabled = true;

            numCouleurSelectionee_P2 = num;
            fanion = liste_Fanion_P2[numCouleurSelectionee_P2 - 1];
            fanion.transform.position = new Vector3(fanion.transform.position.x, positionFanion_SelectioneY, fanion.transform.position.z);

            fanionAdverse = liste_Fanion_P1[numCouleurSelectionee_P2 - 1];
            fanionAdverse.transform.position = new Vector3(fanionAdverse.transform.position.x, positionFanion_IndisponibleY, fanionAdverse.transform.position.z);
            fanionAdverse.GetComponent<BoxCollider2D>().enabled = false;

            blason_Roi_P2.GetComponent<SpriteRenderer>().sprite = listeBlasonsRois[num - 1];
            //GM.imageRoi_P2.GetComponent<SpriteRenderer>().sprite = listeCadresRois[num - 1];
        }
    }
}

