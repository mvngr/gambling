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
                  1000                    2000


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

        struct Player
        {
            string name;
            string money;
        }

        public MyConsoleOut(Game game)
        {
            getImage(game);
            calculateSpace(game);
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
        public void getImage(Game game)
        {
            Console.Clear();
            Console.WriteLine("Вы играете за: </todo>");
            enter();
            printUpZone(game);

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
        public void printUpZone(Game game)
        {
            List<int> lengthName = new List<int>();
            
            for(int i = 0; i < upUsersCount; i++)
            {
                if (i - 1 == game.blind_pos) //если блайнд был у предыдущего игрока = -2 пробела
                    printAnySpace(6);
                else
                    spacex8();

                int lenGetCash = getLenghtInt(game.users[i].getCash());
                
                if(game.users[i].name.Length >= lenGetCash)
                    lengthName.Add(game.users[i].name.Length);
                else
                    lengthName.Add(lenGetCash);
                
                Console.Write(game.users[i].name);

                if (i - 1 == game.blind_pos) //фишка диллера
                    Console.Write(" D");

                printAnySpace(lengthName.Last() - game.users[i].name.Length); //добавляем пробелы если нужно (для выравнивания)
            }
            Console.Write('\n');
            for(int i = 0; i < upUsersCount; i++)
            {
                spacex8();

                Console.Write(game.users[i].getCash());
                printAnySpace(lengthName.Last() - getLenghtInt(game.users[i].getCash()));
            }
            Console.Write('\n');
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
                downUsersCount = game.users.Count - upUsersCount;
            }
            return;
        }

        int upUsersCount;
        int downUsersCount;
        int leftRightUsersCount;

        List<string> moves;


    }
}
