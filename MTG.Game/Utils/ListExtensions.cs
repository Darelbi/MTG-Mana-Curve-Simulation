namespace MTG.Game.Utils
{
    public static class ListExtensions
    {
        public static List<T> RemoveWithPredicate<T>(this List<T> target, Func<T, bool> predicate)
        {
            var selectd = target.Where( predicate).ToList();
            target.RemoveAll(x => predicate (x));
            return selectd;
        }
    }
}
