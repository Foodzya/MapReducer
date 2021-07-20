using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace MapReducer
{
    public static class MapReducer
    {
        public static void MapReduce<TElement, TResult, TKey>(this IEnumerable<TElement> source, Func<TElement, TResult> selector, Func<TResult, TKey> keySelector, Func<TResult, TResult, TResult> reducer)
        {
            IEnumerable<TResult> afterSelect = CustomSelect(source, selector);
            IEnumerable<IGrouping<TKey, TResult>> afterGroupBy = CustomGroupBy(afterSelect, keySelector);
            CustomReduce(afterGroupBy, reducer);
        }

        public static IEnumerable<TResult> CustomSelect<T, TResult>(IEnumerable<T> source, Func<T, TResult> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        public static IEnumerable<IGrouping<TKey, TElement>> CustomGroupBy<TKey, TElement>(IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
        {
            Dictionary<TKey, List<TElement>> dict = new Dictionary<TKey, List<TElement>>();

            foreach (TElement x in source)
            {
                TKey key = keySelector(x);
                if (dict.Keys.Contains(key))
                {
                    dict[key].Add(x);
                }
                else
                {
                    dict.Add(key, new List<TElement> { x });
                }
            }

            return dict.Select(x => new Grouping<TKey, TElement>(x.Key, x.Value));
        }


        public static void CustomReduce<TKey, T>(IEnumerable<IGrouping<TKey, T>> source, Func<T, T, T> reducer)
        {
            T result = default(T);
            foreach (var key in source)
            {
                //Console.WriteLine(key.Key);
                foreach (var value in key)
                {
                    IEnumerator<T> enumerator = key.GetEnumerator();
                    result = enumerator.Current;
                    while (enumerator.MoveNext())
                        result = reducer(result, enumerator.Current);
                }
                Console.WriteLine(result);
            }

            //IEnumerator<T> enumerator = source.GetEnumerator();
            //T result = enumerator.Current;
            //while (enumerator.MoveNext())
            //{
            //	result = reducer(result, enumerator.Current);
            //}
        }
    }

    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private TKey _key;
        private IEnumerable<TElement> _elements;

        public Grouping(TKey key, IEnumerable<TElement> elements)
        {
            _key = key;
            _elements = elements;
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TKey Key
        {
            get { return _key; }
        }
    }
}
