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

    public string CardName() {
        switch (type) {
        case CardEffect.Add:
            return "Add";
        case CardEffect.Subtract:
            return "Subtract";
        case CardEffect.Attack:
            return "Attack";
        case CardEffect.Stop:
            return "Stop";
        case CardEffect.Skip:
            return "Skip";
        case CardEffect.Approach:
            return "Approach";
        default:
            return "";
        }
    }

    public string CardExplanation () {
        switch (type) {
        case CardEffect.Add:
            return string.Format("Add {0} {1} to the clock", num.ToString(), scale);
        case CardEffect.Subtract:
            return string.Format("Minus {0} {1} from the clock", num.ToString(), scale);
        case CardEffect.Attack:
            return "Speed up the clock and +5 to your countdown timer";
        case CardEffect.Stop:
            return "Stop the clock until your next turn";
        case CardEffect.Skip:
            return "Skip the next player";
        case CardEffect.Approach:
            return "Set the clock to 10 minutes before your goal time";
        default:
            return "";
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
