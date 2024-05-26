public class Message
{
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public string Content { get; set; }
    public string Timestamp { get; set; }
    public int ID { get; set; }

    public Message(string sender, string receiver, string content, string timestamp, int id)
    {
        Sender = sender;
        Receiver = receiver;
        Content = content;
        Timestamp = timestamp;
        ID = id;
    }

    public override string ToString()
    {
        return $"{Timestamp} - {Sender} to {Receiver}: {Content}";
    }
}