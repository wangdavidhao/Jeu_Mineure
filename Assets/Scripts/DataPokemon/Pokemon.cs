using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pokemon
{
	//On a forcément besoin des informations de base d'un pokémon pour le créer
    public Pokemon_base _base {get; set;}
    //Niveau du pokémon
    public int level {get; set;}

    //Liste des attaques du pokémons
    public List<Move> Moves {get; set;}
    //Point de vie du pokémon
    public int HP {get; set;}

    public Pokemon(Pokemon_base pbase, int plevel)
    {
    	_base = pbase;
    	level = plevel;

    	//Les HP du pokémon sont maximals à a création
    	HP = Mathf.FloorToInt(2*(_base.GetHp() * level) / 100f ) + 10;

    	//Créer une liste d'attaque qui est assigné au pokémon
    	Moves = new List<Move>();
    	//On apprend une nouvelle attaque au pokémon s'il atteint le niveau requis et s'il n'a pas déjà 4 attaques
    	foreach(var move in _base.GetLearnableMoves())
    	{
    		//Si le niveau du pokémon est supérieur au niveau qu'il faut avoir pour une attaque, alrs on lui assigne cette attaque
    		if(move.GetLevel() <= level)
    		{
    			Moves.Add(new Move(move.GetMoveBase()));
    		}
    		//On arrête de lui assigner des attaques s'il a déjà 4 attaques
    		if(Moves.Count >= 4)
    		{
    			break;
    		}
    	}
    }

    public int GetAttack()
    {
    	return Mathf.FloorToInt(2*(_base.GetAttack() * level) / 100f ) + 5;
    }
    public int GetDefense()
    {
    	return Mathf.FloorToInt(2*(_base.GetDefense() * level) / 100f ) + 5;
    }
    public int GetspAttack()
    {
    	return Mathf.FloorToInt(2*(_base.GetSpAttack() * level) / 100f ) + 5;
    }
    public int GetspDefense()
    {
    	return Mathf.FloorToInt(2*(_base.GetSpDefense() * level) / 100f ) + 5;
    }
    public int GetSpeed()
    {
    	return Mathf.FloorToInt(2*(_base.GetSpeed() * level) / 100f ) + 5;
    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1f;

        if(Random.value * 100f <= 6.25f)
            critical = 2f;

        float type = TypeChart.GetEffectiveness(move.Base.GetType1(), this._base.GetType1()) * TypeChart.GetEffectiveness(move.Base.GetType1(), this._base.GetType2()) * TypeChart.GetEffectiveness(move.Base.GetType1(), this._base.GetType3());

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            ko = false
        };

        float modifier = Random.Range(0.05f, 1f) * type * critical;
        float a = (float) ((0.4 * attacker.level) + 2);
        Debug.Log(a);
        float d = (float) (a * move.Base.GetPower() * (float)attacker.GetAttack() / (GetDefense() *50)) + 2;
        Debug.Log(d);
        int damage = Mathf.FloorToInt(d * modifier);
        Debug.Log(damage);
        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            damageDetails.ko = true;

        }
        return damageDetails;
    }
    public Move GetEnnemyMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

}

public class DamageDetails
{
    public bool ko {get; set;}

    public float Critical {get; set;}

    public float TypeEffectiveness {get; set;}
}