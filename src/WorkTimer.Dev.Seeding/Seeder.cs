using System;
using System.Collections.Generic;

namespace WorkTimer.Dev.Seeding
{
    public abstract class Seeder<T>
    {
        protected IList<T> _list = new List<T>();
        protected static Random _random = new Random(3297593);

        public abstract IList<T> Seed();
    }
}
