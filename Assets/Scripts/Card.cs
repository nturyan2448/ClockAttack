using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

public class Card : MonoBehaviour
{
    public CardEffect type;
    public int num;
    public string scale; // Minute or Hour
    Deck deck;

    // Start is called before the first frame update
    void Start()
    {
        deck = GameController.instance.GetComponent<Deck>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum CardEffect
    {
        Add,
        Subtract,
        SpeedUp,
        Stop,
        Skip,
        Attack,
        Approach,
        Size
    }


    public void TriggerCardEffect (int callerID = -1, int argument = 0) {
        switch (type) {
        case CardEffect.Add:
            GameController.instance.AddTime(num, scale);
            break;
        case CardEffect.Subtract:
            GameController.instance.SubtractTime(num, scale);
            break;
        case CardEffect.SpeedUp:
        case CardEffect.Attack:
            GameController.instance.SpeedUp();
            break;
        case CardEffect.Stop:
            GameController.instance.StopClock(callerID); // playerID
            break;
        case CardEffect.Skip:
            GameController.instance.SkipRound();
            break;
        case CardEffect.Approach:
            argument = (argument + 710) % 720; // set to 10 minutes before
            GameController.instance.SetTime(argument / 60, argument % 60);
            break;
        default:
            break;
        }
    }

    

    //private void OnMouseDown () {
    //    Player p = transform.parent.GetComponent<Player>();
    //    if (p != null && p.playerID == 0 && GameController.instance.round == 0) {
    //        p.PlayCard(this.gameObject);
    //    }
    //}

    //private void OnMouseOver () {
    //    Debug.Log("MouseOver " + name);
    //    transform.localPosition = new Vector2(transform.localPosition.x, 1);
    //}

    //private void OnMouseExit () {
    //    transform.localPosition = new Vector2(transform.localPosition.x, 0);
    //}
}
