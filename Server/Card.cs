using System;
using System.Collections.Generic;
using UnityEngine;

public enum CardColor { Red, Green, Blue, Yellow, Black }
public enum CardValue { Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Skip, Reverse, DrawTwo, Wild, WildDrawFour }

public class Card
{
    private static int cardIdCounter = 0;

    public int Id { get; private set; }
    public CardColor Color;
    public CardValue Value;

    public Card(CardColor color, CardValue value) {
        Id = ++cardIdCounter;
        Color = color;
        Value = value;
    }

    public override string ToString() {
        return $"{Color} {Value}";
    }
}
