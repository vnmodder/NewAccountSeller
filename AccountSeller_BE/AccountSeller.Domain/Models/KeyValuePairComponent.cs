namespace AccountSeller.Domain.Models
{
    public class KeyValuePairComponent<TValue>
    {
        public string? Key { get; set; }

        public TValue? Value { get; set; }
    }
}
