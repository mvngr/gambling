using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace gambling
{
    class Program
    {
        void connectSQL()
        {
            string serverName = "y99923kl.beget.tech"; // Адрес сервера (для локальной базы пишите "localhost")
            string userName = "y99923kl_db"; // Имя пользователя
            string dbName = "y99923kl_db"; //Имя базы данных
            string port = "3306"; // Порт для подключения
            string password = "myPassword123"; // Пароль для подключения
            string connStr = "server=" + serverName +
                ";user=" + userName +
                ";database=" + dbName +
                ";port=" + port +
                ";password=" + password + ";";

            MySqlConnection conn = new MySqlConnection(connStr);
            Console.WriteLine("new conn ..");
            conn.Open();
            string sql = "SELECT * FROM `users`"; // Строка запроса
            MySqlScript script = new MySqlScript(conn, sql);
            int count = script.Execute();
            Console.WriteLine("Executed " + count + " statement(s).");
            Console.WriteLine("Delimiter: " + script.Delimiter);
        }
        static void help(string cmd)
        {
            switch(cmd)
            {
                case "/add":
                    Console.WriteLine("/add используется для добавления нового пользователя\nСинтаксис: /add [Имя пользователя]");
                    break;
                case "/start":
                    Console.WriteLine("/start используется для обозначения начала игры");
                    break;
                default:
                    Console.WriteLine("У нас нет команды " + cmd);
                    break;
            }
        }
        struct Command
        {
            public string cmd;
            public string args;
        }
        static private Command getCommandFromString(string inp)
        {
            Command res;
            if (inp == null)
            {
                res.cmd = null;
                res.args = null;
                return res;
            }
                
            if (inp[0] != '/')
            {
                res.cmd = null;
                res.args = inp;
                return res;
            }
            int i = 0;
            while(i < inp.Length && inp[i] != ' ')
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
                res.args = inp.Substring(i + 1, inp.Length-i-1);
            }
            return res; 
        }
        static private bool checkNoArgs(string cmd)
        {
            switch(cmd)
            {
                case "/start": return true;
                case "/1": return true;
                default: return false;
            }
        }
        static public void readCommand(string command, Table table)
        {
            Command inp = getCommandFromString(command);
            if (inp.args == null && !checkNoArgs(inp.cmd))
                help(inp.cmd);
            else
                switch (inp.cmd)
                {
                    case "/add":
                        table.addUser(new User(inp.args));
                        break;
                    case "/start":
                        new Game(table);
                        break;
                    case "/1":
                        table.addUser(new User("Олег"));
                        table.addUser(new User("Владимир"));
                        table.addUser(new User("Павел"));
                        table.addUser(new User("Пётр"));
                        table.addUser(new User("Валерий"));
                        break;

                    default:
                        Console.WriteLine("Не могу прочитать команду" + inp.cmd);
                        break;
                }
        }
        static void mainParametrs()
        {
            Console.Title = "Poker";
            Table table = new Table();
            string inp;
            while (true)
            {
                inp = Console.ReadLine();
                readCommand(inp, table);

                table.printAllInfo();
            }
            
            
        }
        static void Main(string[] args)
        {
            mainParametrs();
            
            Console.ReadKey();
        }
    }
}
