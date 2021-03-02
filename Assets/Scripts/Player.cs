using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class Player : MonoBehaviour
{
    public int playerID;
    public Countdown countdown;
    public AudioClip playCardAudio;
    public Transform clockPosition;
    public TextMeshPro cardName;
    public TextMeshPro cardExplanation;

    Deck deck;

    int winHour;
    int winMinute;

    int prevRound = -1;
    float AIStartTime;
    float AIThinkTime;
    bool waitTillNextRound = false;
    List<GameObject> hand; // Cards in hand

    // Start is called before the first frame update
    void Start ()
    {
        deck = GameController.instance.GetComponent<Deck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hand != null && hand.Count > 0) {
            for (int i = 0; i < hand.Count; i++) {
                if (playerID != 0) {
                    hand[i].transform.GetChild(4).gameObject.SetActive(true);
                    hand[i].transform.localRotation = Quaternion.Euler(0,0,180);
                }
                hand[i].transform.localPosition = new Vector2((i-2) * 0.04f, 0);
                hand[i].GetComponent<SortingGroup>().sortingOrder = i + 1;
                hand[i].SetActive(true);
            }
        }

        // Check win condition no matter which round it is now currently
        if (GameController.instance.state == "playing")
            CheckWin();

        GameObject target = GetHoverCard();

        // Show explanation of the card;
        if (playerID == 0) {
            if (target != null) {
                cardName.text = target.GetComponent<Card>().CardName();
                cardExplanation.text = target.GetComponent<Card>().CardExplanation();
            } else {
                cardName.text = "";
                cardExplanation.text = "";
            }
        }

        if (GameController.instance.round == playerID && GameController.instance.state == "playing") {
            // Is your turn!

            // Check Stop effect
            if (waitTillNextRound) {
                waitTillNextRound = false;
                GameController.instance.RestartClock();
            }

            // Actions for AIs
            if (playerID != 0) {
                if (Time.timeScale == 0) {
                    if (IsStartingRound()) {
                        AIStartTime = Time.unscaledTime;
                        AIThinkTime = Random.Range(1f, 10f);
                    }
                    if (Time.unscaledTime - AIStartTime >= AIThinkTime)
                        PlayCard(hand[0]);
                }
                else {
                    if (IsStartingRound()) {
                        AIStartTime = Time.time;
                        AIThinkTime = Random.Range(1f, 10f);
                    }
                    if (Time.time - AIStartTime >= AIThinkTime)
                        PlayCard(hand[0]);
                }
            }
            // Actions for Human Players
            else if (Input.GetMouseButtonDown(0) && GameController.instance.round == 0) {
                //List<RaycastHit2D> hits = new List<RaycastHit2D>(Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition)));
                //if (hits.Count > 0) {
                //    hits.Sort(delegate (RaycastHit2D x, RaycastHit2D y) {
                //        return x.collider.GetComponent<SortingGroup>().sortingOrder.CompareTo(y.collider.GetComponent<SortingGroup>().sortingOrder);
                //    });
                //    GameObject target = hits[hits.Count - 1].collider.gameObject;
                //    Player owner = target.transform.parent.GetComponent<Player>();
                //    if (owner != null && owner.playerID == 0)
                //        PlayCard(target);
                //}
                if (target != null)
                    PlayCard(target);
            }
        }
        prevRound = GameController.instance.round;
    }

    private void FixedUpdate () {
        
    }

    public void SetHand(List<GameObject> l) {
        hand = l;
    }

    public void SetWinTime(int hour, int minute) {
        winHour = hour;
        winMinute = minute;
        GetComponentInChildren<TextMeshPro>().text = string.Format("{0:00}:{1:00}", hour, minute);
    }

    public void PlayCard(GameObject g) {
        if (g.GetComponent<Card>().type == Card.CardEffect.Approach)
            g.GetComponent<Card>().TriggerCardEffect(playerID, winHour * 60 + winMinute);
        else g.GetComponent<Card>().TriggerCardEffect(playerID);

        StartCoroutine(PlayCardAnimation(g.transform));

        //Debug.Log(g.ToString());
        for (int i = 0; i < hand.Count; i++) {
            if (hand[i] == g) {
                hand.RemoveAt(i);
                break;
            }
        }
        deck.Discard(g);
        //CheckWin();
        GameController.instance.NextRound();

        // Something about card effect that has to be done in Player
        switch(g.GetComponent<Card>().type) {
        case Card.CardEffect.Stop:
            waitTillNextRound = true;
            break;
        case Card.CardEffect.Attack:
            // Extend the countdown
            countdown.IncreaseCountdown(playerID, 5);
            break;
        default:
            break;
        }
    }

    IEnumerator PlayCardAnimation(Transform t) {
        AudioSource.PlayClipAtPoint(playCardAudio, Vector3.zero);
        t.GetChild(4).gameObject.SetActive(false);
        yield return CoroutineUtilities.MoveObjectOverTime(t, t.position, clockPosition.position, 0.2f);
        GameObject newCard = deck.Draw();
        newCard.transform.SetParent(transform);
        hand.Add(newCard);
        if (Time.timeScale == 0)
            yield return new WaitForSecondsRealtime(0.5f);
        else yield return new WaitForSeconds(0.5f);
        t.gameObject.SetActive(false);
        t.rotation = Quaternion.identity;
    }

    void CheckWin() {
        if (GameController.instance.currentHour == winHour && GameController.instance.currentMinute == winMinute) {
            GameController.instance.EndGame(playerID);
        }
    }

    bool IsStartingRound() {
        return GameController.instance.round != prevRound;
    }

    GameObject GetHoverCard() {
        List<RaycastHit2D> hits = new List<RaycastHit2D>(Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition)));
        if (hits.Count > 0) {
            hits.Sort(delegate (RaycastHit2D x, RaycastHit2D y) {
                return x.collider.GetComponent<SortingGroup>().sortingOrder.CompareTo(y.collider.GetComponent<SortingGroup>().sortingOrder);
            });
            GameObject target = hits[hits.Count - 1].collider.gameObject;
            Player owner = target.transform.parent.GetComponent<Player>();
            if (owner != null && owner.playerID == 0)
                return target;
        }
        return null;
    }
}
