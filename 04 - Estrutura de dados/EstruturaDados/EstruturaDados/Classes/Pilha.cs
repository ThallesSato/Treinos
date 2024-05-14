using System.Collections;

namespace EstruturaDados.Classes;

public class Pilha
{
    private object?[] _itens;
    private int _size;
    private readonly int _max;

    public Pilha()
    {
        _itens = Array.Empty<object>();
        _size = 0;
        _max = 0;
    }
    public Pilha(int max)
    {
        _itens = new object[max];
        _size = 0;
        this._max = max;
    }

    public int Size()
    {
        return _size;
    }

    public bool Push(object obj)
    {
        if (_max != 0 && _size == _max) return false;
        if (_max == 0 && _itens.Length == _size)
        {
            Resize();
        }
        
        _itens[_size] = obj;
        _size += 1;
        return true;
    }

    public object? Pop()
    {
        if (_size == 0) return null;
        
        var result = _itens[_size - 1];

        _itens[_size - 1] = null;
        _size -= 1;
        return result;
    }

    public object? Peek()
    {
        return _itens[_size - 1];
    }

    public object? IsEmpty()
    {
        return _size == 0;
    }

    public void Clear()
    {
        _itens = Array.Empty<object>();
        _size = 0;
    }
    public string ToString()
    {
        string result = "";

        for (int i = 0; i < _size; i++)
        {
            result = result + _itens[i] + ", ";
        }

        return result;
    }

    private void Resize()
    {
        Array.Resize(ref _itens, _size+5);
    }

}