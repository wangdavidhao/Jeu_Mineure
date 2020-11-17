using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
	//Cette variable ajuste la vitesse du personnage, ici initialisée à 5
	public float speed = 5;
	//Cette variable prend le composant Rigidbody2D du personnage
    Rigidbody2D rb;
    //Cette variable prend la direction indiquée par les flèches directionnelles de l'utilisateur
    Vector2 dir;
    //Cette variable prend en compte l'animation du sprite du personnage
    Animator anim;
    //Cette variable va permettre au personnage de s'immobiliser dans la direction vers laquelle il s'est arrêté
    int immobile = 0;
    //Cette variable va récupérer la layer sur lequel se trouve l'herbe
    public LayerMask grass;
    BoxCollider2D collider;
    Vector2 posInit;
    Vector2 nul;

    // Start is called before the first frame update
    void Start()
    {
        //On récupère le Rigidbody et les animations du sprite concerné par le script
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        posInit = collider.size;
    }

    // Update is called once per frame
    void Update()
    {
        //Les deux paramètres du vecteur dir vont stocker les directions indiquées par l'utilisateur
        if(Input.GetKey(KeyCode.RightArrow))
        {
        	dir.y = 0;
        	dir.x = 1;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
        	dir.y = 0;
        	dir.x = -1;
        }
        else if(Input.GetKey(KeyCode.UpArrow))
        {
        	dir.x = 0;
        	dir.y = 1;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
        	dir.x = 0;
        	dir.y = -1;
        }
        else
        {
        	dir.x = 0;
        	dir.y = 0;
        }

        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
        //On appelle la fonction setParam qui va permettre de savoir quelle animation jouer en fonction de la direction rentrée
        setParam();
        //On appelle également la fonction qui va faire apparaître les pokémons sauvages
        randomFight();
        
        
    }

    void randomFight()
    {
    	//SI l'on rentre dans l'herbe
    	if(Physics2D.OverlapCircle(rb.position + dir,0.1f,grass) != null)
    	{
    		nul.Set(0,0);
    		collider.size = nul;
            //On fait apparaître au hasard des pokémons, en moyenne 10% du temps
            if(Random.Range(1,101) <= 10)
            {
                //Ici, mettre la fonction amenant un combat pokémon
                Debug.Log("Fight");
            }
    		if(Physics2D.OverlapCircle(rb.position + dir,0.1f,LayerMask.GetMask("Solid")) != null)
    		{
    			collider.size = posInit;
    		}
    		
    	}
    	else if(Physics2D.OverlapCircle(rb.position,0.1f,grass) == null)
    	{
    		
    		if(Physics2D.OverlapCircle(rb.position - dir,0.1f,grass) != null)
    		{
    			collider.size = posInit;
    		} 	
    	}
    }

    void setParam()
    {
    	//Immobile
    	if(dir.x == 0 && dir.y == 0)
    	{
    		//Cas où le personnage allait vers le bas
    		if(immobile == 0)
    		{
    			anim.SetInteger("Dir",0);
    		}
    		//Cas où le personnage allait vers la droite
    		else if(immobile == 1)
    		{
    			anim.SetInteger("Dir",5);
    		}
    		//Cas où le personnage allait vers la gauche
    		else if(immobile == 2)
    		{
    			anim.SetInteger("Dir",6);
    		}
    		//Cas où le personnage allait vers le haut
    		else if(immobile == 3)
    		{
    			anim.SetInteger("Dir",7);
    		}
    	}

    	//Droite
    	else if(dir.x > 0)
    	{
    		if(Input.GetKey(KeyCode.Space))
    		{
    			anim.SetInteger("Dir",8);
    			speed = 10;
    		}
    		else
    		{
    			anim.SetInteger("Dir",1);
    			speed = 5;
    		}
    		immobile = 1;
    	}
    	//Gauche
    	else if(dir.x < 0)
    	{
    		if(Input.GetKey(KeyCode.Space))
    		{
    			anim.SetInteger("Dir",9);
    			speed = 10;
    		}
    		else
    		{
    			anim.SetInteger("Dir",2);
    			speed = 5;
    		}
    		immobile = 2;
    	}
    	//Bas
    	else if(dir.y < 0)
    	{
    		if(Input.GetKey(KeyCode.Space))
    		{
    			anim.SetInteger("Dir",10);
    			speed = 10;
    		}
    		else
    		{
    			anim.SetInteger("Dir",3);
    			speed = 5;
    		}
    		immobile = 0;
    	}
    	//Haut
    	else if(dir.y > 0)
    	{
    		if(Input.GetKey(KeyCode.Space))
    		{
    			anim.SetInteger("Dir",11);
    			speed = 10;
    		}
    		else
    		{
    			anim.SetInteger("Dir",4);
    			speed = 5;
    		}
    		immobile = 3;
    	}

    	//Cas où l'on court
    	
    }
}
