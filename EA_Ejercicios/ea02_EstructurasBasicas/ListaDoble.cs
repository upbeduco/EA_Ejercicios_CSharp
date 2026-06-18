using System.Collections;
using System.Diagnostics;

namespace EA_UPB {

    /// <summary>
    /// Lista doblemente enlazada (doubly linked list) genérica.
    /// Mantiene referencias al primer y último nodo y el número de elementos.
    /// </summary>
    /// <typeparam name="T">Tipo de los elementos almacenados.</typeparam>
    class ListaDoble<T> : IEnumerable<T>
    {

        private class Nodo
        {
            public T? item;
            public Nodo? sig;
            public Nodo? ant;
        }

        private Nodo? first = null;
        private Nodo? last = null;
        private int n = 0;

        /// <summary>Agrega un elemento al inicio de la lista.</summary>
        /// <param name="item">Elemento a agregar.</param>
        public void AddHead(T item)
        {
            Nodo x = new Nodo();
            x.item = item;
            x.sig = first;
            if (first!=null) first.ant = x;
            first = x;
            if (last==null) last = first;
            n++;
        }

        /// <summary>Remueve y retorna el elemento al inicio de la lista.</summary>
        /// <returns>El elemento removido.</returns>
        /// <exception cref="InvalidOperationException">Si la lista está vacía.</exception>
        public T? RemoveHead()
        {
            if (first==null)
                throw new InvalidOperationException("Lista vacia");
            T? i = first.item;
            if (last==first) last=null;
            first = first.sig;
            if (first!=null) first.ant = null;
            n--;
            return i;
        }

        /// <summary>Indica si la lista no contiene elementos.</summary>
        public bool IsEmpty()
        {
            return first==null;
        }

        /// <summary>Retorna el número de elementos de la lista.</summary>
        public int Size()
        {
            return n;
        }

        // TODO Remueve el ultimo elemento de la lista
        public T? RemoveLast() { return default(T); }

        // TODO Agregar un elemento al final de la lista
        public void AddLast(T item) {  }

        // TODO Obtener el item en la i-ésima posición de la lista
        public T? Get(int i) { return default(T); }

        // TODO Insertar un item en la i-ésima posición de la lista
        public void Insert(int i, T dato) { }

        // TODO remueve el item de la i-ésima posición de la lista
        public T? Remove(int i) { return default(T); }

        /// <summary>Obtener una nueva ListaDoble con todos los items en orden inverso.</summary>
        public ListaDoble<T>? Invert() { return default; }

        /// <summary>Dividir una lista en dos mitades.</summary>
        public ListaDoble<T>[]? SplitList() { return default; }

        /// <summary>
        /// Implementacion del iterador para la ListaDoble.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            for(Nodo? pos = first; pos!=null; pos=pos.sig) {
                yield return pos.item!;
            }

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public static void Demo()
        {
            ListaDoble<string> lista = new ListaDoble<string>();
            lista.AddHead("Hola");
            lista.AddHead("Mundo");
            Console.WriteLine($"lista.size() = {lista.Size()}");

            foreach(string w in lista)
                Console.WriteLine(w);

        }

        public static void Main()
        {
            // Implementación de algunas pruebas unitarias
            ListaDoble<int> l = new ListaDoble<int>();
            Debug.Assert(l.Size()==0);
            l.AddHead(1);
            Debug.Assert(l.Size()==1);
            int x = l.RemoveHead();
            Debug.Assert(x==1);
            Debug.Assert(l.Size()==0);

        }

    }

}
