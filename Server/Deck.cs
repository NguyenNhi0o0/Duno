using System;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private readonly List<Card> drawPile = new List<Card>();
    private readonly List<Card> discardPile = new List<Card>();

    public Deck() {
        Init();
        Shuffle();
    }

    private void Init() {
        foreach (CardColor color in Enum.GetValues(typeof(CardColor))) {
            if (color != CardColor.Black) {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue))) {
                    if (value != CardValue.Wild && value != CardValue.WildDrawFour && value != CardValue.Zero) {
                        drawPile.Add(new Card(color, value));
                        drawPile.Add(new Card(color, value)); 
                    }
                    else if(value == CardValue.Zero) {
                        drawPile.Add(new Card(color, value));
                    }
                }
            }
            else {
                for (int i = 0; i < 4; i++) {
                    drawPile.Add(new Card(CardColor.Black, CardValue.Wild));
                    drawPile.Add(new Card(CardColor.Black, CardValue.WildDrawFour));
                }
            }
        }
    } 

    private void Shuffle() {
        System.Random rnd = new System.Random();
        for (int i = drawPile.Count - 1; i > 0; i--) {
            int j = rnd.Next(i + 1);
            Card temp = drawPile[i];
            drawPile[i] = drawPile[j];
            drawPile[j] = temp;
        }
    }

    public Card DrawCard() {
        if (drawPile.Count == 0 && discardPile.Count == 0) {
            // Excpetion ou return null ? jsp encore
        }
        else if (drawPile.Count == 0) {
            drawPile.AddRange(discardPile);
            discardPile.Clear();
            Shuffle();
        }
        Card card = drawPile[0];
        drawPile.RemoveAt(0);
        return card;
    }

    public void DiscardCard(Card card) {
        discardPile.Add(card);
    }
}