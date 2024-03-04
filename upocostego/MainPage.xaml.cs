using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upocostego.Models;
using Xamarin.Forms;

namespace upocostego {
    public partial class MainPage : ContentPage {
        Deck deck = new Deck();
        List<Card> playerCard=new List<Card>();
        List<Card> computerCards=new List<Card>();
        Card middleCard = null;
        bool win=false;
        bool playerAction = true;
        public MainPage() {
            InitializeComponent();
            playerCard = deck.GetPlayerCards(3);
            computerCards = deck.GetPlayerCards(3);
            SetLastCard();
            RenderPlayerCards();
        }

        private void RenderPlayerCards() {
            if (win)
                return;
            PlayerCards.Children.Clear();
            playerCard = playerCard.OrderBy(z => z.Color.ToString()).ToList();

            foreach (Card item in playerCard) {
                Button button = item.Render();
                button.Clicked += PlayerCardClick;
                button.BindingContext= item;
                if(playerAction) {
                    if(item.Color==middleCard.Color||item.Value==middleCard.Value||item.Action==SpecialActions.Color) {
                        button.BorderWidth = 2;
                        button.BorderColor = Color.Black;
                    }
                }
                PlayerCards.Children.Add(button);
            }
            ComputerCardsN.Text= computerCards.Count.ToString()+" Ilosc kart przeciwnika";

        }

        private void SetLastCard(Card c = null) {
            if (middleCard == null)
            {
                middleCard = deck.deck[0];
                deck.deck.RemoveAt(0);
            }
            else
                middleCard = c;
            LastCardText.Text = middleCard.Value.ToString();
            LastCardStack.BackgroundColor = middleCard.Color;
        }

        private void PlayerCardClick(object sender, EventArgs e) {
            if(win) return;
            Card c = (sender as Button).BindingContext as Card;
            if(c.Color == middleCard.Color || c.Value == middleCard.Value || c.Action == SpecialActions.Color) {
                SetLastCard(c);
                if (c.Action != SpecialActions.Normal)
                    SpecialCardAction(computerCards, c);
                playerCard.Remove(c);
                playerAction = false;
                RenderPlayerCards();
                WinCheck();
                ComputerMove();
            }
        }

        private void SpecialCardAction(List<Card> target,Card card) {
            if (deck.deck.Count < 10)
                deck.GenerateDeck();
            if (card.Action == SpecialActions.AddTwo) {
                for(int i = 0; i < 2; ++i) {
                    target.Add(deck.deck[0]);
                    deck.deck.RemoveAt(0);
                }
            }
            if (card.Action == SpecialActions.AddFour) {
                for (int i = 0; i < 4; ++i) {
                    target.Add(deck.deck[0]);
                    deck.deck.RemoveAt(0);
                }
            }
        }

        async void WinCheck() {
            if(playerCard.Count == 0)
            {
                win = true;
                await DisplayAlert("Wygrana", "Wygrał gracz", "OK");
            }
            if (computerCards.Count == 0)
            { 
                win = true;
                await DisplayAlert("Wygrana", "Wygrał komputer", "OK");
            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if(playerAction&&!win)
            {
                playerCard.Add(deck.deck[0]);
                deck.deck.RemoveAt(0);
                playerAction = false;
                ComputerMove();
            }
        }
        private async void ComputerMove() {
            if (win == true) return;

            List<Card> possibleMove = computerCards.Where(e=> e.Color.ToString() == middleCard.Color.ToString() || e.Value == middleCard.Value || e.Action == SpecialActions.Color).ToList();
            if(possibleMove.Count > 0)
            {
                Random r = new Random();
                int n = r.Next(possibleMove.Count);
                await Task.Delay(1000);
                SetLastCard(possibleMove[n]);
                SpecialCardAction(playerCard, possibleMove[n]);
                ComputerAction.Text = "Komputer dał" + possibleMove[n].Value + " " + possibleMove[n].Color.ToString();
                computerCards.Remove(possibleMove[n]);
            }else
            {
                ComputerAction.Text = "Komputer dobral";
                computerCards.Add(deck.deck[0]);
                deck.deck.RemoveAt(0);
            }
            playerAction = true;
            WinCheck();
            RenderPlayerCards();
        }
    }
}
