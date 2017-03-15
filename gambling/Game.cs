using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gambling
{
    class Game : Table
    {
        const string CALL = " заколлировал ставку в размере ";
        const string ALLIN = " ставит все свои фишки";
        const string RACE = " поднял ставку до ";
        const string CHECK = " пропустил свой ход";
        const string FOLD = " сбросил карты";

        List<Bet> bets; //хранит в себе суммы, поставленные в происходящем кону
        public List<string> log;

        public Game(Table table)
        {
            blind_cost = table.blind_cost;
            blind_pos = table.blind_pos;
            chairs = table.users.Count;
            usedCards = table.usedCards;
            users = table.users;
            bets = new List<Bet>(); 
            log = new List<string>();

            cons = new MyConsoleOut(this);

            while (true)
            {
                fillBetArray(bets, chairs);
                dealTheCards();
                makeBlind();
                printAllInfo();
                move(blind_cost);
                openThreeNewCards();
                move(0);
                openNewPublicCard();
                move(0);
                openNewPublicCard();
                endRound();
     
            }
        }
        ~Game()
        {

        }

        struct Command
        {
            public string cmd;
            public string args;
        }
        struct Bet
        {
            public bool fold;
            public bool allin;
            public int amount;
        }

        private MyConsoleOut cons;

        public override void printAllInfo()
        {
            cons.getImage(this);
        }
        private void endRound()
        {
            log.Add("Раунд закончился");
            //todo проверка кто выйграл
            bets.Clear();
            fillBetArray(bets, chairs);
            nextBlind();

        }
        private int getRankCombinations(Card n1, Card n2, List<Card> openCards)
        {
            //todo
            return 0;
        }
        private void fillBetArray(List<Bet> bets, int count)
        {
            for(int i = 0; i < count; i++)
            {
                Bet tmp;
                tmp.allin = false;
                tmp.fold = false;
                tmp.amount = 0;
                bets.Add(tmp);
            }
        }
        private bool checkCardToAllUsers(Card card)
        {
            Card tmp = usedCards.Find(x => (x.numCard == card.numCard && x.suitCard == card.suitCard));
            if (tmp == null)
                return true;
            else
                return false;
        }        
        private void dealTheCards()
        {
            foreach(User user in users)
            {
                Card c0 = new Card();
                Card c1 = new Card();

                while (!checkCardToAllUsers(c0))
                    c0 = new Card();
                usedCards.Add(c0);
                while (!checkCardToAllUsers(c1))
                    c1 = new Card();
                usedCards.Add(c1);

                user.setTwoCards(c0, c1);
                
            }
        }
        private int cycle(int inp)
        {
            if (inp >= users.Count)
                return inp % users.Count;
            else
                return inp;
        }
        private bool addBet(int user_ind, int cash)
        {
            user_ind = cycle(user_ind);

            if (bets[user_ind].allin)
                return true;
            if (bets[user_ind].fold)
                return false;

            Bet tmp;
            if (users[user_ind].getCash() <= cash)
            {
                tmp.allin = true;
                users[user_ind].plusCash(-users[user_ind].getCash());
                tmp.amount = users[user_ind].getCash() + bets[user_ind].amount;
                tmp.fold = false;

                bets[user_ind] = tmp;

                return true;
            }
            else
            {
                
                tmp.fold = false;
                tmp.allin = false;
                tmp.amount = cash + bets[user_ind].amount;

                bets[user_ind] = tmp;
                users[user_ind].plusCash(-cash);

                return true;
            }
            
            
        }
        private void fold(int user_ind)
        {
            Bet tmp;
            tmp = bets[user_ind];
            tmp.fold = true;

            bets[user_ind] = tmp;

            Card nulCard = null;

            users[user_ind].setCard(0, nulCard);
            users[user_ind].setCard(1, nulCard);

            return;
        }
        private void makeBlind()
        {

            nextBlind();

            addBet(blind_pos + 1, blind_cost / 2);
            addBet(blind_pos + 2, blind_cost);
            
            return;
        }
        public void help(string cmd)
        {
            switch (cmd)
            {
                case "/race":
                    Console.WriteLine("/race [сумма, которую вы хотите поставить]");
                    break;
                case "/help":
                    Console.WriteLine("/help Существующие команды:\n/bet\n/call\n/fond\n/allin\n/check");
                    break;
                default:
                    Console.WriteLine("У нас нет команды " + cmd);
                    break;
            }
        }
        private Command getCommandFromString(string inp)
        {
            Command res;
            while (inp.Length == 0)
            {
                inp = Console.ReadLine();
            }

            if (inp[0] != '/')
            {
                res.cmd = null;
                res.args = inp;
                return res;
            }
            int i = 0;
            while (i < inp.Length && inp[i] != ' ')
            {
                i++;
            }
            if (i == inp.Length)
            {
                res.cmd = inp;
                res.args = null;
            }
            else
            {
                res.cmd = inp.Substring(0, i);
                res.args = inp.Substring(i + 1, inp.Length - i - 1);
            }
            return res;
        }
        private bool checkNoArgs(string cmd)
        {
            switch (cmd)
            {
                case "/call": return true;
                case "/allin": return true;
                case "/fold": return true;
                case "/help": return true;
                case "/check": return true;
                default: return false;
            }
        }
        private void addHistory(string s)
        {
            if(log.Count == 10)
                log.RemoveAt(0);
            log.Add(s);
            return;
        }
        private string getBegining(int user_ind)
        {
            return "Игрок " + users[user_ind].name;
        }
        public int turn(int user_ind, int current_bet)
        {
            string command = Console.ReadLine();
            Command inp = getCommandFromString(command);
            if (inp.args == null && !checkNoArgs(inp.cmd))
                help(inp.cmd);
            else
            {
                if (inp.cmd == "/bet")
                    inp.cmd = "/race";
                switch (inp.cmd)
                {
                    case "/call":
                        addBet(user_ind, current_bet - bets[user_ind].amount); //todo
                        addHistory(getBegining(user_ind) + CALL + current_bet);
                        return current_bet;

                    case "/race": //todo после /call может увеличивать количество фишек
                        int tmp = Convert.ToInt32(inp.args);
                        if (tmp <= 0)
                        {
                            Console.WriteLine("Неправильный синтаксис в /race");
                            return -1;
                        }
                        if (Convert.ToInt32(inp.args) >= blind_cost)
                        {
                            addBet(user_ind, tmp);
                            addHistory(getBegining(user_ind) + RACE + tmp);
                            return tmp;
                        }
                        else
                        {
                            Console.WriteLine("Можно повышать только на значение большее, чем блайнд");
                            return -1;
                        }
                    case "/allin": //good job
                        int temp = current_bet;
                        if (users[user_ind].getCash() >= current_bet)
                            temp = users[user_ind].getCash();
                        addBet(user_ind, users[user_ind].getCash());
                        addHistory(getBegining(user_ind) + ALLIN);
                        return temp;
                    case "/fold": 
                        fold(user_ind);
                        addHistory(getBegining(user_ind) + FOLD);
                        return current_bet;
                    case "/check":
                        addHistory(getBegining(user_ind) + CHECK);
                        return current_bet;

                    default:
                        Console.WriteLine("Не могу прочитать команду" + inp.cmd);
                        return -1;
                }   
            }
            return -1;
        }
        private bool checkBets(int cur_bet)
        {
            foreach(Bet b in bets)
            {
                if (!b.fold && !b.allin && b.amount < cur_bet)
                    return false;
            }
            return true;
        }
        private bool checkPlayUsers()
        {
            int count = 0;
            bool allin = false;
            foreach(Bet b in bets)
                if (!b.fold)
                {
                    count++;
                    if (b.allin)
                    {
                        count--;
                        allin = true;
                    }    
                }
            
            if (count < 2 || (allin && count < 1))
                return false;
            else
                return true;
        }
        private void findWinner()
        {
            int i = 0;
            while (i < 5)
            {
                //todo

                i++;
            }
        }
        private void move(int start_bet)
        {
            int moveUserInd = cycle(blind_pos + 3); //начало хода
            int cur_bet = start_bet; //текущая ставка err - не включает в себя блайнды
            int tmp;
            while (true)
            {
                if (users[moveUserInd].getCard(0) != null) {
                    Console.WriteLine("Ход игрока " + users[moveUserInd].name + " | Карты: " + users[moveUserInd].getCard(0).toString() + ' ' + users[moveUserInd].getCard(1).toString() + ':');
                    do
                    {
                            tmp = turn(moveUserInd, cur_bet);
                            if (tmp > cur_bet)
                                cur_bet = tmp;

                            printAllInfo();
                    }
                    while (tmp == -1);

                    if(checkPlayUsers())
                    {
                        
                    }

                    if (moveUserInd == cycle(blind_pos + 2) && checkBets(cur_bet))
                        return;
                }

                moveUserInd = cycle(moveUserInd + 1); //двигает ход
            }
            
        }
        private void openNewPublicCard()
        {
            Card temp;
            do
                temp = new Card();
            while (!checkCardToAllUsers(temp));
            usedCards.Add(temp);
            openCards.Add(temp);
            printAllInfo();
            return;
        }
        private void openThreeNewCards()
        {
            openNewPublicCard();
            openNewPublicCard();
            openNewPublicCard();
            return;
        }


        
    }
}
