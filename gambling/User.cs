using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gambling
{
    class User
    {
        public string name;
        private int cash;
        private Card card0;
        private Card card1;

        public User(string _name, int _cash)
        {
            name = _name;
            cash = _cash;
        }
        public User(string _name)
        {
            name = _name;
            cash = 1000;
        }
        ~User()
        {
            
        }

        public Card getCard(int index)
        {
            switch (index)
            {
                case 0:
                    return card0;
                case 1:
                    return card1;
                default:
                    return null;
            }
        }
        public bool setCard(int index, Card card)
        {
            switch (index)
            {
                case 0:
                    card0 = card;
                    return true;
                case 1:
                    card1 = card;
                    return true;
                default:
                    return false;
            }
        }
        public bool setTwoCards(Card card0, Card card1)
        {
            setCard(0, card0);
            setCard(1, card1);
            return true;
        }
        public int getCash()
        {
            return cash;
        }
        public void plusCash(int value)
        {
            cash += value;
            return;
        }
        
    }

}
