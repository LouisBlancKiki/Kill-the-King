using UnityEngine;

public static class Elements
{
    public enum Joueur
    {
        Game,
        P1,
        P2
    }
    public enum TypeTerrain
    {
        Prairie,
        GisementDePierre,
        GisementDeCharbon,
        GisementDeMetal,
        Vide
    }
    public enum TypeTuile
    {
        Chateau,
        AileDeChateau,
        Champ,
        Foret,
        MineDePierre,
        MineDeCharbon,
        MineDeMetal,
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
        Monument,
        Vide
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
        Magistrat,
        Mendiant,
        Artilleur,
        Assassin
    }
    public enum Ressource
    {
        Nourriture,
        Bois,
        Pierre,
        Charbon,
        Metal,
        Piece
    }

    public enum Action
    {
        Semer,
        Recolter,
        Planter,
        Couper,
        Creuser,
        Extraire,
        Marchander,
        Recruter,
        Convertir,
        Celebrer,
        Promouvoir,
        Bannir,
        Bombarder,
        Manoeuvrer,
        Assassiner,
        Construire_Maison,
        Construire_Marche,
        Construire_Taverne,
        Construire_Entrepot,
        Construire_Forge,
        Construire_Moulin,
        Construire_Chapelle,
        Construire_Tribunal,
        Construire_Rempart,
        Construire_Canon,
        Construire_Monument,
        Demolir
    }

}



