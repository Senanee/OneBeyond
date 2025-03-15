namespace OneBeyondApi.Model
{
    public class Response(bool flag, string message)
    {
        public bool Flag { get; set; } = flag;
        public string Message { get; set; } = message;
    }
}
