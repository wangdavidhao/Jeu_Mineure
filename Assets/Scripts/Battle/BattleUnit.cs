using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
 	//Variable contenant les information de base du pokemon ciblé   
    [SerializeField] Pokemon_base _base;
    //Variable contenant le niveau du pokémon ciblé
    [SerializeField] int level;
    //Variable permettant de savoir si le pokémon est celui du joueur ou non, et donc d'afficher son sprite de dos ou de face
    public bool isPlayer;

    //Variable contenant le pokémon ciblé
    public Pokemon pokemon {get; set;}

    //Méthode permettant d'afficher le sprite de dos ou de face selon si celui-ci est celui du joueur ou non
    public void Setup()
    {
    	pokemon = new Pokemon(_base,level);
    	if(isPlayer)
    	{
    		GetComponent<Image>().sprite = pokemon._base.GetbackSprite();
    	}
    	else
    	{
    		GetComponent<Image>().sprite = pokemon._base.GetfrontSprite();
    	}

    }
}
