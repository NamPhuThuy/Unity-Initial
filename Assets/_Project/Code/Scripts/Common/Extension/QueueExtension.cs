using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy.Common
{
    public static class QueueExtension
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