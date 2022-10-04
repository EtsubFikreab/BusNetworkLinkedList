namespace Bus.Models
{
    public class Node<T>
    {
        //  ? = value can be null
        public T? data;
        public Node<T>? next;
        public Node<T>? previous;
        public Node()
        {
            next = null;
            previous = null;
        }
    }
}