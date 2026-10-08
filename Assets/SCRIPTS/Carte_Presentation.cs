using UnityEngine;

public class Carte_Presentation
{
    public Vector3 position;
    public Quaternion rotation;

    public Carte_Presentation(Vector3 positionCible, Quaternion rotationCible)
    {
        position = positionCible;
        rotation = rotationCible;
    }
}