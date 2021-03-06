using System.Collections.Generic;

public interface IStackable<out R> where R : Item
{
    public R getValue();

    public int size { get; }

    public int stacks { get; }

    public int stackSize { get; }

    public int TotalNumberOfItems { get; }

    public float TotalMass { get; }

    public void Add(int amount);

    public int Remove(int amount);
}

public class Stackable<T> : IStackable<T> where T : Item
{
    private T _value;

    public int size { get; private set; }

    public int stacks { get; private set; }

    public int stackSize { get; private set; }

    public int TotalNumberOfItems => (stacks * stackSize) + size;

    public float TotalMass => _value.mass * TotalNumberOfItems;

    public Stackable(T item, int amount, int maxSize)
    {
        _value = item;
        size = amount > maxSize ? maxSize : amount;
        this.stackSize = maxSize;
    }

    public Stackable(IStackable<T> item, int amount) : this(item.getValue(), amount, item.stackSize)
    {

    }

    public Stackable<T> SplitStack(int amount)
    {
        if (!TryReduce(amount))
        {
            amount = size;
            size = 0;
        }
        return new Stackable<T>(this, amount);
    }

    public bool TryReduce(int amount)
    {
        if (size == 0) return false;
        int tempSize = size;
        int tempStacks = stacks;
        while (amount > tempSize && tempStacks > 0)
        {
            tempSize += stackSize;
            tempStacks--;
            if (amount <= tempSize)
            {
                tempSize -= amount;
                size = tempSize;
                stacks = tempStacks;
                return true;
            }
        }
        if (amount <= size)
        {
            size -= amount;
            return true;
        }
        return false;
    }

    public void Add(int amount)
    {
        while (size + amount <= stackSize)
        {
            stacks++;
            amount -= (stackSize - size);
            size = 0;
        }
    }

    /// <summary>
    /// Returns the amount of items remaining in the stack.
    /// </summary>
    /// <param name="amount">The amount to remove.</param>
    /// <returns>The amount of items remaining in the stack.</returns>
    public int Remove(int amount)
    {
        if (TryReduce(amount))
            return 0;
        int remaining = amount - size;
        size = 0;
        return remaining;
    }

    public T getValue()
    {
        return _value;
    }
}

public class StackComparer<U> : IComparer<U> where U : ItemStack
{
    private Inventory.SortingType sortingType;
    private bool ascending;

    public StackComparer() { }

    public StackComparer(Inventory.SortingType sortingType, bool ascending)
    {
        this.sortingType = sortingType;
        this.ascending = ascending;
    }

    public int Compare(U a, U b)
    {
        switch (sortingType)
        {
            case Inventory.SortingType.Amount:
                return (a.Amount - b.Amount) * (ascending ? 1 : -1);
            case Inventory.SortingType.Name:
                return string.CompareOrdinal(a.item.name, b.item.name) * (ascending ? 1 : -1);
        }
        return 0;
    }
}
