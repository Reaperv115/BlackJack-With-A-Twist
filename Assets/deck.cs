using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class deck : card
{
    public List<GameObject> cards = new List<GameObject>(52);

    List<GameObject> dealerCards = new List<GameObject>(2);
    List<GameObject> playerCards = new List<GameObject>(2);

    [SerializeField]
    List<Text> cardInfo = new List<Text>(4);

    [SerializeField]
    Transform dealerDeck;
    [SerializeField]
    Transform playerDeck;
    
    Text hit3;
    Text choice;
    Text result;
    Text dealerPadding;
    Text dTotal, pTotal;

   
    Button button, yes, no, one, eleven;

    //used for itering through
    //lists in start and the DealtoPlayer 
    //and DealtoDealer functions
    int j = 0;
    int dK = 0, pK = 0, cI = 0, l = 0;
    int dealerTotal = 0;
    int playerTotal = 0;

    int timesDealt = 0;
    float informTime = 0;

    bool dealOut = false;
    bool takeaHit = false;
    bool tallyUp = false;
    bool inform = false;
    bool hasanAce = false;
    bool displayResults = false;
    bool isEleven = false, isOne = false;

    // Use this for initialization
    void Start () 
    {
        //Initializing all buttons
        button = GameObject.Find("hit").GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        yes = GameObject.Find("yes").GetComponent<Button>();
        yes.onClick.AddListener(OnYes);
        no = GameObject.Find("no").GetComponent<Button>();
        no.onClick.AddListener(OnNo);
        one = GameObject.Find("1").GetComponent<Button>();
        one.onClick.AddListener(On1);
        eleven = GameObject.Find("11").GetComponent<Button>();
        eleven.onClick.AddListener(On11);
        //

        //initializing text objects
        hit3 = GameObject.Find("3rd hit").GetComponent<Text>();
        choice = GameObject.Find("1 or 11").GetComponent<Text>();
        result = GameObject.Find("Result").GetComponent<Text>();
        dealerPadding = GameObject.Find("Dealer padding").GetComponent<Text>();
        dTotal = GameObject.Find("dealer total").GetComponent<Text>();
        pTotal = GameObject.Find("player total").GetComponent<Text>();

        int k, l;
        GameObject temp;

        for (int i = 0; i < cards.Capacity; ++i)
        {
            cards[i].name = name[j];
            ++j;
        }

        //shuffling the cards
        for (int x = 0; x < 20; ++x)
        {
            k = UnityEngine.Random.Range(0, cards.Capacity);
            l = UnityEngine.Random.Range(0, cards.Capacity);
            temp = cards[k];
            cards[k] = cards[l];
            cards[l] = temp;
        }

        for (int i = 0; i < 2; ++i)
        {
            playerCards.Add(null);
            dealerCards.Add(null);
        }

        //disabling buttons until they are needed
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        one.gameObject.SetActive(false);
        eleven.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (isOne)
        {
            playerTotal += 1;
            one.gameObject.SetActive(false);
            eleven.gameObject.SetActive(false);
            displayResults = true;
            isOne = false;
            hasanAce = false;
            choice.text = "";
        }
        if (isEleven)
        {
            playerTotal += 11;
            one.gameObject.SetActive(false);
            eleven.gameObject.SetActive(false);
            displayResults = true;
            isEleven = false;
            hasanAce = false;
            choice.text = "";
        }

        //if-check to inform you when the dealer is 
        //retrieving cards to get his deck to at least 16
        if (inform)
        {
            if (informTime > 5)
                dealerPadding.text = "";
            
            else
            {
                dealerPadding.text = "dealer is receiving cards until they're at least at 16";
                informTime += 0.03f;
            }
            
        }

        if (timesDealt == 2 && dealerTotal <= 0)
        {
            button.gameObject.SetActive(false);
            CalculateDealerTotal();

            if (dealerTotal > 16)
            {
                yes.gameObject.SetActive(true);
                no.gameObject.SetActive(true);
                hit3.text = "Want 1 more hit?";
                timesDealt = 0;
            }
        }

        //dealers cards don't add up to 16 so give him cards until 
        //he's at 16 or more
        else if (dealerTotal <= 16 && timesDealt == 2)
            GivetoBeGreater();
        
        //dealer has at least 16 so now you can hit again if you choose
        if (dealerTotal > 16 && timesDealt == 2)
        {
            yes.gameObject.SetActive(true);
            no.gameObject.SetActive(true);
            hit3.text = "Want 1 more hit?";
            timesDealt = 0;
        }

        //player decided to take another hit
        if (takeaHit)
        {
            playerCards.Add(null);
            Dealer3rdHit();
            DealtoPlayer();
            takeaHit = false;
        }

        //calculating player total
        if (tallyUp)
        {
            button.gameObject.SetActive(false);
            CalculatePlayerTotal();
            tallyUp = false;
        }

        //asking the player if they 
        //want their ace to count
        //as 1 or 11
        if (hasanAce)
        {
            choice.text = "Do you want the Ace to be counted as a 1 or 11?";
            one.gameObject.SetActive(true);
            eleven.gameObject.SetActive(true);
        }

        //displaying the results of the game
        if (displayResults && hasanAce == false && takeaHit == false)
        {
            dTotal.text = "Dealer total: " + dealerTotal;
            pTotal.text = "Player total: " + playerTotal;

            if (playerTotal > 21)
            {
                if (dealerTotal > 21)
                    result.text = "You both bust, it's a tie";
                else
                    result.text = "Dealer wins!";
            }
            if (playerTotal == 21)
            {
                if (dealerTotal == 21)
                    result.text = "You both have 21, it's a draw";
                else
                    result.text = "You win!";
            }
            if (playerTotal < 21)
            {
                if (dealerTotal == playerTotal)
                    result.text = "You both have the same amount, it's a tie";
                else
                {
                    if (dealerTotal > playerTotal)
                    {
                        if (dealerTotal <= 21)
                            result.text = "Dealer wins!";
                        else
                            result.text = "You win!";
                    }
                    else
                        result.text = "You win!";
                }
            }
            
        }
        
        //giving a card to both player and dealer
        if (dealOut)
        {
            DealtoDealer();
            DealtoPlayer();
            dealOut = false;
            ++timesDealt;
        }
    }


    void DealtoDealer()
    {
        dealerCards[dK] = cards[l];
        StartCoroutine(moveCard(dealerCards[dK], dealerDeck));
        if (dealerCards[dK] != null)
        {
            if (cI == cardInfo.Capacity) { }
            else
            {
                cardInfo[cI].text = dealerCards[dK].name;
                ++cI;
                cards.Remove(cards[l]);
                ++dK;
            }
        }
        
    }

    //function that's called if the cards
    //the dealer currently has don't add up 
    //to at least 16
    void GivetoBeGreater()
    {
        inform = true;
        
        dealerCards.Add(null);
        dealerCards[dK] = cards[l];
        cards.Remove(cards[l]);

        int dealerChoice;
        if (dealerCards[dK].name.Contains("A"))
        {
            dealerChoice = UnityEngine.Random.Range(1, 3);
            if (dealerChoice == 1)
                dealerTotal += 1;
            else
                dealerTotal += 11;
        }
        else if (dealerCards[dK].name.Contains("Q") || dealerCards[dK].name.Contains("J") || dealerCards[dK].name.Contains("K") || dealerCards[dK].name.Contains("10"))
            dealerTotal += 10;
        else
        {
            switch (dealerCards[dK].name[0])
            {
                case '2':
                    dealerTotal += 2;
                    break;
                case '3':
                    dealerTotal += 3;
                    break;
                case '4':
                    dealerTotal += 4;
                    break;
                case '5':
                    dealerTotal += 5;
                    break;
                case '6':
                    dealerTotal += 6;
                    break;
                case '7':
                    dealerTotal += 7;
                    break;
                case '8':
                    dealerTotal += 8;
                    break;
                case '9':
                    dealerTotal += 9;
                    break;
                default:
                    break;
            }
        }
    }

    void DealtoPlayer()
    {
        playerCards[pK] = cards[l];
        StartCoroutine(moveCard(playerCards[pK], playerDeck));
        if (playerCards[pK] != null)
        {
            if (cI == cardInfo.Capacity) { }
            else
            {
                cardInfo[cI].text = playerCards[pK].name;
                ++cI;
                cards.Remove(cards[l]);
                ++pK;
            }
            
        }
        
    }

    public void OnClick() { dealOut = true; }

    //button mechanisms for when the player decided if they want to 
    //be given a 4rd card
    public void OnYes() 
    {
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        hit3.text = "";

        takeaHit = true;
        tallyUp = true;
        displayResults = true;
    }
    public void OnNo() 
    {
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        hit3.text = "";

        tallyUp = true;
        displayResults = true;
    }
    //

    // the twist: when the player decides if they want to 
    //take a 3rd hit, the dealer may also take another hit if they choose
    //but only if the player decides to take a 3rd hit
    void Dealer3rdHit() 
    {
        int hit3 = UnityEngine.Random.Range(0, 10);
        int dealerChoice;

         if (hit3 % 2 == 0)
         {
             dealerCards.Add(null);
            dealerCards[dK] = cards[l];
            cardInfo[cI].text = dealerCards[dK].name;
            StartCoroutine(moveCard(dealerCards[dK], dealerDeck));
            cards.Remove(cards[l]);

            if (dealerCards[dK].name.Contains("A"))
            {
                dealerChoice = UnityEngine.Random.Range(1, 3);
                if (dealerChoice == 1)
                    dealerTotal += 1;
                else
                    dealerTotal += 11;
            }
            else if (dealerCards[dK].name.Contains("Q") || dealerCards[dK].name.Contains("J") || dealerCards[dK].name.Contains("K") || dealerCards[dK].name.Contains("10"))
                dealerTotal += 10;
            else
            {
                switch (dealerCards[dK].name[0])
                {
                    case '2':
                        dealerTotal += 2;
                        break;
                    case '3':
                        dealerTotal += 3;
                        break;
                    case '4':
                        dealerTotal += 4;
                        break;
                    case '5':
                        dealerTotal += 5;
                        break;
                    case '6':
                        dealerTotal += 6;
                        break;
                    case '7':
                        dealerTotal += 7;
                        break;
                    case '8':
                        dealerTotal += 8;
                        break;
                    case '9':
                        dealerTotal += 9;
                        break;
                    default:
                        break;
                }
            }
            ++cI;
            return;
         }
         else
             ++cI;
    }

    public void On1() { isOne = true; }

    public void On11() { isEleven = true; }

    void CalculatePlayerTotal()
    {
        for (int i = 0; i < playerCards.Count; ++i)
        {
            if (playerCards[i].name.Contains("A"))
                hasanAce = true;
            else if (playerCards[i].name.Contains("Q") || playerCards[i].name.Contains("K") || playerCards[i].name.Contains("J") || playerCards[i].name.Contains("10"))
                playerTotal += 10;
            else
            {
                switch (playerCards[i].name[0])
                {
                    case '2':
                        playerTotal += 2;
                        break;
                    case '3':
                        playerTotal += 3;
                        break;
                    case '4':
                        playerTotal += 4;
                        break;
                    case '5':
                        playerTotal += 5;
                        break;
                    case '6':
                        playerTotal += 6;
                        break;
                    case '7':
                        playerTotal += 7;
                        break;
                    case '8':
                        playerTotal += 8;
                        break;
                    case '9':
                        playerTotal += 9;
                        break;
                    default:
                        break;
                }

            }
        }
       
    }

    void CalculateDealerTotal()
    {
        int dealerChoice;
        for (int i = 0; i < dealerCards.Count; ++i)
        {
            if (dealerCards[i].name.Contains("A"))
            {
                dealerChoice = UnityEngine.Random.Range(1, 3);
                if (dealerChoice == 1)
                    dealerTotal += 1;
                else
                    dealerTotal += 11;
            }
            else if (dealerCards[i].name.Contains("Q") || dealerCards[i].name.Contains("J") || dealerCards[i].name.Contains("K") || dealerCards[i].name.Contains("10"))
                dealerTotal += 10;
            else
            {
                switch (dealerCards[i].name[0])
                {
                    case '2':
                        dealerTotal += 2;
                        break;
                    case '3':
                        dealerTotal += 3;
                        break;
                    case '4':
                        dealerTotal += 4;
                        break;
                    case '5':
                        dealerTotal += 5;
                        break;
                    case '6':
                        dealerTotal += 6;
                        break;
                    case '7':
                        dealerTotal += 7;
                        break;
                    case '8':
                        dealerTotal += 8;
                        break;
                    case '9':
                        dealerTotal += 9;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    IEnumerator moveCard(GameObject c, Transform dest)
    {
        while (c.transform.position != dest.position)
        {
            c.transform.position = Vector3.MoveTowards(c.transform.position, dest.position, .03f);
            yield return null;
        }
    }
}
