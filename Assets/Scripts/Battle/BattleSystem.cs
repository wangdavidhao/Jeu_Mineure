using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Listes des états pendant le combat
public enum BattleState
{
    Start,
    PlayerAction,
    PlayerMove,
    EnnemyMove,
    Busy
}

public class BattleSystem : MonoBehaviour
{
    //Variables contenant le pokémon du joueurs et ses informations
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] Infos playerInfo;
    //Variables contenant le pokémon de l'adversaire et ses informations
    [SerializeField] BattleUnit ennemyUnit;
    [SerializeField] Infos ennemyInfo;
    //Variable permettant d'afficher les textes pour effectuer le combat
    [SerializeField] BattleDialogBox dialogBox;

    //Variable contenant l'état actuel du combat
    BattleState state;
    //Variables permettant de sélectionner une action et une attaque 
    int currentAction;
    int currentMove;

    private void Start()
    {
    	StartCoroutine(SetupBattle());
    }

    //Méthode permattant d'initialiser les pokémons, leurs information et le premier dialogue du combat
    public IEnumerator SetupBattle()
    {
        state = BattleState.Busy;

    	playerUnit.Setup();
    	ennemyUnit.Setup();
    	playerInfo.SetData(playerUnit.pokemon);
    	ennemyInfo.SetData(ennemyUnit.pokemon);

        dialogBox.SetMoveNames(playerUnit.pokemon.Moves);

        yield return dialogBox.TypeDialog($"Un {ennemyUnit.pokemon._base.GetName()} sauvage apparaît !");
        //On appelle la méthode qui initialise les textes permettant de choisir une action
        PlayerAction();
    }

    //Méthode affichant les textes permettant de choisir une action
    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        //On affiche l'objet contenant les textes qui permettent d'effectuer l'action
        dialogBox.EnableActionSelector(true);
    }

    //Méthode affichant les textes permettant de choisir une attaque
    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        //On efface tous les objets et on affiche celui contenant les textes permettant de choisir une attaque
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove()
    {
        var move = playerUnit.pokemon.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.pokemon._base.GetName()} utilise {move.Base.GetName()}");
        yield return new WaitForSeconds(1f);

        var damageDetails = ennemyUnit.pokemon.TakeDamage(move, playerUnit.pokemon);
        yield return ennemyInfo.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if(damageDetails.ko)
        {
            yield return dialogBox.TypeDialog($"{ennemyUnit.pokemon._base.GetName()} est K.O.");
        }
        else
        {
            StartCoroutine(EnnemyMove());
        }
    }

    IEnumerator EnnemyMove()
    {
        state = BattleState.EnnemyMove;

        var move = ennemyUnit.pokemon.GetEnnemyMove();

        yield return dialogBox.TypeDialog($"{ennemyUnit.pokemon._base.GetName()} utilise {move.Base.GetName()}");

        var damageDetails = playerUnit.pokemon.TakeDamage(move, ennemyUnit.pokemon);
        yield return playerInfo.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if(damageDetails.ko)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.pokemon._base.GetName()} est K.O.");
        }
        else
        {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.Critical > 1f)
            yield return dialogBox.TypeDialog("Coup critique !");

        if(damageDetails.TypeEffectiveness > 1)
            yield return dialogBox.TypeDialog("C'est super efficace !");
        else if(damageDetails.TypeEffectiveness < 1)
            yield return dialogBox.TypeDialog("Ce n'est pas très efficace...");
    }

    //Méthode permettant de passer de la phase de sélection d'action à celle de sélection d'attaque
    private void Update()
    {
        if(state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if(state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    //Méthode permettant de sélectionner une action
    void HandleActionSelection()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentAction < 1)
                ++currentAction;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentAction > 0)
            {
                --currentAction;
            }
        }
        //On appelle la fonction qui va afficher le curseur de l'action sélectionner
        dialogBox.UpdateActionSelection(currentAction);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(currentAction == 0)
            {
                //Fight
                PlayerMove();
            }
            else if(currentAction == 1)
            {
                //Run
            }
        }
    }

    //Méthode permettant de sélectionner une attaque
    void HandleMoveSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(currentMove < playerUnit.pokemon.Moves.Count - 1)
            {
                ++currentMove;
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(currentMove > 0)
            {
                --currentMove;
            }
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentMove < playerUnit.pokemon.Moves.Count - 2)
            {
                currentMove += 2;
            }
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentMove > 1)
            {
                currentMove -= 2;
            }
        }
        //On appelle la fonction qui va afficher le curseur de l'attaque à sélectionner
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.pokemon.Moves[currentMove]);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }
}
