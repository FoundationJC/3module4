using ClassLibrary1;

namespace ConsoleApp3
{
    // Делегат з потрібним підписом
    delegate int[] DiagonalDelegate(int[,] matrix);

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;


            // ---------- Завдання 1: анонімний метод і лямбда ----------
            int[,] matrix =
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };

            // Анонімний метод
            DiagonalDelegate anon = delegate (int[,] m)
            {
                int n = Math.Min(m.GetLength(0), m.GetLength(1));
                int[] result = new int[n];
                for (int i = 0; i < n; i++)
                    result[i] = m[i, i];
                return result;
            };

            // Лямбда-вираз
            DiagonalDelegate lambda = m =>
            {
                int n = Math.Min(m.GetLength(0), m.GetLength(1));
                int[] result = new int[n];
                for (int i = 0; i < n; i++)
                    result[i] = m[i, i];
                return result;
            };

            Console.WriteLine("Анонімний метод: " + string.Join(", ", anon(matrix)));
            Console.WriteLine("Лямбда-вираз:    " + string.Join(", ", lambda(matrix)));
            Console.WriteLine();

            // ---------- Завдання 2–3: подія ----------
            MobileCommunications phone = new MobileCommunications(startBalance: 100);

            phone.BalanceLow += Phone_BalanceLow;   // підписка

            phone.ActivatePackage("Інтернет+", 30);
            Console.WriteLine(phone.GetStatus());

            phone.Talk(20);                          // -40 грн
            Console.WriteLine(phone.GetStatus());

            phone.Talk(10);                          // -20 грн -> баланс 10 -> подія
            Console.WriteLine(phone.GetStatus());

            phone.BalanceLow -= Phone_BalanceLow;   // скасування підписки
            phone.TopUp(50);
            phone.Talk(25);                          // подія вже не обробляється
            Console.WriteLine(phone.GetStatus());

            Console.ReadKey();
        }

        // Обробник події зі стандартною сигнатурою
        static void Phone_BalanceLow(object? sender, BalanceEventArgs e)
        {
            Console.WriteLine($"!!! Увага: баланс {e.Balance} грн (мінімум {e.MinBalance} грн). Поповніть рахунок.");
        }
    }
}