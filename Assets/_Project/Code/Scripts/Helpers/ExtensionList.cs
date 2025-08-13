using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NamPhuThuy
{
    public static class ExtensionList
    {
        #region Element Access Methods
        
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
        
        public static void Add<T>(this List<T> list, List<T> additionList)
        {
            foreach (var item in additionList)
            {
                list.Add(item);
            }
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
        #endregion
        
        

        

        #region Randomization Methods
        
        public static int RandomIndex<T>(this List<T> list)
        {
            return Random.Range(0, list.Count);
        }
        
        public static List<int> RandomIndices<T>(this List<T> list, int count, bool allowRepeat = false)
        {
            List<int> indexList = new List<int>();
            HashSet<int> usedIndices = new HashSet<int>();

            for (int i = 0; i < count; i++)
            {
                if (allowRepeat)
                {
                    indexList.Add(list.RandomIndex());
                }
                else
                {
                    int randomIndex;
                    do
                    {
                        randomIndex = list.RandomIndex();
                    } while (usedIndices.Contains(randomIndex) && usedIndices.Count < list.Count);

                    if (usedIndices.Count < list.Count)
                    {
                        usedIndices.Add(randomIndex);
                        indexList.Add(randomIndex);
                    }
                    else
                    {
                        break; // Stop if all indices are used
                    }
                }
            }

            return indexList;
        }
        
        public static T PopRandomElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);
            int index = Random.Range(0, list.Count);
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }
        
        /// <summary>
        /// Shuffle the list in place
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                list.Swap(i, randomIndex);
            }
        }
        
        /// <summary>
        /// Clone a shuffled version of input-List
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> CloneMix<T>(this List<T> list)
        {
            List<T> newList = new List<T>();
            var cloneList = list.Clone();
            while (cloneList.Count > 0)
            {
                newList.Add(cloneList.PopRandomElement());
            }
            return newList;
        }
        
        public static T RandomElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);
            return list[list.RandomIndex()];
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

        #endregion
        
        

        

        

        #region Dictionary and Queue Helper

        public static Queue<T> ToQueue<T>(this List<T> list)
        {
            Queue<T> temp = new Queue<T>();
            foreach (var item in list)
            {
                temp.Enqueue(item);
            }
            return temp;
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

        #endregion

        #region List Operations

        /// <summary>
        /// Clone a list with new address in RAM
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Clone<T>(this List<T> list)
        {
            List<T> temp = new List<T>();
            foreach (var item in list)
            {
                temp.Add(item);
            }
            return temp;
        }
        
        public static T GetElementClamp<T>(this List<T> list, int index)
        {
            if (list.Count == 0) return default(T);
            return list[Mathf.Clamp(index, 0, list.Count - 1)];
        }

        public static void Swap<T>(this List<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static bool Contain<T>(this List<T> list, Predicate<T> match)
        {
            foreach (T item in list)
            {
                if (match(item)) return true;
            }
            return false;
        }
        
        /// <summary>
        /// Add all elements of additionList to list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="additionList"></param>
        /// <typeparam name="T"></typeparam>
        
        public static List<T> ValueList<S, T>(this Dictionary<S, T> dict)
        {
            List<T> newList = new List<T>();
            foreach (var item in dict)
            {
                newList.Add(item.Value);
            }
            return newList;
        }
        
        /// <summary>
        /// Get all values of a Queue into a List
        /// </summary>
        /// <param name="dict"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ValueList<T>(this Queue<T> dict)
        {
            List<T> newList = new List<T>();
            foreach (var item in dict)
            {
                newList.Add(item);
            }
            return newList;
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
        #endregion
    }
}
