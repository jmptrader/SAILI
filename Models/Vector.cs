using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    [Serializable]
    public class Vector<T>: ICollection<T>, IList<T>
    {
        private T[] data;
        private int count;
        private const int DEFAULT_CAPACITY = 10;

        public Vector()
        {
            data = new T[DEFAULT_CAPACITY];
        }

        public Vector(int CAPACITY)
        {
            data = new T[CAPACITY];
        }

        public void Add(T element)
        {
            bool check = false;
            check = CheckCapacity();
            if (check)
            {
                resizeArray();
            }
            data[count++] = element;
        }

        public void Insert(T element, int index)
        {
            int length = data.Length;
            bool check = false;
            check = CheckCapacity();
            T[] temp = new T[data.Length + 1];

            if (check)
            {
                resizeArray();
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (i < index){
                    temp[i] = data[i];
                    continue;
                }
                if (i == index){
                    temp[i] = element;
                    temp[i + 1] = data[i];
                    count++;
                    continue;
                }
                if (i > index){
                    temp[i + 1] = data[i];
                }
            }
            data = null;
            data = new T[length];
            data = temp;
        }

        public bool CheckCapacity()
        {
            bool check = false;
            if (count == data.Length - 1)
                check = true;
            return check;
        }

        public int Count
        {
            get { return count; }
        }

        public int Capacity
        {
            get { return data.Length; }
        }

        public bool Contains(T element)
        {
            foreach (var x in data)
            {
                if (x.Equals(element))
                {
                    return true;
                }
            }
            return false;
        }

        /* 
         *IndexOf: 
         * Worse case = 3n + 3
         * Best case = 3n + 3
         */
        public int IndexOf(T element)
        {
            int index;

            for (index = 0; index < data.Length; index++)
            {
                if (data[index].Equals(element))
                    break;
            }
            return index;
        }

        public void Remove(T element)
        {
            T[] temp = new T[1];

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Equals(element))
                {
                    data[i] = temp[0];
                    count--;
                    break;
                }
            }
        }

        public void RemoveAll(T element)
        {
            T[] temp = new T[1];

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Equals(element))
                {
                    data[i] = temp[0];
                    count--;
                }
            }
        }

        public void RemoveAt(T element)
        {
            T[] temp = new T[data.Length];
            int track = 0, length = 0;
            bool removeAt = false;

            length = data.Length;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Equals(element))
                {
                    if (!removeAt){
                        removeAt = true;
                    }
                    else
                    {
                        temp[track] = data[i];
                        track++;
                    }
                }
                else
                {
                    temp[track] = data[i];
                    track++;
                }
            }

            data = null;
            data = new T[length];
            data = temp;
            count--;
        }

        /*
         * Min:
         * Worse case = 8n + 5
         * Best case = 7n + 5
         */

        public int Min()
        {
            int min = 0;
            int convert = 0;

            for (int i = 0; i < data.Length; i++){
                convert = Convert.ToInt32(data[i]);

                if (min == 0 && convert > 0){
                    min = convert;
                    continue;
                }
                if (min > convert && convert > 0)
                    min = convert;
            }
            return min;
        }

        /* 
         * Max:
         * Worse case = 6n + 5
         * Best case = 5n + 5
         */

        public int Max()
        {
            int max = 0;
            int convert = 0;

            for (int i = 0; i < data.Length; i++){
                convert = Convert.ToInt32(data[i]);

                if (max == 0 && convert > 0){
                    max = convert;
                    continue;
                }
                if (max < convert)
                    max = convert;
            }
            return max;
        }

        public void Sort()
        {
            Array.Sort(data);
        }

        public void resizeArray()
        {
            Array.Resize(ref data, data.Length + DEFAULT_CAPACITY);
        }

        public void toString()
        {
            foreach (var x in data)
            {
                Console.WriteLine(x.ToString());
            }
        }

        /* 
        * All codes below belongs to: 
        * Dr Mohamed Abdelrazek Unit Chair Data Structures 
        * And Algorithms - Deakin University. 
        *  
        */

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)data).CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<T>)data).GetEnumerator();
        }

        public void Clear()
        {
            ((ICollection<T>)data).Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((ICollection<T>)data).GetEnumerator();
        }

        public void Insert(int index, T item)
        {
            ((IList<T>)data).Insert(index, item);
        }

        void IList<T>.RemoveAt(int index)
        {
            ((IList<T>)data).RemoveAt(index);
        }


        private int BinarySearch(T value, int left, int right, IComparer<T> comparer)
        {
            if (comparer == null) comparer = Comparer<T>.Default;
            if (left <= right)
            {
                int middle = (left + right) / 2;
                int result = comparer.Compare(value, data[middle]);
                if (result == 0)
                    return middle;
                if (result < 0)
                    return BinarySearch(value, left, middle - 1, comparer);
                if (result > 0)
                    return BinarySearch(value, middle + 1, right, comparer);
            }
            return -1;

        }
        public int BinarySearch(T element)
        {
            return BinarySearch(element, 0, count, null);
        }
        public int BinarySearch(T element, IComparer<T> comparer)
        {
            return BinarySearch(element, 0, count, comparer);
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<T>)data).IsReadOnly;
            }
        }

        public T this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                if (index >= count)
                    throw new IndexOutOfRangeException();
                data[index] = value;

            }
        }

/* ================= End of Dr Mohamed Abdelrazek  Deakin Univesity Unit Chair ==================*/
    }
}