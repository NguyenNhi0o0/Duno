using System;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private readonly List<Player> players;
    private int gameMode;
    private int currentPlayerIndex;
    private Card currentCard;
    private Deck deck;
    private CardColor currentColor;
    private Player colorChooser = null; // player that played a wild card
    private int direction = 1; // 1 for clockwise, -1 for counter-clockwise
    private bool isGameOver = false;

    public Game(List<Player> players, int gameMode) {
        this.players = players;
        this.gameMode = gameMode;
        deck = new Deck();
        Start();
    }

    public void Start() {
        DealCards(7);
        SelectFirstCard();
        SelectFirstPlayer();
    }

    private void DealCards(int numberOfCards) {
        foreach (var player in players) {
            for (int i = 0; i < numberOfCards; i++) {
                Card card = deck.DrawCard();
                player.DrawCard(card);
            }
        }
    }

    private void SelectFirstCard() { // pas exactement les bonne regles mais sa fait le taff pour l'instant
        currentCard = deck.DrawCard();
        while (currentCard.Color == CardColor.Black) {
            deck.DiscardCard(currentCard);
            currentCard = deck.DrawCard();
            currentColor = currentCard.Color;
        }
        deck.DiscardCard(currentCard);
    }

    private void SelectFirstPlayer() { // aleatoire pour l'instant
        System.Random rand = new System.Random();
        currentPlayerIndex = rand.Next(players.Count);
    }

    public Player GetCurrentPlayer() {
        return players[currentPlayerIndex];
    }

    public Card GetCurrentCard() {
        return currentCard;
    }

    public CardInfo GetCurrentCardAsInfo() {
        return new CardInfo {
            CardC = currentCard.Color,
            CardV = currentCard.Value,
            CardId = currentCard.Id
        };
    }

    public PlayerInfo GetCurrentPlayerAsInfo() {
        return new PlayerInfo {
            Index = currentPlayerIndex,
            NbCartes = GetCurrentPlayer().Hand.Count
        };
    }

    public PlayerInfo[] GetPlayersAsInfo() {
        PlayerInfo[] playersInfo = new PlayerInfo[players.Count];
        for (int i = 0; i < players.Count; i++) {
            playersInfo[i] = new PlayerInfo {
                Index = i,
                NbCartes = players[i].Hand.Count
            };
        }
        return playersInfo;
    }

    public int PlayCard(Player player, Card card) {
        // Check if it's the player's turn
        if (player != GetCurrentPlayer()) {
            return -1;
        }
        // Check if the player has the card
        if (!player.Hand.Contains(card)) {
            return -2;
        }
        // check if the card can be played
        if (!IsCardPlayable(card)) {
            return -3;
        }
        // Check if the game is over
        if (isGameOver) {
            return -4;
        }
        player.PlayCard(card);
        currentCard = card;
        deck.DiscardCard(card);
        currentColor = card.Color;
        ApplyCardEffect(card);
        CheckWinCondition(player);
        NextPlayer();
        return 1;
    }

    private void ApplyCardEffect(Card card) {
        Player nextPlayer = GetNextPlayer();
        switch (card.Value) {
            case CardValue.Skip:
                NextPlayer();
                break;
            case CardValue.Reverse:
                direction *= -1;
                if (players.Count == 2) {
                    NextPlayer();
                }
                break;
            case CardValue.DrawTwo: // peut pas jouer +2 sur un +2 pour l'instant et passe son tour après avoir piocher
                for (int i = 0; i < 2; i++) {
                    nextPlayer.DrawCard(deck.DrawCard());
                }
                nextPlayer.SayUNO = false;
                NextPlayer();
                break;
            case CardValue.Wild:
                colorChooser = GetCurrentPlayer();
                break;
            case CardValue.WildDrawFour:
                colorChooser = GetCurrentPlayer();
                for (int i = 0; i < 4; i++) {
                    nextPlayer.DrawCard(deck.DrawCard());
                }
                nextPlayer.SayUNO = false;
                NextPlayer();
                break;
        }
    }

    public int ChooseColor(Player player, CardColor color) {
        // Check if it's the player's turn
        if (player != colorChooser) {
            return -1;
        }
        // Check if the color is valid
        if (color == CardColor.Black) {
            return -2;
        }
        currentColor = color;
        colorChooser = null;
        return 1;
    }

    public int DrawCard(Player player) {
        // Check if it's the player's turn
        if (player != GetCurrentPlayer()) {
            return -1;
        }
        // Check if the game is over
        if (isGameOver) {
            return -2;
        }
        // ajouter conditon pioche vide
        player.DrawCard(deck.DrawCard());
        // Pour l'instant on passe le tour après avoir piocher
        player.SayUNO = false;
        NextPlayer();
        return 1;
    }

    public bool CheckWinCondition(Player player) {
        if (player.Hand.Count == 0) {
            isGameOver = true;
            return true;
        }
        return false;
    }

    public bool SayUno(Player player) {
        if (player.Hand.Count == 1) {
            player.SayUNO = true;
            return true;
        }
        return false;
    }

    public bool CallOut(Player target) {
        if (target.Hand.Count == 1 && !target.SayUNO) {
            target.DrawCard(deck.DrawCard());
            target.DrawCard(deck.DrawCard());
            target.SayUNO = false;
            return true;
        }
        return false;
    }

    private void NextPlayer() {
        currentPlayerIndex = (currentPlayerIndex + direction + players.Count) % players.Count;
    }

    private Player GetNextPlayer() {
        return players[(currentPlayerIndex + direction + players.Count) % players.Count];
    }

    public bool IsCardPlayable(Card card) {
        if (currentColor == CardColor.Black) {
            return false;
        }
        return card.Color == currentCard.Color || card.Value == currentCard.Value || card.Color == CardColor.Black;
    }
}