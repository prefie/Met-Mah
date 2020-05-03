using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MetMah.Additionally
{
    public class SinglyLinkedList<T> : IEnumerable<T>
    {
        public readonly T Value;
        public readonly SinglyLinkedList<T> Previous;
        public readonly int Length;

        public SinglyLinkedList(T value, SinglyLinkedList<T> previous = null)
        {
            Value = value;
            Previous = previous;
            Length = previous?.Length + 1 ?? 1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield return Value;
            var pathItem = Previous;
            while (pathItem != null)
            {
                yield return pathItem.Value;
                pathItem = pathItem.Previous;
            }
        }

        public SinglyLinkedList<T> Reverse()
        {
            var answer = new SinglyLinkedList<T>(Value);
            var next = Previous;
            while (next != null)
            {
                answer = new SinglyLinkedList<T>(next.Value, answer);
                next = next.Previous;
            }
            return answer;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
