namespace CustomerWebsite.Models.Types;

public static class AccountType
{
    public const string Checking = "C";
    public const string Saving = "S";
}

public static class TransactionType
{
    public const string Deposit = "D";
    public const string Withdraw = "W";
    public const string Transfer = "T";
    public const string ServiceCharge = "S";
    public const string BillPay = "B";
}

public static class StateType
{
    public const string NSW = "NSW";
    public const string VIC = "VIC";
    public const string QLD = "QLD";
    public const string SA = "SA";
    public const string WA = "WA";
    public const string TAS = "TAS";
    public const string NT = "NT";
    public const string ACT = "ACT";
}