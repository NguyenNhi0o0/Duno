using System;
using System.Collections.Generic;

public class Player
{
    private static int playerIdCounter = 0;

    public string Name;
    public List<Card> Hand;
    public int Id { get; private set; }
    public bool SayUNO = false;

    public Player(string name) {
        Name = name;
        Hand = new List<Card>();
        Id = ++playerIdCounter;
    }

    public void DrawCard(Card card) {
        Hand.Add(card);
    }

    public bool PlayCard(Card card) {
        if (Hand.Contains(card)) {
            Hand.Remove(card);
            return true;
        }
        return false;
    }

    public Card GetCardById(int cardId) {
        return Hand.Find(card => card.Id == cardId);
    }

    public List<Card> GetHand() {
        return Hand;
    }

    public CardInfo[] GetHandAsInfo() {
        CardInfo[] handInfo = new CardInfo[Hand.Count];
        for (int i = 0; i < Hand.Count; i++) {
            handInfo[i] = new CardInfo {
                CardC = Hand[i].Color,
                CardV = Hand[i].Value,
                CardId = Hand[i].Id
            };
        }
        return handInfo;
    }

    public override string ToString() {
        return Name;
    }
}