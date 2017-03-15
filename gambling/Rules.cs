using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gambling
{
    class Rules
    {
        List<Card> cards;
        int rank;
        int[] numC;
        int[] suitC;

        public Rules(Card c1, Card c2, List<Card> openCards)
        {
            cards = new List<Card>();
            rank = 0;

            cards.Add(c1);
            cards.Add(c2);
            foreach (Card c in openCards)
                cards.Add(c);

            load();
        }
        void load()
        {
            numC = new int[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            suitC = new int[4] { 0, 0, 0, 0 };
            foreach (Card c in cards)
            {
                numC[c.numCard]++;
                suitC[c.suitCard]++;
            }
            return;
        }
        public int getRank()
        {
            int i = 1;
            int temp = specStraightFlush();

            while (i != 11)
            {
                switch (i)
                {
                    case 1:
                        if (temp == 12)
                            return 1;
                        break;
                    case 2:
                        if (temp != 0)
                            return 2;
                        break;
                    case 3:
                        if (FourOfAKind())
                            return 3;
                        break;
                    case 4:
                        if (FullHouse())
                            return 4;
                        break;
                    case 5:
                        if (Flush())
                            return 5;
                        break;
                    case 6:
                        if (Straight())
                            return 6;
                        break;
                    case 7:
                        if (ThreeOfAKind())
                            return 7;
                        break;
                    case 8:
                        if (twoPairs())
                            return 8;
                        break;
                    case 9:
                        if (Pair())
                            return 9;
                        break;
                    default: return 10;
                }
                i++;
            }
            return 10;
        }

        //Rules funcs

        public bool Pair() //9
        {
            int i = 12;
            while (i != -1)
            {
                if (numC[i] == 2)
                    return true;
                i--;
            }
            return false;
        }
        public bool twoPairs() //8
        {
            int i = 12;
            int count = 0;
            while (i != -1)
            {
                i--;
                if(numC[i] == 2)
                {
                    count++;
                    if (count == 2)
                        return true;
                }
            }
            return false;
        }
        public bool ThreeOfAKind()//7
        {
            int i = 12;
            while (i != -1)
            {
                if (numC[i] == 3)
                    return true;
                i--;
            }
            return false;
        }
        public bool Straight()//6
        {
            int i = 12;
            int count = 0;
            while (i != -1)
            {
                if (numC[i] > 0)
                {
                    count++;
                    if (count == 5)
                        return true;
                }  
                else
                    count = 0;
                i--;
            }
            if (count == 4 && numC[12] > 0) //обратный стрит
                return true;
            else
                return false;
        }
        private int specStraight() //6 копия, возвращает наибольшую из цифр
        {
            int i = 12;
            int count = 0;
            while (i != -1)
            {
                if (numC[i] > 0)
                {
                    count++;
                    if (count == 5)
                        return i + 4; //debug
                }
                else
                    count = 0;
                i--;
            }
            if (count == 4 && numC[12] > 0) //обратный стрит
                return 5;
            else
                return 0;
        }
        public bool Flush() //5
        {
            int i = 3;
            while (i != -1)
            {
                if (suitC[i] == 5)
                    return true;
                i--;
            }
            return false;
        }
        public bool FullHouse() //4
        {
            int i = 12;
            bool r1 = false, r2 = false;
            while (i != -1)
            {
                if (numC[i] == 3)
                    r1 = true;
                else
                    if (numC[i] == 2)
                        r2 = true;
                if (r1 && r2)
                    return true;
                i--;
            }
            return false;
        }
        public bool FourOfAKind() //3
        {
            int i = 12;
            while (i != -1)
            {
                if (numC[i] == 4)
                    return true;
                i--;
            }
            return false;
        }
        private int specStraightFlush() //2 + 1
        {
            int i = specStraight();
            int cpy = i - 5;
            if(i != 0)
            {
                while (i > cpy)
                {
                    int suitC = 5; // 5 = любая
                    Card tmp = cards.Find(x => x.numCard == i);
                    if (tmp.suitCard == suitC || suitC == 5)
                        suitC = tmp.suitCard;
                    else
                        return 0;
                }
                return i;
            }
            else
            return i;
        }

    }
}
