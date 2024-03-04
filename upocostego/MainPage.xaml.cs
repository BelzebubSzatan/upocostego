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
            throw new NotImplementedException();
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

        }

        private void SpecialCardAction(List<Card> target,Card card) {

        }

        async void WinChech() {
            if(playerCard.Count == 0)
            {
                await DisplayAlert("Wygrana", "Wygrał gracz", "OK");
                win = true;
            }
            if (computerCards.Count == 0)
            { 
                await DisplayAlert("Wygrana", "Wygrał komputer", "OK");
                win = true;
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
        private void ComputerMove() {

        }
    }
}
