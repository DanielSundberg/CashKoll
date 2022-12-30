public class AccountLine
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int Account { get; set; }
    public int Currency { get; set; }
    public long Amount { get; set; }
    public long Balance { get; set; }
    public string Description { get; set; } = "";
    public int Category { get; set; }
    public string CategoryName { get; set; } = "";

    public bool CategoryMapFound { get; set; }
    public string AmountAsString() {
        return $"{(Amount / 1000.0):0.00}";
    }

    public void UpdateAmount(string newAmount) 
    {
        if (!string.IsNullOrEmpty(newAmount)) {
            Amount = Convert.ToInt64(Double.Parse(newAmount) * 1000);
        }
    }

    public string BalanceAsString() {
        return $"{(Balance / 1000.0):0.00}";
    }

    public void UpdateBalance(string newAmount) 
    {
        if (!string.IsNullOrEmpty(newAmount)) {
            Balance = Convert.ToInt64(Double.Parse(newAmount) * 1000);
        }
    }


    public string CategoryAsString(IEnumerable<Category> categories) 
    {
        return categories.SingleOrDefault(c => c.Id == Category)?.Name;
    }

    public override string ToString()
    {
        return @$"
            Id:          {Id}
            Timestamp:   {Timestamp}
            Account:     {Account}
            Currency:    {Currency}
            Amount:      {Amount}
            Balance:     {Balance}
            Description: {Description}
            Category:    {Category}
        ";
    }
}