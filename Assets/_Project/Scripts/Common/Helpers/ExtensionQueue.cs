using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy
{
    public static class ExtensionQueue
    {
        public static void AddRange<T>(this Queue<T> queue, Queue<T> addQueue)
        {
            while (addQueue.Count > 0)
            {
                queue.Enqueue(addQueue.Dequeue());
            }
        }
    }
}