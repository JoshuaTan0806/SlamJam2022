//#define STATIC_WEIGHT
using System.Collections.Generic;

namespace RandomCollections
{
    [System.Serializable]
    public class Chance
    {
        public double weight = 1;

        private float probability = 0;
    }

    [System.Serializable]
    public class RandomList<T> : List<T> where T : Chance
    {
        private static System.Random s_rand = new();
#if STATIC_WEIGHT
        private double _totalWeight = 0;
        /// <summary>
        /// Gets a random item from the collection
        /// </summary>
        /// <returns></returns>
        public T Random()
        {
            double r = s_rand.NextDouble() % _totalWeight;
            //Search for our random item
            foreach(T item in this)
            {
                if (r > item.weight)
                    r -= item.weight;
                else
                    return item;
            }

            return null;
        }
        new public void Add(T item)
        {
        if (item != null)
            _totalWeight += item.weight;

            base.Add(item);
        }

        new public void AddRange(IEnumerable<T> range)
        {
            foreach (T i in range)
                Add(i);
        }

        new public void Insert(int index, T item)
        {
        if (item != null)
            _totalWeight += item.weight;

            base.Insert(index, item);
        }

        new public void InsertRange(int index, IEnumerable<T> collection)
        {   //Add weight
            foreach (T i in collection)
            if (i != null)
                _totalWeight += i.weight;

            base.InsertRange(index, collection);
        }

        new public bool Remove(T item)
        {
            bool b = base.Remove(item);

            if (b && item != null)
                _totalWeight -= item.weight;

            return b;
        }

        new public void Clear()
        {
            _totalWeight = 0;

            base.Clear();
        }
#else
        /// <summary>
        /// Gets a random item from the collection
        /// </summary>
        /// <returns></returns>
        public T Random()
        {   //Count up
            double total = 0;
            foreach (T i in this)
                if (i != null)
                total += i.weight;
            //Get random value
            double r = s_rand.NextDouble() % total;
            //Find
            foreach (T i in this)
            {
                if (i == null)
                    continue;

                if (r > i.weight)
                    r -= i.weight;
                else
                    return i;
            }
            //Failed
            return null;
        }
#endif

        public static void SetSeed(int seed) => s_rand = new System.Random(seed);
    }

    [System.Serializable]
    public abstract class RandomDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : Chance
    {
        private static System.Random s_rand = new();
        private static List<TKey> _orderedKeys = new List<TKey>();

        public TValue RandomValue()
        {
            TKey random = RandomKey();
            //Make sure its a valid key.
            if (ContainsKey(random))
                return this[random];

            return null;
        }
#if STATIC_WEIGHT

#else
        public TKey RandomKey()
        {
            //Build the ordered list and sort it.
            _orderedKeys.Clear();
            _orderedKeys.AddRange(Keys);
            _orderedKeys.Sort(Compare);
            //Sum up random values
            double total = 0;
            foreach (TValue v in Values)
                if (v != null)
                    total += v.weight;

            double r = s_rand.NextDouble() % total;
            //Find random value
            foreach (TKey k in _orderedKeys)
            {   //Ignore nulls
                if (k == null || this[k] == null)
                    continue;

                TValue v = this[k];
                if (r > v.weight)
                    r -= v.weight;
                else
                    return k;
            }

            return default;
        }
#endif
        protected abstract int Compare(TKey left, TKey right);

        public static void SetSeed(int seed) => s_rand = new System.Random(seed);
    }
}
