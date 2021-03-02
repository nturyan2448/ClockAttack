using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Deck : MonoBehaviour
{
    //readonly Card[] TypeOfCard =  {
    //    new Card(CardEffect.Add, 5, "min"),
    //    new Card(CardEffect.Add, 10, "min"),
    //    new Card(CardEffect.Add, 30, "min"),
    //    new Card(CardEffect.Add, 1, "hour"),
    //    new Card(CardEffect.Add, 3, "hour"),
    //    new Card(CardEffect.Add, 6, "hour"),
    //    new Card(CardEffect.Subtract, 10, "min"),
    //    new Card(CardEffect.Subtract, 30, "min"),
    //    new Card(CardEffect.Subtract, 1, "hour"),
    //    new Card(CardEffect.Subtract, 3, "hour"),
    //};

    //                         Add,Sub
    //readonly int[] numCards = { 4, 4 };
    public GameObject deckObject;
    public GameObject players;
    public List<Card> TypeOfCard;
    List<GameObject> deck = new List<GameObject>();
    List<GameObject> discardDeck = new List<GameObject>();

    private void Awake () {
        foreach (Card c in TypeOfCard) {
            for (int i = 0; i < 3; i++) {
                GameObject g = Instantiate(c.gameObject);
                g.transform.SetParent(deckObject.transform);
                deck.Add(g);
                g.SetActive(false);
            }
        }
        Shuffle(deck);
        foreach (GameObject g in deck) {
            Debug.Log(g.name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Shuffle(List<GameObject> d) {
        for (int i = 0; i < d.Count; i++) {
            int index = Random.Range(i, d.Count);
            if (index != i) {
                GameObject c = d[i];
                d[i] = d[index];
                d[index] = c;
            }
        }
    }

    public void Deal() {
        for (int j = 0; j < players.transform.childCount; j++) {
            List<GameObject> ret = new List<GameObject>();
            GameObject playerArea = players.transform.GetChild(j).gameObject;
            for (int i = -2; i < 3; i++) {
                GameObject g = deck[0];
                g.transform.SetParent(playerArea.transform);
                g.transform.localPosition = new Vector2(i * 0.04f, 0);
                g.SetActive(true);
                g.GetComponent<SortingGroup>().sortingOrder = i + 3;
                deck.RemoveAt(0);
                ret.Add(g);
            }
            playerArea.GetComponent<Player>().SetHand(ret);
        }
       
    }

    public GameObject Draw() {
        if (deck.Count == 0) {
            Debug.LogError("No more card in Deck. Shuffle");
            Shuffle(discardDeck);
            deck = discardDeck;
            discardDeck = new List<GameObject>();
        }
        GameObject ret = deck[0];
        deck.RemoveAt(0);
        return ret;
    }

    public void Discard(GameObject g) {
        discardDeck.Add(g);
        //g.SetActive(false);
        g.transform.SetParent(deckObject.transform);
    }

}

