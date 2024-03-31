using System.Collections;
using System.Diagnostics;

namespace EA_UPB {

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

        public T? RemoveHead()
        {
            if (first==null)
                throw new Exception("Lista vacia");
            T? i = first.item;
            if (last==first) last=null;
            first = first.sig;
            n--;
            return i;
        }

        public Boolean IsEmpty() 
        {
            return first==null;
        }

        public int Size()
        {
            return n;
        }

        // TODO Remueve el ultimo elemento de la lista
        public T? RemoveLast() { return default(T); }

        // TODO Agregar un elemento al final de la lista */
        public void AddLast() {  }

        // TODO Obtener el item en la i-ésima posición de la lista */
        public T? Get(int i) { return default(T); }

        // TODO Insertar un item en la i-ésima posición de la lista */
        public void Insert(int i, T dato) { }

        // TODO remueve el item de la i-ésima posición de la lista */
        public T? Remove(int i) { return default(T); }

        /** Obtener una nueva ListaDoble con todos los items en orden inverso */
        public ListaDoble<T>? Invert() { return default; }

        /** Dividir una lista en dos mitades */
        public ListaDoble<T>[]? SplitList() { return default; }

        /** 
         * Implementacion del iterador para la ListaDoble
         */
        public IEnumerator<T> GetEnumerator()
        {
            for(Nodo? pos = first; pos!=null; pos=pos.sig) {
                yield return pos.item;
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }


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
