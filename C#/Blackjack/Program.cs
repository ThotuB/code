﻿using System;

namespace Blackjack
{
    class Game
    {
        private Deck deck;
        private Deck discard;

        private Dealer dealer;
        private ArrayList<Player> players;

        public Game(ArrayList<Player> players, int nrDecks)
        {
            this.deck = new Deck(nrDecks);
            this.discard = new Deck(0);

            this.dealer = new Dealer();
            this.players = players;
        }

        public Game(ArrayList<Player> players)
        {
            this(players, 1);
        }

        // GETTERS
        public Deck getDeck()
        {
            return this.deck;
        }

        public Dealer getDealer()
        {
            return this.dealer;
        }

        public Player getPlayer(int index)
        {
            return this.players.get(index);
        }

        public ArrayList<Player> getPlayers()
        {
            return this.players;
        }

        // DECK
        public void shuffleDeck()
        {
            this.deck.shuffle();
        }

        // AUTODEAL
        public void deal()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (Player player in players)
                {
                    player.deal(deck.draw(), 0);
                }
                this.dealer.deal(deck.draw(), (i == 0));
            }
            foreach (Player player in players)
            {
                player.checkSplit();
            }
        }

        // PLAYER MOVES
        public bool betPlayer(Player player, int index, int amount)
        {
            return player.makebet(index, amount);
        }

        public void splitPlayer(Player player)
        {
            player.split();

            int handNr = player.getHands().size() - 1;
            betPlayer(player, handNr, player.getHand(0).getBet());

            player.deal(deck.draw(), 0);
            player.deal(deck.draw(), handNr);
            player.checkSplit();
        }

        public bool doubledownPlayer(Player player, int index)
        {
            Hand hand = player.getHand(index);
            if (this.betPlayer(player, index, hand.getBet()))
            {
                hitPlayer(player, index);
                if (hand.getOutcome() == Hand.NONE)
                {
                    hand.stand();
                }
                return true;
            }
            return false;
        }

        // PLAYER/DEALER MOVES
        public void hitPlayer(Player player, int index)
        {
            player.deal(this.deck.draw(), index);
        }

        public void standPlayer(Player player, int index)
        {
            player.stand(index);
        }

        public void hitDealer()
        {
            this.dealer.deal(this.deck.draw());
        }

        public void standDealer()
        {
            this.dealer.stand();
        }


        // DISCARD
        public void discard_all()
        {
            foreach (Player player in players)
            {
                this.discardPlayer(player);
            }
            this.discardDealer();
        }

        public void discardPlayer(Player player)
        {
            foreach (Hand hand in player.getHands())
            {
                foreach (Card card in hand.getCards())
                {
                    this.discard.add(card);
                }
            }
            player.discard();
        }

        public void discardDealer()
        {
            foreach (Card card in this.dealer.getHand().getCards())
            {
                this.discard.add(card);
            }
            this.dealer.discard();
        }

        // REFILL
        public void refill()
        {
            foreach (Card card in this.discard.getCards())
            {
                this.deck.add(card);
            }
            this.discard.clear();
        }
    }

    /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    class Dealer
    {
        private Hand hand;

        public Dealer()
        {
            this.hand = new Hand();
        }

        // GETTERS
        public Hand getHand()
        {
            return this.hand;
        }

        // MOVES
        public void deal(Card card, bool hidden)
        {
            this.hand.add(card, hidden);
        }

        public void deal(Card card)
        {
            this.deal(card, false);
        }

        public void stand()
        {
            this.hand.stand();
        }

        // HIDE/SHOW CARD
        public void hideCard(int index)
        {
            this.hand.hideCard(index);
        }

        public void showCard(int index)
        {
            this.hand.showCard(index);
        }

        // DISCARD
        public void discard()
        {
            this.hand.discard();
        }
    }

    class Player
    {
        private String id;
        private String name;
        private int money;
        private int totalBet;

        private ArrayList<Hand> hands;

        private bool splitFlag;

        Player(String id, String name, int money)
        {
            this.id = id;
            this.name = name;
            this.money = money;

            this.hands = new ArrayList<Hand>(4);
            this.hands.add(new Hand());

            this.splitFlag = false;
        }

        // GETTERS
        public String getName()
        {
            return this.name;
        }

        public Hand getHand(int index)
        {
            return this.hands.get(index);
        }

        public ArrayList<Hand> getHands()
        {
            return this.hands;
        }

        public bool canSplit()
        {
            return this.splitFlag;
        }

        // BETS
        public bool makebet(int index, int amount)
        {
            if (0 <= amount && amount <= this.money && amount % 2 == 0)
            {
                this.getHands().get(index).bet(amount);
                this.money -= amount;
                this.totalBet += amount;
                return true;
            }
            return false;
        }

        // SPLIT
        public void checkSplit()
        {
            Hand hand = this.hands.get(0);
            if (hand.getCard(0).getValue() == hand.getCard(1).getValue())
            {
                this.splitFlag = true;
            }
            else
            {
                this.splitFlag = false;
            }
        }

        public void split()
        {
            Card card = this.hands.get(0).remove(1);
            Hand newHand = new Hand();

            newHand.add(card, false);
            this.hands.add(newHand);
        }

        // MOVES
        public void deal(Card card, int index, bool hidden)
        {
            this.hands.get(index).add(card, hidden);
        }

        public void deal(Card card, int index)
        {
            this.deal(card, index, false);
        }

        public void stand(int index)
        {
            this.hands.get(index).stand();
        }

        // HIDE/SHOW CARD
        public void hideCardInHand(int indexCard, int indexHand)
        {
            this.hands.get(indexHand).hideCard(indexCard);
        }

        public void showCardInHand(int indexCard, int indexHand)
        {
            this.hands.get(indexHand).showCard(indexCard);
        }

        // PAYOUT
        public void payoutHand(Hand hand, double multiplier)
        {
            this.money += (int)Math.round(hand.getBet() * multiplier);
        }

        // DISCARD
        public void discard()
        {
            foreach (Hand hand in this.hands)
            {
                hand.discard();
            }
            this.totalBet = 0;

            this.hands.clear();
            this.hands.add(new Hand());
            this.splitFlag = false;
        }
    }

    /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    class Hand
    {
        private int bet;

        private ArrayList<Card> cards;
        private int points;

        private bool hiddenCardFlag;
        private bool aceCardFlag;

        public const int NONE = 0;
        public const int BUST = 1;
        public const int STAND = 2;
        public const int BLACKJACK = 3;
        private int outcome;

        public Hand()
        {
            this.bet = 0;

            this.cards = new ArrayList<Card>();
            this.points = 0;

            this.hiddenCardFlag = false;
            this.aceCardFlag = false;

            this.outcome = NONE;
        }

        // GETTERS
        public int getBet()
        {
            return this.bet;
        }

        public Card getCard(int index)
        {
            return this.cards.get(index);
        }

        public ArrayList<Card> getCards()
        {
            return this.cards;
        }

        public int getPoints()
        {
            return this.points;
        }

        public int getOutcome()
        {
            return this.outcome;
        }

        public bool isBust()
        {
            return (this.outcome == BUST);
        }

        public bool isStand()
        {
            return (this.outcome == STAND);
        }

        public bool hasBlackjack()
        {
            return (this.outcome == BLACKJACK);
        }

        // OUTCOME SETTERS
        public void setOutcome(int outcome)
        {
            this.outcome = outcome; // NONE/BUST/STAND/BLACKJACK
        }

        public void bust()
        {
            this.outcome = BUST;
        }

        public void stand()
        {
            this.outcome = STAND;
        }

        public void blackjack()
        {
            this.outcome = BLACKJACK;
        }

        // BETS
        public void make_bet(int amount)
        {
            this.bet += amount;
        }

        // DRAW CARD
        public void add(Card card, bool hidden)
        {
            this.cards.add(card);
            if (!hidden)
            {
                addstate(card);
            }
            else
            {
                this.hiddenCardFlag = true;
                card.hide();
            }
        }

        public Card remove(int index)
        {
            Card card = this.cards.get(index);

            this.cards.remove(index);
            removestate(card);

            return card;
        }

        // HIDE/SHOW CARD
        public void hideCard(int index)
        {
            Card card = this.cards.get(index);

            card.hide();
            this.removestate(card);
            this.hiddenCardFlag = true;
        }

        public void showCard(int index)
        {
            Card card = this.cards.get(index);

            card.show();
            this.addstate(card);
            this.hiddenCardFlag = false;
        }

        public void addstate(Card card)
        {
            this.points += card.getValue();
            if (card.getValue() == 11)
            {
                this.aceCardFlag = true;
            }

            if (this.points == 21)
            {
                this.blackjack();
            }
            else if (this.points > 21)
            {
                if (!this.aceCardFlag)
                {
                    this.bust();
                }
                else
                {
                    this.points -= 10;
                    this.aceCardFlag = false;
                }
            }
        }

        public void removestate(Card card)
        {
            this.points -= card.getValue();
            if (card.getValue() == 11)
            {
                this.aceCardFlag = false;
            }
        }

        // DISCARD
        public void discard()
        {
            this.bet = 0;

            this.cards.clear();
            this.points = 0;

            this.hiddenCardFlag = false;
            this.aceCardFlag = false;

            this.outcome = NONE;
        }
    }

    /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    class Deck
    {
        private int size;
        private ArrayList<Card> cards;

        public Deck(int nrDecks)
        {
            this.cards = new ArrayList<Card>();
            this.reset(nrDecks);
        }

        // GETTERS
        public int get_size()
        {
            return this.size;
        }

        public ArrayList<Card> getCards()
        {
            return this.cards;
        }

        // AUTO FUNCTIONS
        public void reset(int nrDecks)
        {
            this.cards.clear();
            for (int k = 0; k < nrDecks; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        this.cards.add(new Card(i, j));
                    }
                }
            }
            this.size = this.cards.size();
        }

        public void shuffle()
        {
            for (int indexCurr = this.size; indexCurr != 0; indexCurr--)
            {
                int indexRand = (int)Math.floor(Math.random() * indexCurr);

                Card tempCard = this.cards.get(indexCurr - 1);
                this.cards.set(indexCurr - 1, this.cards.get(indexRand));
                this.cards.set(indexRand, tempCard);
            }
        }

        public Card draw()
        {
            Card card = this.cards.get(0);

            this.cards.remove(0);
            this.size = this.cards.size();

            return card;
        }

        public void add(Card card)
        {
            this.cards.add(card);
            this.size = this.cards.size();
        }

        public void clear()
        {
            this.cards.clear();
            this.size = 0;
        }
    }

    /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    class Card
    {
        private const String[] SUITS = {"♠️", "♥️", "♣", "♦️"};
        private const String[] NUMBS = {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
        private int suit;
        private int number;

        private bool hiddenFlag;

        public Card(int suit, int number, bool hidden)
        {
            this.suit = suit;
            this.number = number;
            this.hiddenFlag = hidden;
        }

        public Card(int suit, int number)
        {
            this(suit, number, false);
        }

        // GETTERS
        public int getValue()
        {
            if (this.number == 0)
            {
                return 11;
            }
            if (this.number > 9)
            {
                return 10;
            }
            return this.number + 1;
        }

        public bool isHidden()
        {
            return this.hiddenFlag;
        }

        // FUNCTIONS
        public void hide()
        {
            this.hiddenFlag = true;
        }

        public void show()
        {
            this.hiddenFlag = false;
        }

        // TOSTRING()
        public String toObjectString()
        {
            String printString = "Card { ";
            if (this.hiddenFlag)
            {
                printString += "number: ? suit: ? }";
            }
            else
            {
                printString += "number: " + NUMBS[this.number] + " suit: " + SUITS[this.suit] + " }";
            }

            return printString;
        }
    }

    /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    class Program
    {
        public static void placeBet(Game game, Player player, Scanner scan)
        {
            System.out.print(player.getName() + "'s bet: ");
            int bet = scan.nextInt();

            if (!game.betPlayer(player, 0, bet))
            {
                System.out.println("Invalid bet amount!");
                Client.placeBet(game, player, scan);
            }
            else
            {
                scan.nextLine();
            }
        }

        public static void manualdeal(Game game, Player player, Scanner scan)
        {
            for (int indexHand = 0; indexHand < player.getHands().size(); indexHand++)
            {
                int move = 1;
                while (player.getHand(indexHand).getOutcome() == Hand.NONE)
                {
                    System.out.print(player.getName() + ": ");
                    String option = scan.nextLine();
                    boolean flag = false;

                    switch (option)
                    {
                        case "split":
                            if (move == 1)
                            {
                                if (player.canSplit())
                                {
                                    game.splitPlayer(player);
                                    flag = true;
                                    move--;
                                }
                            }
                            break;
                        case "double":
                            if (move == 1)
                            {
                                if (game.doubledownPlayer(player, indexHand))
                                {
                                    flag = true;
                                }
                            }
                            break;
                        case "hit":
                            game.hitPlayer(player, indexHand);
                            flag = true; break;

                        case "stand":
                            game.standPlayer(player, indexHand);
                            flag = true; break;
                        default:
                    }

                    if (flag)
                    {
                        move++;
                        System.out.println(game.toObjectString(0));
                    }
                }
            }
        }

        public static void autodeal(Game game, int standPoints)
        {
            Hand hand = game.getDealer().getHand();
            while (hand.getOutcome() == Hand.NONE)
            {
                System.out.print("Dealer: ");

                if (hand.getPoints() < standPoints)
                {
                    System.out.println("Hit");
                    game.hitDealer();
                }
                else
                {
                    System.out.println("Stand");
                    game.standDealer();
                }

                System.out.println(game.toObjectString(0));
            }
        }

        public static void payout(Player player, Dealer dealer)
        {
            System.out.print(player.getName() + ": ");

            String outcomeStr;
            double multi;

            Hand dealerHand = dealer.getHand();
            int dealerOUTCOME = dealerHand.getOutcome();

            for (Hand hand: player.getHands())
            {
                int playerOUTCOME = hand.getOutcome();

                switch (playerOUTCOME)
                {
                    case Hand.BUST:
                        outcomeStr = "BUST"; multi = 0;
                        break;
                    case Hand.STAND:
                        switch (dealerOUTCOME)
                        {
                            case Hand.BLACKJACK:
                                outcomeStr = "LOSS"; multi = 0;
                                break;
                            case Hand.STAND:
                                if (hand.getPoints() < dealerHand.getPoints())
                                {
                                    outcomeStr = "LOSS"; multi = 0;
                                }
                                else if (hand.getPoints() == dealerHand.getPoints())
                                {
                                    outcomeStr = "PUSH"; multi = 1;
                                }
                                else
                                {
                                    outcomeStr = "WIN"; multi = 2;
                                }
                                break;
                            default:
                                outcomeStr = "WIN"; multi = 2;
                                break;
                        }
                        break;
                    case Hand.BLACKJACK:
                        if (dealerOUTCOME == Hand.BLACKJACK)
                        {
                            outcomeStr = "PUSH"; multi = 1;
                        }
                        else
                        {
                            outcomeStr = "BLACKJACK"; multi = 2.5;
                        }
                        break;
                    default:
                        outcomeStr = "HOW DID YOU GET HERE?"; multi = 100;
                        break;
                }

                player.payoutHand(hand, multi);
                System.out.print(outcomeStr + " x" + multi + " ");
            }
            System.out.println();
        }

        public static void main(String[] args)
        {
            ArrayList<Player> playerArr = new ArrayList<Player>();
            playerArr.add(new Player("42324", "Thotu", 100));
            playerArr.add(new Player("43324", "Fake Thotu", 100));

            Game game = new Game(playerArr);

            game.shuffleDeck();

            Scanner scan = new Scanner(System.in);

            boolean goNext;

            do
            {
                // BETS
                for (Player player : game.getPlayers())
                {
                    Client.placeBet(game, player, scan);
                }

                // ROUND START
                game.deal();
                System.out.println(game.toObjectString(0));

                // PLAYERS' TURNS
                for (Player player : game.getPlayers())
                {
                    Client.manualdeal(game, player, scan);
                }

                // DEALER'S TURN
                Dealer dealer = game.getDealer();

                dealer.showCard(0);
                System.out.println(game.toObjectString(0));

                Client.autodeal(game, 17);

                // PAYOUT
                for (Player player : game.getPlayers())
                {
                    Client.payout(player, dealer);
                }

                // ROUND END
                game.discard();
                game.refill();

                System.out.print("Another round? (Y): ");
                goNext = (scan.nextLine().equalsIgnoreCase("Y"));
            } while (goNext);

            scan.close();
        }
    }
}