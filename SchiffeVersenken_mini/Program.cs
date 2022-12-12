using System;
using System.Linq;

namespace SchiffeVersenken_mini
{
    internal class Program
    {
        static int shot;
        static string[] pField = new string[16];

        static void DrawBoard()
        {
            for (int i = 1; i < 17; i++)
            {
                if (pField[i - 1] == "~")
                    Console.ForegroundColor = ConsoleColor.DarkCyan; //blue water
                else if (pField[i - 1] == "X")
                    Console.ForegroundColor = ConsoleColor.Red; //red hit mark
                else
                    Console.ForegroundColor = ConsoleColor.Gray; //gray numbers

                if (pField[i - 1] == "0" || pField[i - 1] == "x")
                    Console.Write(i + "\t");
                else
                    Console.Write(pField[i - 1] + "\t");

                if (i % 4 == 0)
                    Console.WriteLine("\n");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static bool CheckShot()
        {
            shot = int.TryParse(Console.ReadLine(), out shot) ? shot : 0;
            if (shot <= 0 || shot > 16)
            {
                Console.WriteLine("Spiel beendet.");
                return false;
            }
            else
                return true;
        }

        static void Main(string[] args)
        {
            int counter = 1;
            bool failShot = false;
            int endGame = 0;

            for (int i = 0; i < 16; i++)
                pField[i] = "0";

            Console.WriteLine("Willkommen bei Schiffe versenken!\n" +
                "Versuchen Sie ein 2x1 großes Schiff zu versenken!");
            DrawBoard();
            Console.WriteLine("Wollen Sie das Schiff manuell platzieren oder soll es zufällig gesetzt werden? (m = manuell)");
            if (Console.ReadLine() != "m")
                PlaceShipAtRandom();
            else
            {
                Console.WriteLine("Wo wollen Sie den ersten Teil des Schiffes setzen? ");
                if (!CheckShot())
                    return;
                shot--;
                pField[shot] = "x";
                Console.WriteLine("Wo wollen Sie den zweiten Teil des Schiffes setzen? ");
                while (true)
                {
                    if (!CheckShot())
                        return;
                    shot--;
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    Console.Write("   \r");
                    if (pField[shot] != "x")
                        break;
                }
                pField[shot] = "x";

            }
            Console.Clear();
            do
            {
                Console.Clear();
                DrawBoard();

                Console.WriteLine("Versuch: " + counter);
                if (failShot)
                    Console.WriteLine("Feld bereits abgeschossen!");
                failShot = false;
                Console.WriteLine("Auf welches Feld wollen Sie schießen? (0 = Spiel beenden)");
                if (!CheckShot())
                    return;
                shot--; //input = array index
                if (pField[shot] == "0") //miss
                {
                    pField[shot] = "~"; //mark missed field
                    RocketAnim();
                }
                else if (pField[shot] == "x") //hit
                {
                    pField[shot] = "X"; //mark hit field
                    RocketAnim();
                }
                else if (pField[shot] == "X" || pField[shot] == "~") //already hit
                {
                    failShot = true;
                    continue;
                }
                counter++;
                endGame = pField.Where(field => field == "X").Count();
            } while (endGame < 2);

            Console.Clear();
            DrawBoard();
            Console.WriteLine("Glückwunsch. Sie haben gewonnen!\n" +
                "Anzahl Versuche: " + --counter);
        }

        static void RocketAnim()
        {
            int tempCursX = Console.CursorLeft;
            int tempCursY = Console.CursorTop;
            int buff = 0;
            Console.CursorVisible = false; //removes blinking cursor during animation
            for (int i = 0; i < 22; i++)
            {
                Console.SetCursorPosition(70 + buff, 3);
                if (Console.CursorLeft > 20)
                    buff -= 2;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(" _____/");
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                Console.SetCursorPosition(71 + buff, 4);
                Console.Write("<|_____|");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(">>>>>");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                Console.SetCursorPosition(72 + buff, 5);
                Console.Write("      \\");
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                System.Threading.Thread.Sleep(35);
            }
            Console.SetCursorPosition(tempCursX, tempCursY);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            DrawBoard();
            Console.CursorVisible = true;
        }

        static void PlaceShipAtRandom()
        {
            Random rnd = new Random();
            int ship1 = rnd.Next(1, 17), ship2;
            while (true)
            {
                ship2 = rnd.Next(1, 17);
                if (((ship1 - 1) % 4 == 0 && ship2 == ship1 - 1) || (ship1 % 4 == 0 && ship2 == ship1 + 1))//checks first and last column
                    continue;
                else if (Math.Abs(ship1 - ship2) != 1 && Math.Abs(ship1 - ship2) != 4) //checks inner fields
                    continue;
                else
                    break;
            }
            pField[ship1 - 1] = "x";
            pField[ship2 - 1] = "x";
        }
    }
}