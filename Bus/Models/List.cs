using System;
namespace Bus.Models
{
    public class List<T>
    {
        //implement getFront() and getBack() by using C# builtin automatic properties 
        public Node<T>? front { get; private set; }
        public Node<T>? back { get; private set; }
        public List()
        {
            front = null;
            back = null;
        }
        // ~List() not required because c# has its own garbage collector
        public bool isEmpty()
        {
            return front == null;
        }
        public void insertFirst(T data)
        {
            Node<T> p = new Node<T>();
            p.data = data;
            insertNode(p, null);
        }
        public void insertBack(T data)
        {
            Node<T> p = new Node<T>();
            p.data = data;
            insertNode(p, back);

        }
        public void insertAfter(Node<T> before, T data)
        {
            //when function is called, find is passed as a parameter
            //if data is not found, the data is inserted in the front
            Node<T> p = new Node<T>();
            p.data = data;
            insertNode(p, before);
        }
        public void insertBefore(Node<T>? after, T data)
        {
            // if data is not found, the data is inserted in the back
            Node<T> p = new Node<T>();
            p.data = data;
            if (after != null)
                insertNode(p, after.previous);
            else
                insertNode(p, back);

        }
        public int removeFirst()
        {
            if (!isEmpty())
            {
                deleteNode(front);
                return 0;
            }
            return -1;
        }
        public int removeLast()
        {
            if (!isEmpty())
            {
                deleteNode(back);
                return 0;
            }
            return -1;
        }
        public int removeBefore(Node<T>? before)
        {
            if (before == null)
                return -1;
            else if (!isEmpty() && before.previous != null)
            {
                deleteNode(before.previous);
                return 0;
            }
            return -1;
        }
        public int removeAfter(Node<T>? after)
        {
            if (after == null)
                return -1;
            else if (!isEmpty() && after.next != null)
            {
                deleteNode(after.next);
                return 0;
            }
            return -1;
        }
        public bool remove(Node<T>? p)
        {
            if (p == null)
                return false;
            deleteNode(p);
            return true;
        }
        public Node<T>? find(T data)
        {
            Node<T>? previous = front;
            while (previous != null && !Equals(previous.data.ToString(), data.ToString()))
            {
                //used comparer because == operator is not overloaded
                previous = previous.next;
            }
            return previous;
        }
        public void destroy()
        {
            front = back = null;
            GC.Collect(); // garbage collection
        }
        public void insertRangeBefore(Node<T>? before, List<T> range)
        {
            if (range.isEmpty())
                return;
            else if (front == null)
            {
                front = range.front;
                back = range.back;
            }
            else if (before == null || before == front)
            {
                //insert in the front
                range.back.next = front;
                front.previous = range.back;
                front = range.front;
            }
            else
            {
                range.back.next = before;
                range.front.previous = before.previous;
                before.previous.next = range.front;
                before.previous = range.back;
            }
        }
        public void insertRangeAfter(Node<T>? after, List<T> range)
        {
            if (range.isEmpty())
                return;
            else if (back == null)
            {
                front = range.front;
                back = range.back;
            }
            else if (after == null || after == back)
            {
                //insert in the back
                range.front.previous = back;
                back.next = range.front;
                back = range.back;
            }
            else
            {
                range.front.previous = after;
                range.back.next = after.next;
                after.next.previous = range.back;
                after.next = range.front;
            }
        }
        public void removeRange(Node<T>? rangeFirst, Node<T>? rangeLast)
        {
            if (rangeFirst == null || rangeLast == null)
                return;
            if (rangeFirst == front)
                front = rangeLast.next;
            else
                rangeFirst.previous.next = rangeLast.next;
            if (rangeLast == back)
                back = rangeFirst.previous;
            else
                rangeLast.next.previous = rangeFirst.previous;

            GC.Collect();

        }
        public List<T>? getSublist(Node<T>? rangeFirst, Node<T>? rangeLast)
        {
            if (rangeFirst == null || rangeLast == null)
                return null;
            List<T> nList = new List<T>();
            Node<T> temp = rangeFirst;
            while (temp != rangeLast)
            {
                nList.insertBack(temp.data);
                temp = temp.next;
            }
            nList.insertBack(temp.data);//insert last data
            return nList;
        }
        public void print()
        {
            //for debugging
            Node<T>? temp = front;
            while (temp != null)
            {
                Console.Write(temp.data);
                if (temp.next != null)
                    Console.Write(" -> ");
                temp = temp.next;
            }
        }
        public void deleteNode(Node<T> p)
        {
            if (front == back)
                front = back = null;
            else if (p.previous == null)
            {
                front = p.next;
                p.next.previous = null;
            }
            else
            {
                p.previous.next = p.next;
                if (p == back)
                    back = p.previous;
                else
                    p.next.previous = p.previous;
            }
        }
        void insertNode(Node<T> p, Node<T>? prev)
        {
            if (front == null)
            {
                front = back = p;
            }
            else if (prev == null)
            {
                //insert in front
                p.next = front;
                front.previous = p;
                front = p;
            }
            else
            {
                //Console.WriteLine(prev.data+ " ");
                p.next = prev.next;
                prev.next = p;
                p.previous = prev;
                if (back == prev)
                    back = p;
                else
                    p.next.previous = p;
            }
        }
        Node<T>? insertionSlot(T data)
        {
            Node<T>? slot = front;
            while (slot != null && Equals(slot.data, data))
            {
                //used comparer because == operator is not overloaded
                slot = slot.next;

            }
            return slot;
        }
    }
}