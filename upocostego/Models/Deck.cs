using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace upocostego.Models {
    public class Deck {
        public List<Card> deck = new List<Card>();
        private readonly List<System.Drawing.Color> Colors = new List<System.Drawing.Color>() {
            System.Drawing.Color.Red, System.Drawing.Color.Green,System.Drawing.Color.Blue,System.Drawing.Color.Yellow,
        };
        private readonly List<string> values = new List<string>() { 
            "1","2","3","4","5","6","7","8","9","+2","+4","kolor"
        };
        public Deck()
        {
            GenerateDeck();
        }
        public void GenerateDeck() {
            foreach (System.Drawing.Color item in Colors) {
                foreach (string value in values) {
                    deck.Add(new Card() {
                        Color = item,
                        Value = value,
                        Action = GetAction(value)
                    });
                }
            }
            Shuffle();
        }
        public SpecialActions GetAction(string value) {
            switch (value) {
                case "+2":
                    return SpecialActions.AddTwo;
                case "+4":
                    return SpecialActions.AddFour;
                case "kolor":
                    return SpecialActions.Color;
                default:
                    return SpecialActions.Normal;
            }
        }
        public void Shuffle() {
            Random r=new Random();
            deck.OrderBy(z => r.Next()).ToList();
        }
        public List<Card> GetPlayerCards(int x) {
            List <Card> cards = deck.GetRange(0,x).OrderBy(z => z.Color.ToString()).ToList();
            deck.RemoveRange(0,x);
            return cards;
        }
    }
}
