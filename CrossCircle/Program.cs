using System;

namespace CrossCircle
{
    class Program
    {
        public static char[,] field = new char[3, 3]
        {
               {'0','1','2'},
               {'3','4','5'},
               {'6','7','8'}
        };
        public static void Main()
        {
            Console.Title = "Хрестики-нулики";
            Console.SetWindowSize(60, 30);
            Console.BufferHeight = 30;
            Console.BufferWidth = 60;
            bool gameon = true;
            while (gameon) 
            {
                try
                {
                    int level;
                    do
                    {
                        Console.Clear();
                        Console.SetCursorPosition(5, 0);
                        Console.WriteLine("Вітаю вас у грі хрестики-нулики, оберіть складність:\n1 - Легко\t2 - Складно");
                        Console.ForegroundColor = ConsoleColor.Green;
                        level = int.Parse(Console.ReadLine());
                        Console.ResetColor();
                    } while (level != 1 && level != 2);
                    int queue;
                    do
                    {
                        Console.Clear();
                        Console.SetCursorPosition(5, 0);
                        Console.WriteLine("Оберіть яким ви хочете ходити (хрестики - 1, нулі - 2)\n1 - Хрестики\t2 - Нулі");
                        Console.ForegroundColor = ConsoleColor.Green;
                        queue = int.Parse(Console.ReadLine());
                        Console.ResetColor();
                        Console.Clear();
                    } while (queue != 1 && queue != 2);
                    bool playermove = true;
                    switch (queue)
                    {
                        case 1:
                            playermove = true;
                            break;
                        case 2:
                            playermove = false;
                            break;
                    } 
                    CreateField();
                    for (int i = 0; i < 9;) // gameplay
                    {
                        if (playermove)
                        {
                            PlayerMove(queue);
                            CreateField();
                            playermove = false;
                            Console.Clear();
                        }
                        else if (!playermove)
                        {
                            BotMove(queue, level);
                            CreateField();
                            playermove = true;
                        }
                        if (WinChecker(field, queue) == 1) // перевірка на перемогу
                        {
                            WinDrawLose(ref gameon, 1);
                            break;
                        }
                        else if (WinChecker(field, queue) == 0 && IsFieldFull(field) == true) // перевірка на нічию без перемоги
                        {
                            WinDrawLose(ref gameon, 0);
                            break;
                        }
                        else if (WinChecker(field, queue) == -1) // перевірка на програш
                        {
                            WinDrawLose(ref gameon, -1);
                            break;
                        }
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некоректний ввід");
                    Console.ResetColor();
                }
                catch (OverflowException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некоректний ввід");
                    Console.ResetColor();
                }
            }
        } 
        public static char[,] CreateField()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("┌─────┬─────┬─────┐");
            for (int i = 0; i < 3; i++)
            {
                Console.Write("│");
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($"{field[i, j],3}  |");
                }
                Console.WriteLine();
                if (i < 2)
                {
                    Console.WriteLine("├─────┼─────┼─────┤");
                }
            }
            Console.WriteLine("└─────┴─────┴─────┘");
            Console.ResetColor();
            return field;
        }
        public static void PlayerMove(int queue)
        {
            char playersymbol; int xcoord; int ycoord;
            if (queue == 1)
                playersymbol = 'X';
            else if (queue == 2)
                playersymbol = 'O';
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Некоректний ввід");
                Console.ResetColor();
                return;
            }
            do
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Оберіть в яку комірку поставити {playersymbol} (0-8) ");
                Console.ResetColor();
                int filler = int.Parse(Console.ReadLine());
                if (filler < 0 || filler > 8)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введіть правильний номер клітинки");
                    Console.ResetColor();
                }
                else
                {
                    xcoord = filler / 3;
                    ycoord = filler % 3;
                    if (field[xcoord, ycoord] == 'X' || field[xcoord, ycoord] == 'O')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ця клітинка вже зайнята");
                        Console.ResetColor();
                    } 
                    else
                        break;
                }
            } while (true);
            field[xcoord, ycoord] = playersymbol;
        }
        public static void BotMove(int queue, int level)
        {
            char botsymbol = ' '; int xcoord; int ycoord;
            if (queue == 1)
                botsymbol = 'O';
            else if (queue == 2)
                botsymbol = 'X';
            if (level == 1) // LEVEL 1 - RANDOM
            {
                if (IsFieldFull(field))
                {
                    return;
                }
                Random random = new Random();
                do
                {
                    xcoord = random.Next(0, 3);
                    ycoord = random.Next(0, 3);
                    if (field[xcoord, ycoord] != 'X' && field[xcoord, ycoord] != 'O')
                        break;
                } while (true);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Бот обрав комірку - {field[xcoord,ycoord].ToString()}");
                Console.ResetColor();
                field[xcoord, ycoord] = botsymbol;
            }
            if (level == 2) // LEVEL 2 - HARDCORE
            {
                int bestvalue = int.MinValue; int bestx = -1; int besty = -1; 
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (field[i, j] != 'X' && field[i, j] != 'O')
                        {
                            char originalvalue = field[i, j]; //зберегти попередній зміст комірки
                            field[i, j] = botsymbol; // зробити хід 
                            int movevalue = Minimax(field, false, queue);// порахувати
                            field[i, j] = originalvalue;// повернути оригінальний вміст
                            if (movevalue > bestvalue)//якщо хід ліпше то перезаписати значення
                            {
                                bestvalue = movevalue;
                                bestx = i;
                                besty = j;
                            }
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Бот обрав комірку - {field[bestx, besty].ToString()}");
                Console.ResetColor();
                field[bestx, besty] = botsymbol;    
            }
        }
        public static int WinChecker(char[,] field, int queue)
        {
            char playersymbol = ' '; char botsymbol = ' ';
            if (queue == 1)
            {
                playersymbol = 'X';
                botsymbol = 'O';
            }    
            else if (queue == 2)
            {
                playersymbol = 'O';
                botsymbol = 'X';
            }
            for (int i = 0; i < 3; i++)
            {
                if ((field[i, 0] == playersymbol && field[i, 1] == playersymbol && field[i, 2] == playersymbol) || //рядки і стовпці
                    (field[0, i] == playersymbol && field[1, i] == playersymbol && field[2, i] == playersymbol))
                {
                    return 1;
                }
            }
            if ((field[0, 0] == playersymbol && field[1, 1] == playersymbol && field[2, 2] == playersymbol) || //діагоналі
                (field[0, 2] == playersymbol && field[1, 1] == playersymbol && field[2, 0] == playersymbol))
            {
                return 1;
            }
            for (int i = 0; i < 3; i++)
            {
                if ((field[i, 0] == botsymbol && field[i, 1] == botsymbol && field[i, 2] == botsymbol) || //рядки і стовпці
                    (field[0, i] == botsymbol && field[1, i] == botsymbol && field[2, i] == botsymbol))
                {
                    return -1;
                }
            }
            if ((field[0, 0] == botsymbol && field[1, 1] == botsymbol && field[2, 2] == botsymbol) || //діагоналі
                (field[0, 2] == botsymbol && field[1, 1] == botsymbol && field[2, 0] == botsymbol))
            {
                return -1;
            }
            return 0;
        }
        public static bool IsFieldFull(char[,] field)
        {
            int emptycells = 0;
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j] != 'X' && field[i, j] != 'O')
                    {
                        emptycells++;
                    }
                }
            }
            return emptycells == 0;
        }
        public static void FieldCleaner(char[,] field)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    field[i, j] = (char)(i * 3 + j + '0');
                }
            }
        }
        public static bool Gamekeeper()
        {
            Console.WriteLine("Гра завершилася!\nБажаєте розпочати заново чи завершити програму?\n1 - Заново\n2 - Завершити");
            int gamekeep = int.Parse(Console.ReadLine());
            return gamekeep == 1;
        }
        public static int Minimax(char[,] field, bool ismax, int queue)
        {
            char playersymbol = ' '; char botsymbol = ' ';
            if (queue == 1)
            {
                playersymbol = 'X';
                botsymbol = 'O';
            }
            else if (queue == 2)
            {
                playersymbol = 'O';
                botsymbol = 'X';
            }
            if (WinChecker(field, queue) == 1 || WinChecker(field, queue) == -1)
            {
                if (ismax)
                    return -1; // Програш для максимізуючого гравця
                else
                    return 1; // Перемога для максимізуючого гравця
            }
            else if (IsFieldFull(field))
            {
                return 0; // Нічия
            } 
            if (ismax)
            {
                int best = int.MinValue;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (field[i, j] != 'X' && field[i, j] != 'O')
                        {
                            field[i, j] = botsymbol;
                            int value = Minimax(field, false, queue); // рекурсія
                            field[i, j] = Convert.ToChar(i * 3 + j + '0'); 
                            best = Math.Max(value, best);
                            //Console.WriteLine(best);
                        }
                    }
                }
                return best;
            }
            else
            {
                int best = int.MaxValue;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (field[i, j] != 'X' && field[i, j] != 'O')
                        {
                            field[i, j] = playersymbol;
                            int value = Minimax(field, true, queue);
                            field[i, j] = Convert.ToChar(i * 3 + j + '0'); 
                            best = Math.Min(value, best);
                            //Console.WriteLine(best);
                        }
                    }
                }
                return best;
            }
        }
        public static void WinDrawLose(ref bool gameon, int result)
        {
            switch (result)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Ви перемогли!");
                    break;
                case 0:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Нічия!");
                    break;
                case -1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ви програли!");
                    break;
            }
            Console.ResetColor();
            if (Gamekeeper())
            {
                FieldCleaner(field);
                Console.Clear();
            }
            else
            {
                gameon = false;
            }
        }
    }
}