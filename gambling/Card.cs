using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gambling
{
    class Card
    {
        public int numCard;
        public int suitCard;

        public Card()
        {
            generateCard();
        }

        public string intToNum(int num)
        {
            switch (num)
            {
                case 0: return "2";
                case 1: return "3";
                case 2: return "4";
                case 3: return "5";
                case 4: return "6";
                case 5: return "7";
                case 6: return "8";
                case 7: return "9";
                case 8: return "10";
                case 9: return "J";
                case 10: return "Q";
                case 11: return "K";
                case 12: return "T";
                default: return null;
            }
        }
        public char intToSuit(int num)
        {
            switch (num)
            {
                case 0: return 'H';
                case 1: return 'D';
                case 2: return 'C';
                case 3: return 'S';
                default: return '\0';
            }
        }
        public string toString()
        {
            string str = intToNum(numCard) + intToSuit(suitCard);
            return str;
        }
        public void generateCard()
        {
            Random rand = new Random();
            
            numCard = rand.Next(12);
            suitCard = rand.Next(4);
            return;

        }
    }
}
