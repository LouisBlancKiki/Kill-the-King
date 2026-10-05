using UnityEngine;

public static class Elements
{
    public enum Joueur
    {
        Game,
        P1,
        P2
    }

    public enum TypeTuile
    {
        Prairie,
        GisementDePierre,
        GisementDeCharbon,
        GisementDeMetal,
        MineDePierre,
        MineDeCharbon,
        MineDeMetal,
        Chateau,
        AileDeChateau,
        Champ,
        Foret,
        Maison,
        Marche,
        Taverne,
        Entrepot,
        Forge,
        Moulin,
        Chapelle,
        Academie,
        Rempart,
        Canon,
        Monument
    }

    public enum TypeCarte
    {
        Ressource,
        Personnage,
    }

    public enum NomCarte
    {
        Nourriture,
        Bois,
        Pierre,
        Charbon,
        Metal,
        Paysan,        
        Bucheron,
        Mineur,
        Marchand,
        Tavernier,
        Charpentier,
        Batisseur,
        Architecte,
        Forgeron,
        Aumonier,
        Philosophe,
        Mendiant,
        Artilleur,
        Bourreau
    }
}



