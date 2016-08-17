﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DSA.DataStructures.Lists
{
    /// <summary>
    /// Represents an array-based list structure.
    /// </summary>
    /// <typeparam name="T">The stored data type.</typeparam>
    public class ArrayList<T> : IEnumerable<T>
    {
        /// <summary>
        /// The backing array of the list.
        /// </summary>
        internal T[] array;

        /// <summary>
        /// Gets the current capacity of the backing array.
        /// </summary>
        public int Capacity { get; internal set; }

        /// <summary>
        /// Gets the number of elements in the <see cref="ArrayList{T}"/>.
        /// </summary>
        public int Count { get; internal set; }

        /// <summary>
        /// The element at the given index.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>The element on the given index.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
                return array[index];
            }
            set
            {
                if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
                array[index] = value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ArrayList{T}"/> class with default capacity of 8.
        /// </summary>
        public ArrayList() : this(capacity: 8) { }

        /// <summary>
        /// Creates a new instance of the <see cref="ArrayList{T}"/> class with the given capacity.
        /// </summary>
        /// <param name="capacity">The capacity of the backing array.</param>
        public ArrayList(int capacity)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException();

            Capacity = capacity;
            array = new T[Capacity];
            Count = 0;
        }

        /// <summary>
        /// Resize the backing array to a corresponding size.
        /// </summary>
        /// <param name="increase">if true increses the size. if false decreases it.</param>
        /// <param name="minCapacity">if set determines the minimum needed capacity after resize, else auto change of capacity is performed.</param>
        private void Resize(bool increase, int minCapacity = 0)
        {
            if (minCapacity == 0)// auto increase/decrease
            {
                if (increase)
                {
                    if (Capacity < 2048)
                    {
                        if (Capacity == 0) Capacity = 1;
                        else Capacity *= 2;
                    }
                    else Capacity += 256;
                }
                else Capacity /= 2;
            }
            else// if minCapacity is set
            {
                if (increase)
                {
                    while (Capacity < minCapacity)
                    {
                        if (Capacity < 2048)
                        {
                            if (Capacity == 0) Capacity = 1;
                            else Capacity *= 2;
                        }
                        else Capacity += 256;
                    }
                }
                else
                {
                    while (minCapacity < Capacity) Capacity /= 2;
                    Capacity *= 2;
                }
            }

            T[] newArray = new T[Capacity];
            for (int i = 0; i < Count; i++)
            {
                newArray[i] = array[i];
            }
            array = newArray;
        }

        /// <summary>
        /// Adds an item to the end of the <see cref="ArrayList{T}"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            if (Count == Capacity) Resize(increase: true);
            array[Count++] = item;
        }

        /// <summary>
        /// Adds the elements of the specific collection to the end of the <see cref="ArrayList{T}"/>.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to end of the <see cref="ArrayList{T}"/>.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException();

            int colSize = collection.Count();

            if (Count + colSize > Capacity)
                Resize(increase: true, minCapacity: Count + colSize);

            foreach (var item in collection)
            {
                array[Count++] = item;
            }
        }

        /// <summary>
        /// Inserts an item into the <see cref="ArrayList{T}"/> at the specific index.
        /// </summary>
        /// <param name="index">The index at which the item is inserted.</param>
        /// <param name="item">The item to insert</param>
        public void Insert(int index, T item)
        {
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
            if (index == Count)
            {
                Add(item);
            }
            if (Count == Capacity) Resize(increase: true);

            for (int i = Count - 1; i >= 0; i--)
            {
                array[i + 1] = array[i];
                if (index == i)
                {
                    array[i] = item;
                    break;
                }
            }
            Count++;
        }

        /// <summary>
        /// Inserts the elements of the collection into the <see cref="ArrayList{T}"/> at the specific index.
        /// </summary>
        /// <param name="index">The index at which the item is inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted in the <see cref="ArrayList{T}"/>at the specified index.</param>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
            if (collection == null) throw new ArgumentNullException();

            int colSize = collection.Count();

            if (Count + colSize > Capacity)
                Resize(increase: true, minCapacity: Count + colSize);

            for (int i = Count - 1; i >= index; i--)
            {
                array[i + colSize] = array[i];
            }

            foreach (var item in collection)
            {
                array[index++] = item;
            }

            Count += colSize;
        }

        /// <summary>
        /// Removes the first occurrence of the item from the <see cref="ArrayList{T}"/>.
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>true if the item is removed successfully; otherwise false.</returns>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Removes a range of elements from the <see cref="ArrayList{T}"/>.
        /// </summary>
        /// <param name="index">The starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public void RemoveRange(int index, int count)
        {
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
            if (index + count > Count || count < 1) throw new ArgumentException();

            if (index + count == Count) Count = index;
            else
            {
                while(index + count < Count)
                {
                    array[index] = array[index++ + count];
                }
                Count -= count;
            }

            if (Count - count < Capacity / 3)
                Resize(increase: true, minCapacity: (Count - count) * 3 / 2);
        }

        /// <summary>
        /// Removes the item at the specific index of the <see cref="ArrayList{T}"/>.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();

            for (int i = index; i < Count - 1; i++)
            {
                array[i] = array[i + 1];
            }

            Count--;
            if (Count == Capacity / 3) Resize(increase: false);
        }

        /// <summary>
        /// Searches for the item and returns the index of the first occurrence within the entire <see cref="ArrayList{T}"/>.
        /// </summary>
        /// <param name="item">The item for searching.</param>
        /// <returns>The index of the first occurrence of the item if found; otherwise -1.</returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (object.Equals(array[i], item)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the item and returns the index of the last occurrence within the entire <see cref="ArrayList{T}"/>.
        /// </summary>
        /// <param name="item">The item for searching.</param>
        /// <returns>The index of the last occurrence of the item if found; otherwise -1.</returns>
        public int LastIndexOf(T item)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                if (object.Equals(array[i], item)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Determines whether an item is in the <see cref="ArrayList{T}"/>.
        /// </summary>
        /// <param name="item">The item for searching.</param>
        /// <returns>returns true if the item is found; otherwise false.</returns>
        public bool Contains(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (object.Equals(array[i], item)) return true;
            }
            return false;
        }

        /// <summary>
        /// Removes all elements from the <see cref="ArrayList{T}"/>.
        /// </summary>
        public void Clear()
        {
            Capacity = 10;
            array = new T[Capacity];
            Count = 0;
        }

        /// <summary>
        /// Copies the elements of the <see cref="ArrayList{T}"/> to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the <see cref="ArrayList{T}"/>.</returns>
        public T[] ToArray()
        {
            T[] newArray = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                newArray[i] = array[i];
            }
            return newArray;
        }

        /// <summary>
        /// Returns an enumerator that iterates throught the <see cref="ArrayList{T}"/>.
        /// </summary>
        /// <returns>Enumerator for the <see cref="ArrayList{T}"/></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
