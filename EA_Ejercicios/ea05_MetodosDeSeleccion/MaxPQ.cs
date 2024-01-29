using System.Diagnostics;

namespace Algs4
{

    /**
     * MaxPQ : Implementacion de la cola de Maxima Prioridad
     *         Traducida del original en Java del texto de Sedgewick & Wayne
     */
    class MaxPQ<Key>
    {

        private Key?[] pq;                    // store items at indices 1 to n
        private int n;                       // number of items on priority queue
        private IComparer<Key>? comparator;  // optional comparator

        /**
         * Initializes an empty priority queue with the given initial capacity.
         *
         * @param  initCapacity the initial capacity of this priority queue
         */
        public MaxPQ(int initCapacity)
        {
            pq = new Key[initCapacity + 1];
            n = 0;
        }

        /**
         * Initializes an empty priority queue.
         */
        // public MaxPQ() {
        //     MaxPQ<T>(1);
        // }

        /**
         * Initializes an empty priority queue with the given initial capacity,
         * using the given comparator.
         *
         * @param  initCapacity the initial capacity of this priority queue
         * @param  comparator the order in which to compare the keys
         */
        public MaxPQ(int initCapacity, IComparer<Key> comparator)
        {
            this.comparator = comparator;
            pq = new Key[initCapacity + 1];
            n = 0;
        }

        /**
         * Initializes an empty priority queue using the given comparator.
         *
         * @param  comparator the order in which to compare the keys
         */
        public MaxPQ(IComparer<Key> comparator) {
            this.comparator = comparator;
            pq = new Key[101];
            n = 0;
        }

        /**
         * Initializes a priority queue from the array of keys.
         * Takes time proportional to the number of keys, using sink-based heap construction.
         *
         * @param  keys the array of keys
         */
        public MaxPQ(Key[] keys)
        {
            n = keys.Length;
            pq = new Key[n + 1];
            for (int i = 0; i < n; i++)
                pq[i + 1] = keys[i];
            for (int k = n / 2; k >= 1; k--)
                Sink(k);
            Debug.Assert(IsMaxHeap());
        }



        /**
         * Returns true if this priority queue is empty.
         *
         * @return {@code true} if this priority queue is empty;
         *         {@code false} otherwise
         */
        public bool IsEmpty()
        {
            return n == 0;
        }

        /**
         * Returns the number of keys on this priority queue.
         *
         * @return the number of keys on this priority queue
         */
        public int Size()
        {
            return n;
        }

        /**
         * Returns a largest key on this priority queue.
         *
         * @return a largest key on this priority queue
         * @throws NoSuchElementException if this priority queue is empty
         */
        public Key? Max()
        {
            if (IsEmpty() || pq[1]==null) throw new Exception("Priority queue underflow");
            return pq[1];
        }

        // resize the underlying array to have the given capacity
        private void Resize(int capacity)
        {
            Debug.Assert(capacity > n);
            Key?[] temp = new Key[capacity];
            for (int i = 1; i <= n; i++)
            {
                temp[i] = pq[i];
            }
            pq = temp;
        }


        /**
         * Adds a new key to this priority queue.
         *
         * @param  x the new key to add to this priority queue
         */
        public void Insert(Key x)
        {

            // double size of array if necessary
            if (n == pq.Length - 1) Resize(2 * pq.Length);

            // add x, and percolate it up to maintain heap invariant
            pq[++n] = x;
            swim(n);
            Debug.Assert(IsMaxHeap());
        }

        /**
         * Removes and returns a largest key on this priority queue.
         *
         * @return a largest key on this priority queue
         * @throws NoSuchElementException if this priority queue is empty
         */
        public Key? DelMax()
        {
            if (IsEmpty()) throw new Exception("Priority queue underflow");
            Key? max = pq[1];
            Exch(1, n--);
            Sink(1);
            pq[n + 1] = default;     // to avoid loitering and help with garbage collection
            if ((n > 0) && (n == (pq.Length - 1) / 4)) Resize(pq.Length / 2);
            Debug.Assert(IsMaxHeap());
            return max;
        }


        /***************************************************************************
         * Helper functions to restore the heap invariant.
         ***************************************************************************/

        private void swim(int k)
        {
            while (k > 1 && less(k / 2, k))
            {
                Exch(k / 2, k);
                k = k / 2;
            }
        }

        private void Sink(int k)
        {
            while (2 * k <= n)
            {
                int j = 2 * k;
                if (j < n && less(j, j + 1)) j++;
                if (!less(k, j)) break;
                Exch(k, j);
                k = j;
            }
        }

        /***************************************************************************
         * Helper functions for compares and swaps.
         ***************************************************************************/
        private bool less(int i, int j)
        {
            if (comparator == null)
            {
                return ((IComparable<Key>)pq[i]).CompareTo(pq[j]) < 0;
            }
            else
            {
                return comparator.Compare(pq[i], pq[j]) < 0;
            }
        }

        private void Exch(int i, int j)
        {
            Key? swap = pq[i];
            pq[i] = pq[j];
            pq[j] = swap;
        }

        // is pq[1..n] a max heap?
        private bool IsMaxHeap()
        {
            // printHeap();
            for (int i = 1; i <= n; i++)
            {
                if (pq[i] == null) return false;
            }
            for (int i = n + 1; i < pq.Length; i++)
            {
                if (pq[i] != null) return false;
            }
            if (pq[0] != null) return false;
            return IsMaxHeapOrdered(1);
        }

        // is subtree of pq[1..n] rooted at k a max heap?
        private bool IsMaxHeapOrdered(int k)
        {
            if (k > n) return true;
            int left = 2 * k;
            int right = 2 * k + 1;
            if (left <= n && less(k, left)) return false;
            if (right <= n && less(k, right)) return false;
            return IsMaxHeapOrdered(left) && IsMaxHeapOrdered(right);
        }

        public void printHeap() {
            for(int i=1; i<=n; i++)
                Console.WriteLine(pq[i]+", ");
        }

        /***************************************************************************
         * Iterator.
         ***************************************************************************/

        /**
         * Returns an iterator that iterates over the keys on this priority queue
         * in descending order.
         * The iterator doesn't implement {@code remove()} since it's optional.
         *
         * @return an iterator that iterates over the keys in descending order
         */
        public IEnumerator<Key?> Iterator()
        {
            for (int i = 0; i < pq.Length; i++)
                yield return pq[i];
        }

        public static void UnitTests() {
            MaxPQ<string> cola = new(10);
            Debug.Assert(cola.Size()==0);
            cola.Insert("caballo");
            cola.Insert("jirafa");
            cola.Insert("arbol");
            Debug.Assert(cola.Size()==3);

        }

    }

}