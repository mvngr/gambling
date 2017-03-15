using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace gambling
{
    class MyConsoleOut
    {
        /*

        You: Name

                  Player1  D              Player2
                  1000 | 9S 8S            2000


        Player3           JD 10C 2S TC 2C              Player4
        1000                                           500
                            22000 1000

                  Player5
                  0

        Last 10 moves:
        P1: call
        P2: bet 20
        P3: ...
        ...
        ...
        ...

        =======

        Your turn! [/help]:

        /fold

        */
        const string DESIGN1 = "=======";
        const string YOURTURN = "Твой ход [/help]: ";

        public struct TwoStrings
        {
            public string s1;
            public string s2;
            public int length;
        }

        public MyConsoleOut(Game game)
        {
            calculateSpace(game);
            getImage(game);
        }
        public void enter()
        {
            Console.WriteLine('\n');
            return;
        }
        public void spacex8()
        {
            Console.Write("        ");
            return;
        }
        public void printAnySpace(int count)
        {
            while(count > 0)
            {
                count--;
                Console.Write(' ');
            }
            return;
        }
        public string getAnySpace(int count)
        {
            string res = null;
            while (count > 0)
            {
                count--;
                res += ' ';
            }
            return res;
        }
        public void getImage(Game game)
        {
            Console.Clear();
            Console.WriteLine("Вы играете за: </todo>");
            enter();
            printZone(game, 0, upUsersCount);
            enter();
            printZone(game);
            enter();
            printZone(game, upUsersCount + leftRightUsersCount - 1, leftRightUsersCount + upUsersCount + downUsersCount - 1);
            enter();
            printLog(game);

        }
        private int getLenghtInt(int e)
        {
            int len = 0;
            while (e != 0)
            {
                e /= 10;
                len++;
            }
            return len;
        }
        public TwoStrings alignLength(TwoStrings inp)
        {
            int l = inp.s1.Length - inp.s2.Length;
            if(l > 0)
            {
                inp.length = inp.s1.Length;
                inp.s2 += getAnySpace(l);
            }
            else
            {
                inp.length = inp.s2.Length;
                inp.s1 += getAnySpace(-l);
            }
            return inp;
        }
        private string changeSuit(string inp)
        {
            //♠ ♤ ♥ ♡ ♣ ♧ ♦ ♢
            //Hearts, Diamonds, Clubs, Spades
            switch (inp[inp.Length - 1])
            {
                case 'H':
                    inp = inp.Replace('H', '♥');
                    break;
                case 'D':
                    inp = inp.Replace('D', '♦');
                    break;
                case 'C':
                    inp = inp.Replace('C', '♣');
                    break;
                case 'S':
                    inp = inp.Replace('S', '♠');
                    break;
                default: break;
            }
            return inp;
        }
        public TwoStrings getTSUser(User user, bool blind)
        {
            TwoStrings res;
            res.s1 = user.name;
            if (blind)
                res.s1 += " D";
            if (user.getCard(0) != null)
                res.s2 = user.getCash().ToString() + " | " + user.getCard(0).toString() + ' ' + user.getCard(1).toString();
            else
                res.s2 = user.getCash().ToString();
            res.length = -1;
            res = alignLength(res);
            
            return res;
            
        }
        private void parseTS(TwoStrings ts, List<string> s1, List<string> s2)
        {
            s1.Add(ts.s1);
            s2.Add(ts.s2);
            return;
        }
        private void printZone(Game game)
        {
            List<string> s1 = new List<string>();
            List<string> s2 = new List<string>();

            parseTS(getTSUser(game.users.Last(), true), s1, s2);
            int i = 0, countChar = 0;
            string temp = "";
            while (i != 5)
            {
                if (i < game.openCards.Count)
                {
                    temp += game.openCards[i].toString();
                    countChar += Convert.ToInt16(s1.Last());
                }
                else
                {
                    temp += "  ";
                    countChar += 2;
                }
                i++;
            }
            s1.Add(temp);
            s2.Add(getAnySpace(countChar));

            parseTS(getTSUser(game.users[upUsersCount], true), s1, s2);

            foreach (string s in s1)
            {
                Console.Write(s);
                spacex8();
            }
            Console.Write('\n');
            foreach (string s in s2)
            {
                Console.Write(s);
                spacex8();
            }
            Console.Write('\n');

        }
        private void printZone(Game game, int stIndUsers, int endIndUsers)
        {
            List<string> s1 = new List<string>();
            List<string> s2 = new List<string>();
            if (stIndUsers <= endIndUsers)
            {
                for (int i = stIndUsers; i < endIndUsers; i++)
                    if (game.blind_pos == i)
                        parseTS(getTSUser(game.users[i], true), s1, s2);
                    else
                        parseTS(getTSUser(game.users[i], false), s1, s2);
            }
            else
            {
                for (int i = endIndUsers; i > stIndUsers; i--)
                    if (game.blind_pos == i)
                        parseTS(getTSUser(game.users[i], true), s1, s2);
                    else
                        parseTS(getTSUser(game.users[i], false), s1, s2);
            }
            foreach (string s in s1)
            {
                spacex8();
                Console.Write(s);
            }
            Console.Write('\n');
            foreach (string s in s2)
            {
                spacex8();
                Console.Write(s);
            }
            Console.Write('\n'); 
        }
        private void printLog(Game game)
        {
            if (game.log.Count == 0)
                return;
            Console.WriteLine(DESIGN1);
            foreach(string s in game.log)
                Console.WriteLine(s);
            Console.WriteLine(DESIGN1);
            return;
        }
        private void calculateSpace(Game game)
        {
            bool def = false;
            switch(game.users.Count)
            {
                case 1:
                    upUsersCount = 1;
                    downUsersCount = 0;
                    leftRightUsersCount = 0;
                    break;
                case 2:
                    upUsersCount = 1;
                    downUsersCount = 1;
                    leftRightUsersCount = 0;
                    break;
                case 3:
                    upUsersCount = 2;
                    downUsersCount = 1;
                    leftRightUsersCount = 0;
                    break;
                case 4:
                    upUsersCount = 1;
                    downUsersCount = 1;
                    leftRightUsersCount = 2;
                    break;
                default:
                    def = true;
                    break;
            }
            if (def)
            {
                leftRightUsersCount = 2;
                upUsersCount = (game.users.Count - 2) / 2 + 1;
                downUsersCount = game.users.Count - upUsersCount - 2;
            }
            return;
        }

        int upUsersCount;
        int downUsersCount;
        int leftRightUsersCount;

        


    }
}
