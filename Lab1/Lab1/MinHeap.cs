namespace Lab1;

internal class MinHeap
{
    private readonly int[] elements;
    private int size;

    public MinHeap(int _size)
    {
        elements = new int[_size];
    }
    
    private int GetLeftChildIndex(int index) => 2 * index + 1;
    private int GetRightChildIndex(int index) => 2 * index + 2;
    private int GetParentIndex(int index) => (index - 1) / 2;
    
    private bool HasRightChild(int index) => GetRightChildIndex(index) < size;
    private bool HasLeftChild(int index) => GetLeftChildIndex(index) < size;

    public bool IsEmpty() => size == 0;

    private void Swap(int firstIndex, int secondIndex)
    {
        (elements[firstIndex], elements[secondIndex]) = (elements[secondIndex], elements[firstIndex]);
    }

    public int Peek()
    {
        if (size == 0)
            throw new IndexOutOfRangeException();

        return elements[0];
    }

    public int Pop()
    {
        if (size == 0)
            throw new IndexOutOfRangeException();

        int result = elements[0];
        elements[0] = elements[size - 1];
        size--;

        HeapifyDown();

        return result;
    }

    public void Add(int element)
    {
        if (size == elements.Length)
            throw new IndexOutOfRangeException();
        
        elements[size] = element;
        size++;

        HeapifyUp();
    }

    private void HeapifyDown()
    {
        int index = 0;
        while (HasLeftChild(index))
        {
            int smallerIndex = GetLeftChildIndex(index);
            if (HasRightChild(index) && GetRightChildIndex(index) < GetLeftChildIndex(index))
            {
                smallerIndex = GetRightChildIndex(index);
            }

            if (elements[smallerIndex] >= elements[index])
            {
                break;
            }

            Swap(smallerIndex, index);
            index = smallerIndex;
        }
    }

    private void HeapifyUp()
    {
        int index = size - 1;
        while(index != 0 && elements[index] < elements[GetParentIndex(index)])
        {
            int parentIndex = GetParentIndex(index);
            Swap(parentIndex, index);
            index = parentIndex;
        }
    }

}