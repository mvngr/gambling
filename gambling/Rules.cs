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
        List<int[]> top;

        int rank;
        int biggestCardNum;
        int[] numC;
        int[] suitC;

        public Rules(List<Card> openCards)
        {
            cards = new List<Card>();
            top = new List<int[]>();
            rank = 0;
            
            foreach (Card c in openCards)
                cards.Add(c);

        }
        public List<int> calculateRes()
        {
            List<int> result = new List<int>();
            int r = 20, bC = -5; //числа взяты с потолка, чтобы уж точно началось заполнение переменных в начале
            for(int i = 0; i < top.Count; i++)
            {
                if (top[i][0] > r)
                {
                    r = top[i][0];
                    bC = top[i][1];
                    result.Clear();
                    result.Add(i);
                }
                else
                    if(top[i][0] == r && top[i][1] >= bC)
                {
                    if(top[i][1] == bC)
                        result.Add(i);
                    else
                    {
                        bC = top[i][1];
                        result.Clear();
                        result.Add(i);
                    }
                }
            }
            return result;
        }
        public void setNewCards(Card c1, Card c2)
        {
            rank = 0;

            if(cards.Count == 5)
            {
                cards[3] = c1;
                cards[4] = c2;
            }
            else
            {
                cards.Add(c1);
                cards.Add(c2);
            }

            load();

            top.Add(getRank()); //сразу считает значение

            return;
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
        public int[] getRank()
        {
            int i = 1;
            biggestCardNum = StraightFlush();
            int[] res = new int[2]; // 1 элемент - рэйтинг, 2 элемент - высшая карта

            while (i != 11)
            {
                res[0] = i;
                switch (i)
                {
                    case 1:
                        if (biggestCardNum == 12)
                        {
                            res[1] = biggestCardNum;
                            return res;
                        }
                           
                        break;
                    case 2:
                        if (biggestCardNum != -1)
                        {
                            res[1] = biggestCardNum;
                            return res;
                        }
                            
                        break;
                    case 3:
                        biggestCardNum = FourOfAKind();
                        if (biggestCardNum != -1)
                        {
                            res[1] = biggestCardNum;
                            return res;
                        }
                        break;
                    case 4:
                        biggestCardNum = FullHouse();
                        if (biggestCardNum != -1)
                        {
                            res[1] = biggestCardNum;
                            return res;
                        }
                        break;
                    case 5:
                        biggestCardNum = Flush();
                        if (biggestCardNum != -1)
                        {
                            res[1] = biggestCardNum;
                            return res;
                        }
                        break;
                    case 6:
                        biggestCardNum = Straight();
                        if (biggestCardNum != -1)
                        {
                            res[1] = biggestCardNum;
                            return res;
                        }
                        break;
                    case 7:
                        biggestCardNum = ThreeOfAKind();
                        if (biggestCardNum != -1)
                        {
                            res[1] = biggestCardNum;
                            return res;
                        }
                        break;
                    case 8:
                        biggestCardNum = twoPairs();
                        if (biggestCardNum != -1)
                        {
                            res[1] = biggestCardNum;
                            return res;
                        }
                        break;
                    case 9:
                        biggestCardNum = Pair();
                        if (biggestCardNum != -1)
                        {
                            res[1] = biggestCardNum;
                            return res;
                        }
                        break;
                    default:
                        res[1] = -1;
                        return res;
                }
                i++;
            }
            res[1] = -1;
            return res;
        }

        //Rules funcs

        public int Pair() //9
        {
            int i = 12;
            while (i != -1)
            {
                if (numC[i] == 2)
                    return i;
                i--;
            }
            return -1;
        }
        public int twoPairs() //8
        {
            int i = 12;
            int count = 0;
            while (i != -1)
            {
                i--;
                if(numC[i] == 2)
                {
                    if (count != 0)
                        return i;
                    count = i;
                }
            }
            return -1;
        }
        public int ThreeOfAKind()//7
        {
            int i = 12;
            while (i != -1)
            {
                if (numC[i] == 3)
                    return i;
                i--;
            }
            return -1;
        }
        public int Straight()//6
        {
            int i = 12;
            int count = 0;
            while (i != -1)
            {
                if (numC[i] > 0)
                {
                    count++;
                    if (count == 5)
                        return i+4; //debug
                }  
                else
                    count = 0;
                i--;
            }
            if (count == 4 && numC[12] > 0) //обратный стрит
                return 5;
            else
                return -1;
        }
        public int Flush() //5
        {
            int i = 3;
            while (i != -1)
            {
                if (suitC[i] == 5)
                {
                    int j = 0, temp = 0;
                    while (j < 5)
                    {
                        Card tmp = cards.Find(x => x.suitCard == suitC[i]);
                        if (tmp.numCard > temp)
                            temp = tmp.numCard;
                        j++;
                    }
                    return temp;
                }
                i--;
            }
            return -1;
        }
        public int FullHouse() //4
        {
            int i = 12;
            int num1 = 0, num2 = 0;
            bool r1 = false, r2 = false;
            while (i != -1)
            {
                if (numC[i] == 3 && r1)
                {
                    r1 = true;
                    num1 = i;
                }
                    
                else
                    if (numC[i] == 2 && r2)
                {
                    r2 = true;
                    num2 = i;
                }
                        
                if (r1 && r2)
                {
                    if (num1 > num2)
                        return num1;
                    else
                        return num2;
                }
                i--;
            }
            return -1;
        }
        public int FourOfAKind() //3
        {
            int i = 12;
            while (i != -1)
            {
                if (numC[i] == 4)
                    return i;
                i--;
            }
            return -1;
        }
        private int StraightFlush() //2 + 1
        {
            int i = Straight();
            int cpy = i - 5;
            if(i > 1)
            {
                while (i > cpy)
                {
                    int suitCa = 5; // 5 = любая
                    Card tmp = cards.Find(x => x.numCard == i);
                    if (tmp.suitCard == suitCa || suitCa == 5)
                        suitCa = tmp.suitCard;
                    else
                        return -1;
                    i--;
                }
                return i;
            }
            else
            return -1;
        }

    }
}
