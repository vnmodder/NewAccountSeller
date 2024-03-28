namespace AccountSeller.Application.Common.Models
{
    /// <summary>
    /// Interface for implementation of Builder design pattern
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface IObjectBuilder<TObject> where TObject : class
    {
        public TObject Build();
    }
}
