using System.Diagnostics;

namespace EA_UPB
{

    /// <summary>
    /// MaxPQ: Implementacion de la cola de Maxima Prioridad (max-heap binario).
    /// Traducida del original en Java del texto de Sedgewick &amp; Wayne.
    /// </summary>
    /// <typeparam name="Key">Tipo de las llaves; debe ser comparable (vía
    /// <see cref="IComparable{T}"/>) o proveerse un <see cref="IComparer{T}"/>.</typeparam>
    class MaxPQ<Key>
    {

        private Key?[] pq;                    // store items at indices 1 to n
        private int n;                       // number of items on priority queue
        private IComparer<Key>? comparator;  // optional comparator

        /// <summary>
        /// Inicializa una cola de prioridad vacía con la capacidad inicial dada.
        /// </summary>
        /// <param name="initCapacity">Capacidad inicial de la cola.</param>
        public MaxPQ(int initCapacity)
        {
            pq = new Key[initCapacity + 1];
            n = 0;
        }

        // Initializes an empty priority queue.
        // public MaxPQ() : this(1) { }

        /// <summary>
        /// Inicializa una cola de prioridad vacía con la capacidad inicial dada,
        /// usando el comparador suministrado.
        /// </summary>
        /// <param name="initCapacity">Capacidad inicial de la cola.</param>
        /// <param name="comparator">Orden en el que se comparan las llaves.</param>
        public MaxPQ(int initCapacity, IComparer<Key> comparator)
        {
            this.comparator = comparator;
            pq = new Key[initCapacity + 1];
            n = 0;
        }

        /// <summary>
        /// Inicializa una cola de prioridad vacía usando el comparador suministrado.
        /// </summary>
        /// <param name="comparator">Orden en el que se comparan las llaves.</param>
        public MaxPQ(IComparer<Key> comparator) {
            this.comparator = comparator;
            pq = new Key[101];
            n = 0;
        }

        /// <summary>
        /// Inicializa una cola de prioridad a partir del arreglo de llaves.
        /// Toma tiempo proporcional al número de llaves usando construcción de heap
        /// basada en sink.
        /// </summary>
        /// <param name="keys">El arreglo de llaves.</param>
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



        /// <summary>Retorna true si esta cola de prioridad está vacía.</summary>
        public bool IsEmpty()
        {
            return n == 0;
        }

        /// <summary>Retorna el número de llaves en esta cola de prioridad.</summary>
        public int Size()
        {
            return n;
        }

        /// <summary>Retorna una de las llaves máximas de esta cola de prioridad.</summary>
        /// <returns>Una de las llaves máximas.</returns>
        /// <exception cref="InvalidOperationException">Si la cola de prioridad está vacía.</exception>
        public Key? Max()
        {
            if (IsEmpty() || pq[1]==null) throw new InvalidOperationException("Priority queue underflow");
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


        /// <summary>Agrega una nueva llave a esta cola de prioridad.</summary>
        /// <param name="x">La nueva llave a agregar.</param>
        public void Insert(Key x)
        {

            // double size of array if necessary
            if (n == pq.Length - 1) Resize(2 * pq.Length);

            // add x, and percolate it up to maintain heap invariant
            pq[++n] = x;
            Swim(n);
            Debug.Assert(IsMaxHeap());
        }

        /// <summary>Remueve y retorna una de las llaves máximas de esta cola de prioridad.</summary>
        /// <returns>Una de las llaves máximas.</returns>
        /// <exception cref="InvalidOperationException">Si la cola de prioridad está vacía.</exception>
        public Key? DelMax()
        {
            if (IsEmpty()) throw new InvalidOperationException("Priority queue underflow");
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

        private void Swim(int k)
        {
            while (k > 1 && Less(k / 2, k))
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
                if (j < n && Less(j, j + 1)) j++;
                if (!Less(k, j)) break;
                Exch(k, j);
                k = j;
            }
        }

        /***************************************************************************
         * Helper functions for compares and swaps.
         ***************************************************************************/
        private bool Less(int i, int j)
        {
            if (comparator == null)
            {
                return ((IComparable<Key>)pq[i]!).CompareTo(pq[j]) < 0;
            }
            else
            {
                return comparator.Compare(pq[i]!, pq[j]!) < 0;
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
            // PrintHeap();
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
            if (left <= n && Less(k, left)) return false;
            if (right <= n && Less(k, right)) return false;
            return IsMaxHeapOrdered(left) && IsMaxHeapOrdered(right);
        }

        public void PrintHeap() {
            for(int i=1; i<=n; i++)
                Console.WriteLine(pq[i]+", ");
        }

        /***************************************************************************
         * Iterator.
         ***************************************************************************/

        /// <summary>
        /// Retorna un iterador sobre las llaves de esta cola de prioridad.
        /// </summary>
        /// <returns>Un iterador sobre las llaves.</returns>
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
