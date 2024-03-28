namespace Application.Common.Helpers
{
    public static class LinqUtils
    {
        public static bool AnyDuplicatedKeys<TItem, TKey>(this IEnumerable<TItem> items, Func<TItem, TKey> keySelector)
        {
            return items.GroupBy(keySelector).Any(group => group.Count() > 1);
        }
    }
}
