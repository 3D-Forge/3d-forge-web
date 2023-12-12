namespace Backend3DForge.Enums
{
    public enum OrderStatusType
    {
        New = 0x2,
        Paid = 0x4,
        Processed = 0x8,
        Printing = 0x10,
        Printed = 0x20,
        Sent = 0x40,
        Received = 0x80
    }
}
