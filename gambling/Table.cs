using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gambling
{

    class Table
    {

        public int blind_cost;
        public int blind_pos;
        public List<User> users;
        public int chairs;
        public List<Card> usedCards;

        public Table()
        {
            users = new List<User>(6);
            usedCards = new List<Card>();
            blind_cost = 50;
            chairs = 6;
            blind_pos = 4;
        }
        public Table(int maxPeople, int blind)
        {
            //todo
        }
        ~Table()
        {
            users.Clear();
            usedCards.Clear();
        }

        public bool addUser(User user)
        {
            if (users.Count <= chairs)
            {
                users.Add(user);
                return true;
            }
            else
                return false;
        }
        public virtual void printAllInfo()
        {
            for (int i = 0; i < users.Count; i++)
            {
                string tmp = users[i].name + " | Сидит: " + (i + 1) + " | Монеток: " + users[i].getCash();
                Console.WriteLine(tmp);
            }
            return;
        }
        public void nextBlind()
        {
            if (blind_pos == users.Count - 1)
                blind_pos = 0;
            else
                blind_pos++;
            return;
        }
    }
}
