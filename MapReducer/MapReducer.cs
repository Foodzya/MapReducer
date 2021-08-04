using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace MapReducer
{
    public static class MapReducer
    {
        public static TAccumulate MapReduce<TElement, TResult, TKey, TAccumulate>(this IEnumerable<TElement> source, TAccumulate seed, Func<TElement, TResult> selector, Func<TResult, TKey> keySelector, Func<TAccumulate, TResult, TAccumulate> reducer)
        {
            IEnumerable<TResult> afterSelect = CustomSelect(source, selector);
            IEnumerable<IGrouping<TKey, TResult>> afterGroupBy = CustomGroupBy(afterSelect, keySelector);
            return CustomReduce(afterGroupBy, seed, reducer);         
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


        public static TAccumulate CustomReduce<TKey, TResult, TAccumulate>(IEnumerable<IGrouping<TKey, TResult>> source, TAccumulate seed, Func<TAccumulate, TResult, TAccumulate> reducer)
        {
            TAccumulate result = seed;
            foreach (IGrouping<TKey, TResult> key in source)
            {
                foreach (TResult value in key)
                {
                    result = reducer(result, value);
                }      
            }
            return result;
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
