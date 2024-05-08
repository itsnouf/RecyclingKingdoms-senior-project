using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PetDatabase : ScriptableObject
{
    public Pet[] pets;
    public int PetCount
    {
        get { return pets.Length; }
    }

    public Pet GetPet(int index)
    {
        return pets[index];
    }
}
