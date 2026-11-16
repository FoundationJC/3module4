namespace ClassLibrary1
{
    // Клас-аргумент події
    public class BalanceEventArgs : EventArgs
    {
        public decimal Balance { get; }
        public decimal MinBalance { get; }

        public BalanceEventArgs(decimal balance, decimal minBalance)
        {
            Balance = balance;
            MinBalance = minBalance;
        }
    }

    // Клас, у якому генерується подія
    public class MobileCommunications
    {
        private decimal balance;
        private readonly decimal minBalance;
        private readonly decimal pricePerMinute;

        public string? ActivePackage { get; private set; }
        public int TalkMinutes { get; private set; }
        public decimal Balance => balance;

        // Подія: рахунок зменшився до мінімуму
        public event EventHandler<BalanceEventArgs>? BalanceLow;

        public MobileCommunications(decimal startBalance, decimal minBalance = 10, decimal pricePerMinute = 2)
        {
            balance = startBalance;
            this.minBalance = minBalance;
            this.pricePerMinute = pricePerMinute;
        }

        public void ActivatePackage(string name, decimal price)
        {
            if (price > balance)
            {
                Console.WriteLine("Недостатньо коштів для активації пакету.");
                return;
            }
            balance -= price;
            ActivePackage = name;
            CheckBalance();
        }

        public void TopUp(decimal amount)
        {
            balance += amount;
        }

        public void Talk(int minutes)
        {
            // розмовляємо лише стільки хвилин, скільки вистачає коштів
            int affordable = (int)(balance / pricePerMinute);
            int real = Math.Min(minutes, affordable);

            balance -= real * pricePerMinute;
            TalkMinutes += real;
            CheckBalance();
        }

        public string GetStatus() =>
            $"Баланс: {balance} грн, пакет: {ActivePackage ?? "немає"}, розмов: {TalkMinutes} хв";

        // Метод, що викликає подію
        protected virtual void OnBalanceLow(BalanceEventArgs e)
        {
            BalanceLow?.Invoke(this, e);
        }

        private void CheckBalance()
        {
            if (balance <= minBalance)
                OnBalanceLow(new BalanceEventArgs(balance, minBalance));
        }
    }
}
