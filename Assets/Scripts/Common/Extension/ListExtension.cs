using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NamPhuThuy.Common
{
    
    public static class ListExtensions
    {
        /// <summary>
        /// Shuffles the list in place using the Fisher–Yates algorithm.
        /// </summary>
        public static void FisherYatesShuffle<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>
        /// Returns a new shuffled copy of the list, leaving the original untouched.
        /// </summary>
        public static List<T> FisherYatesShuffled<T>(this IList<T> list)
        {
            var copy = new List<T>(list);
            copy.FisherYatesShuffle();
            return copy;
        }
        
        public static List<T> Clone<T>(this List<T> list)
        {
            List<T> temp = new List<T>();
            foreach (var item in list)
            {
                temp.Add(item);
            }
            return temp;
        }


        public static T RandomElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);
            return list[Random.Range(0, list.Count)];
        }
        public static T GetElementClamp<T>(this List<T> list, int index)
        {
            if (list.Count == 0) return default(T);
            return list[Mathf.Clamp(index, 0, list.Count - 1)];
        }

        public static int LastIndex<T>(this List<T> list)
        {
            return list.Count - 1;
        }
        public static T LastElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);
            return list[list.Count - 1];
        }
        public static T FirstElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);
            return list[0];
        }
        public static T PopLastElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);
            T result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return result;
        }
        public static T PopFirstElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);
            T result = list[0];
            list.RemoveAt(0);
            return result;
        }
        public static int RandomIndex<T>(this List<T> list)
        {
            return Random.Range(0, list.Count);
        }
        public static T PopRandomElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);
            int index = Random.Range(0, list.Count);
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }

        public static void Swap<T>(this List<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static T Remove<T>(this List<T> list, Predicate<T> match)
        {
            int index = list.FindIndex(match);
            if (index != -1)
            {
                T result = list[index];
                list.RemoveAt(index);
                return result;
            }
            return default(T);
        }

        public static bool Contain<T>(this List<T> list, Predicate<T> match)
        {
            foreach (T item in list)
            {
                if (match(item)) return true;
            }
            return false;
        }
        public static List<T> RandomElements<T>(this List<T> list, int count, bool allowRepeat = false)
        {
            List<T> cloner = list.Clone();
            List<T> randList = new List<T>();
            if (cloner.Count == 0) return randList;
            while (count > 0)
            {
                count--;
                var randElement = cloner.PopRandomElement();
                randList.Add(randElement);
                if (cloner.Count == 0)
                {
                    if (allowRepeat)
                    {
                        cloner = list.Clone();

                    }
                    else
                    {
                        break;
                    }
                }
            }
            return randList;
        }

        public static List<T> Mix<T>(this List<T> list)
        {
            List<T> newList = new List<T>();
            var cloneList = list.Clone();
            while (cloneList.Count > 0)
            {
                newList.Add(cloneList.PopRandomElement());
            }
            return newList;
        }

        public static void Add<T>(this List<T> list, List<T> additionList)
        {
            foreach (var item in additionList)
            {
                list.Add(item);
            }
        }
        public static List<T> ValueList<S, T>(this Dictionary<S, T> dict)
        {
            List<T> newList = new List<T>();
            foreach (var item in dict)
            {
                newList.Add(item.Value);
            }
            return newList;
        }
        public static List<T> ValueList<T>(this Queue<T> dict)
        {
            List<T> newList = new List<T>();
            foreach (var item in dict)
            {
                newList.Add(item);
            }
            return newList;
        }

        public static HashSet<T> KeyHashset<T, S>(this Dictionary<T, S> dict)
        {
            HashSet<T> result = new HashSet<T>();
            foreach (var item in dict)
            {
                result.Add(item.Key);
            }
            return result;
        }

        public static Queue<T> ToQueue<T>(this List<T> list)
        {
            Queue<T> temp = new Queue<T>();
            foreach (var item in list)
            {
                temp.Enqueue(item);
            }
            return temp;
        }

        public static List<T> ReverseList<T>(this List<T> list)
        {
            List<T> clone = list.Clone();
            clone.Reverse();
            return clone;
        }

        public static IEnumerable<T> ReverseEnumerable<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                yield return list[i];
            }
        }
    }
}