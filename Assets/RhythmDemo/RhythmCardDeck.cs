﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Pull data from spreadsheet, populate card deck, draw random cards for player to choose from
public class RhythmCardDeck : MonoBehaviour {
    List<string> cardStrings;
    List<GameObject> cards = new List<GameObject> ();
    public GameObject cardPrefab;

    // Use this for initialization
    void Start () {
        StartCoroutine(GetEventsFromSpreadsheet());

        // create 4 placeholder cards
        for (int i = 0; i < 4; i++)
        {
            GameObject card = Instantiate(cardPrefab);
            card.gameObject.transform.SetParent(gameObject.transform.parent);

            card.gameObject.transform.localPosition = gameObject.transform.localPosition + (new Vector3(0, -140 * i, 0));
            card.gameObject.transform.localScale = new Vector3(2, 2, 1);
            card.gameObject.SetActive(true);
            cards.Add(card);
        }
    }

    // download data
    IEnumerator GetEventsFromSpreadsheet()
    {
        UnityWebRequest wr = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/1IPKzyXYkCehmvdp1zJx6S9GUlyYXkVZNKGaPzu5k_3o/export?format=csv");
        yield return wr.SendWebRequest();
        while (!wr.downloadHandler.isDone)
        {
            yield return null;
        }

        Debug.Log("Download done");
        string fileBody = wr.downloadHandler.text;
        Debug.Log(fileBody);

        // Split by line, each line is a card
        string[] cardDataSplit = fileBody.Split('\n');

        cardStrings = new List<string>(cardDataSplit);

        DrawCards();
    }

    // Draw random cards
    public void DrawCards()
    {
        System.Random rd = new System.Random();

        List<int> randomedCards = new List<int>();

        if (cardStrings != null)
        {
            while (randomedCards.Count < 4)
            {
                int randomCard = rd.Next(cardStrings.Count);
                if (!randomedCards.Contains(randomCard))
                {
                    randomedCards.Add(randomCard);
                }
            }

            // Put the contents in the placeholder cards
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].GetComponent<RhythmCard>().setContent(cardStrings[randomedCards[i]]);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
