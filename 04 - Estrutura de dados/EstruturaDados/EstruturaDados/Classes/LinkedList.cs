namespace EstruturaDados.Classes;

public class LinkedList
{
    private class Node
    {
        public object Value { get; set; }
        public Node? NodeNext { get; set; }

        public Node() { }
        public Node(object value)
        {
            this.Value = value;
        }
    }

    private Node _head;
    private int _size;

    public LinkedList()
    {
        _head = null;
        _size = 0;
    }

    public void Print()
    {
        Node node = _head;
        string result="";
        while (node != null)
        {
            result += node.Value +", ";
            node = node.NodeNext;
        }
        Console.WriteLine(result);
    }

    public int Size()
    {
        return _size;
    }
    
    public void Push(object value)
    {
        Node node = new Node(value);
        node.NodeNext = _head;
        _head = node;
        _size++;
    }

    public void Append(object value)
    {
        Node node = new Node(value);
        Node lastNode = ReturnLast();
        if (lastNode != null)
        {
            lastNode.NodeNext = node;
        }
        else
        {
            node.NodeNext = _head;
            _head = node;
        }
        _size++;
    }
    
    public void InsertAfter(object position, object value)
    {
        Node node = new Node(value);
        Node prevNode = Find(position);
        if (prevNode == null)
        {
            Console.WriteLine("Position not found");
            return ;
        }
        if (prevNode.NodeNext != null)
        {
            node.NodeNext = prevNode.NodeNext;
        }
        prevNode.NodeNext = node;
        _size++;
    }

    public bool Contais(object value)
    {
        Node node = Find(value);
        return node != null;
    }

    public object Pop()
    {
        Node node = _head;
        _head = node.NodeNext;
        _size--;
        return node.Value;
    }

    public object PopLast()
    {
        Node node = ReturnPenultimate();
        var result = node.NodeNext.Value;
        node.NodeNext = null;
        _size--;
        return result;
    }

    public void Remove(object value)
    {
        Node pre = new Node();
        Node node = Find(value, ref pre);
        if (node == null)
        {
            Console.WriteLine("Value not found");
            return ;
        }
        if (node==pre)
        {
            _head = node.NodeNext;
            _size--;
            return;
        }
        pre.NodeNext = node.NodeNext;
        _size--;
    }

    public void Clear()
    {
        _head = null;
        _size = 0;
    }

    public object? IsEmpty()
    {
        return _head == null;
    }

    private Node Find(object value)
    {
        Node node = _head;
        if (node == null) return null;
        while (node != null)
        {
            if (Equals(node.Value, value))
            {
                return node;
            }
            node = node.NodeNext;
        }

        return null;
    }

    private Node Find(object value, ref Node predecessor)
    {
        Node node = _head;
        if (node == null) return null;
        Node pre = node;
        while (node != null && !Equals(node.Value, value))
        {
            pre = node;
            node = node.NodeNext;
        }

        if (node != null)
        {
            predecessor = pre;
            return node;
        }

        return null;
    }

    private Node ReturnLast()
    {
        Node node = _head;
        if (node == null) return null;
        while (node.NodeNext != null) node = node.NodeNext;
        return node;
    }

    private Node ReturnPenultimate()
    {
        Node node = _head;
        if (node == null) return null;
        while (node.NodeNext.NodeNext != null) node = node.NodeNext;
        return node;
    }
}