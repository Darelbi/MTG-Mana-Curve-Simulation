namespace MTG.Mana
{
    public static class ManaCostExtensions
    {
        public static int ConvertedManaValue(this Mana mana)
        {
            if (mana == null) return 0;

            int colorless = mana.Colorless ?? 0;
            int red = mana.Red ?? 0;
            int blue = mana.Blue ?? 0;
            int black = mana.Black ?? 0;

            int total = 0;
            foreach (var and in mana.And)
            {
                total += and.ConvertedManaValue();
            }

            if (mana.Or.Any())
                total += mana.Or.Select(x => x.ConvertedManaValue()).Max();

            return colorless + red + blue + black + total;
        }

        public static int TotalValueOnMember(this Mana mana, Func<Mana, int> predicate)
        {
            if (mana == null) return 0;

            int total = predicate(mana);

            foreach (var and in mana.And)
            {
                total += predicate(and);
            }

            if(mana.Or.Any())
                total += mana.Or.Select(x => predicate(x)).Max();

            return total;
        }

        public static int TotalBlueValue(this Mana mana)
        {
            return mana.TotalValueOnMember(x => x.Blue ?? 0);
        }
        public static int TotalRedValue(this Mana mana)
        {
            return mana.TotalValueOnMember(x => x.Red ?? 0);
        }
        public static int TotalBlackValue(this Mana mana)
        {
            return mana.TotalValueOnMember(x => x.Black ?? 0);
        }
        public static int TotalColorlessValue(this Mana mana)
        {
            return mana.TotalValueOnMember(x => x.Colorless ?? 0);
        }

        public static int ConvertedManaValueWithAffinity(this Mana mana, int artifacts)
        {
            int colorless = mana.Colorless ?? 0;
            colorless -= artifacts;
            if (colorless < 0)
                colorless = 0;

            int red = mana.Red ?? 0;
            int blue = mana.Blue ?? 0;
            int black = mana.Black ?? 0;

            int total = 0;
            foreach (var and in mana.And)
            {
                total += and.ConvertedManaValueWithAffinity(artifacts);
            }

            if (mana.Or.Any())
                total += mana.Or.Select(x => x.ConvertedManaValueWithAffinity(artifacts)).Max();

            return colorless + red + blue + black + total;
        }

        public static int ManaComplexity(this Mana mana)
        {
            return mana.Or.Count;
        }

        public static bool HaveBlue(this Mana mana)
        {
            bool haveBlue = false;

            foreach (var and in mana.And)
            {
                if (and.HaveBlue())
                    haveBlue = true;
            }

            foreach (var or in mana.Or)
            {
                if (or.HaveBlue())
                    haveBlue = true;
            }

            if (mana.Blue > 0)
            {
                haveBlue = true;
            }

            return haveBlue;
        }

        public static bool HaveBlack(this Mana mana)
        {
            bool haveBlack = false;

            foreach (var and in mana.And)
            {
                if (and.HaveBlack())
                    haveBlack = true;
            }

            foreach (var or in mana.Or)
            {
                if (or.HaveBlack())
                    haveBlack = true;
            }

            if (mana.Black > 0)
            {
                haveBlack = true;
            }

            return haveBlack;
        }

        public static bool HaveRed(this Mana mana)
        {
            bool haveRed = false;

            foreach (var and in mana.And)
            {
                if (and.HaveRed())
                    haveRed = true;
            }

            foreach (var or in mana.Or)
            {
                if (or.HaveRed())
                    haveRed = true;
            }

            if (mana.Red > 0)
            {
                haveRed = true;
            }

            return haveRed;
        }
    }
}
