using System;
using System.Collections.Generic;


/// <summary>
/// Stores a prioritized node item
/// </summary>
/// <typeparam name="V"></typeparam>
internal class PriorityNode<V> : IComparable
{
    public V item;              // Item to prioritize
    public double priority;     // LOWER value is HIGHER priority
    public int index;           // Position in the pqueue

    /// <summary>
    /// Creates a priority item with assigned priority
    /// </summary>
    /// <param name="item"></param>
    /// <param name="priority"></param>
    public PriorityNode(V item, double priority)
    {
        this.item = item;
        this.priority = priority;
        this.index = -1;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="node"></param>
    public PriorityNode(PriorityNode<V> node)
    {
        this.item = node.item;
        this.priority = node.priority;
        this.index = node.index;
    }

    /// <summary>
    /// Compare two items by priority
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int CompareTo(Object item)
    {
        PriorityNode<V> i = (PriorityNode<V>)item;
        if (i == null)
        {
            throw new Exception("ERROR: Cannot compare to " + item);
        }

        return this.priority.CompareTo(i.priority);
    }
}

/// <summary>
/// Priority queue
/// </summary>
/// <typeparam name="T"></typeparam>
internal class PriorityQueue<T> where T : class
{
    List<PriorityNode<T>> priorityQueue = new List<PriorityNode<T>>();

    /// <summary>
    /// Number of items in the pqueue
    /// </summary>
    public int Count
    {
        get { return priorityQueue.Count; }
    }

    /// <summary>
    /// Clear the pqueue
    /// </summary>
    public void Clear()
    {
        priorityQueue.Clear();
    }

    /// <summary>
    /// Add a new item to the pqueue, put it in priority order
    /// </summary>
    /// <param name="node"></param>
    public void Enqueue(PriorityNode<T> node)
    {
        // Add to the end of the list
        node.index = priorityQueue.Count;
        priorityQueue.Add(node);
        int current = priorityQueue.Count - 1;
        RaisePriority(current);
    }

    /// <summary>
    /// Remove an item from the pqueue, highest priority first
    /// </summary>
    /// <returns></returns>
    public PriorityNode<T> Dequeue()
    {
        if (priorityQueue.Count == 0)
        {
            throw new Exception("Cannot dequeue from an empty queue");
        }

        // Store the first item, we will return it
        PriorityNode<T> node = new PriorityNode<T>(priorityQueue[0]);

        // Store the last item, remove it from the list
        priorityQueue[0] = priorityQueue[priorityQueue.Count - 1];
        priorityQueue[0].index = 0;
        priorityQueue.RemoveAt(priorityQueue.Count - 1);

        LowerPriority(0);

        return node;
    }

    /// <summary>
    /// Change the priority of a node
    /// </summary>
    /// <param name="node"></param>
    /// <param name="newPriority"></param>
    public void ChangePriority(PriorityNode<T> node, double newPriority)
    {
        if (0 <= node.index && node.index < priorityQueue.Count)
        {
            node.priority = newPriority;
            priorityQueue[node.index].priority = newPriority;

            RaisePriority(node.index);
            LowerPriority(node.index);
        }
    }

    /// <summary>
    /// Raise the priority of the node
    /// </summary>
    /// <param name="current"></param>
    private void RaisePriority(int current)
    {
        PriorityNode<T> newItem = priorityQueue[current];
        int parent = (current - 1) / 2;

        // Percolate UP
        while (current > 0 && newItem.CompareTo(priorityQueue[parent]) < 0)
        {
            // Copy parent down into current
            priorityQueue[current] = priorityQueue[parent];
            priorityQueue[current].index = current;
            current = parent;
            parent = (current - 1) / 2;
        }
        priorityQueue[current] = newItem;
        priorityQueue[current].index = current;
    }

    /// <summary>
    /// Lower the priority of a node
    /// </summary>
    /// <param name="current"></param>
    private void LowerPriority(int current)
    {
        if (priorityQueue.Count >= 1)
        {
            PriorityNode<T> lastItem = priorityQueue[current];

            // Percolate Down
            int parent = current;
            int left = parent * 2 + 1;
            int right = parent * 2 + 2;
            int swap = current;
            bool swapped = true;

            while (swapped && left < priorityQueue.Count)
            {
                // Assume we will swap with the left child
                swap = left;

                // If the right child exists and its priority is less than the left child,
                // choose the right child as the "to swap" item
                if (right < priorityQueue.Count && priorityQueue[right].CompareTo(priorityQueue[left]) < 0)
                {
                    swap = right;
                }

                // If the "to swap" item is lower priority than the parent, swap them.
                if (priorityQueue[swap].CompareTo(lastItem) < 0)
                {
                    priorityQueue[parent] = priorityQueue[swap];
                    priorityQueue[parent].index = parent;

                    parent = swap;
                    left = parent * 2 + 1;
                    right = parent * 2 + 2;
                }
                else
                {
                    swapped = false;
                }
            }
            priorityQueue[parent] = lastItem;
            priorityQueue[parent].index = parent;
        }
    }

    /// <summary>
    /// Determine if an item is in the pqueue
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(T item)
    {
        foreach (PriorityNode<T> p in priorityQueue)
        {
            if (p.item == item)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  Get the node that corresponds to the item
    /// </summary>
    /// <param name="item"></param>
    /// <returns>the PriorityNode or null if not found</returns>
    public PriorityNode<T> Find(T item)
    {
        foreach (PriorityNode<T> p in priorityQueue)
        {
            if (p.item == item)
            {
                return p;
            }
        }
        return null;
    }

    /// <summary>
    /// Convert the pqueue to a string for printing
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string output = "[ ";
        for (int i = 0; i < priorityQueue.Count; ++i)
        {
            output += priorityQueue[i].priority + " ";
        }
        output += "]";
        return output;
    }
}
