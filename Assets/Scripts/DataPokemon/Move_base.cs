using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new Move")]

public class Move_base : ScriptableObject
{
    //Informations sur l'attaque
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;
    [SerializeField] int status;
    [SerializeField] int setup;

    //Getter permettant de récupérer les informations sur l'attaque 
    public string GetName()
    {
    	return name;
    }
    public string GetDescription()
    {
    	return description;
    }
    public PokemonType GetType1()
    {
    	return type1;
    }
    public PokemonType GetType2()
    {
    	return type2;
    }
    public int GetPower()
    {
    	return power;
    }
    public int GetAccuracy()
    {
    	return accuracy;
    }
    public int GetPP()
    {
    	return pp;
    }
    public int GetStatus()
    {
    	return status;
    }
    public int GetSetup()
    {
    	return setup;
    }


}
