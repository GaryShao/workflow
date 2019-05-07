namespace SFood.DataAccess.Common.Enums
{
    public enum OrderStatus: byte
    {
        Pending,
        Cooking,
        DeliveringOrTaking,
        Done, 
        Closed
    }
}
