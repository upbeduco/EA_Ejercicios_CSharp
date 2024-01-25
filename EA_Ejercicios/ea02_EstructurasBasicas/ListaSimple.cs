using System.Collections;
using System.Diagnostics;

namespace EA_UPB {

    class ListaSimple<T> : IEnumerable<T>
    {

        private class Nodo
        {
            public T item;
            public Nodo sig;
        }

        private Nodo first = null;
        private int n = 0;

        public void AddHead(T item)
        {
            Nodo x = new Nodo();
            x.item = item;
            x.sig = first;
            first = x;
            n++;
        }

        public T RemoveHead()
        {
            if (first==null)
                throw new Exception("Lista vacia");
            T i = first.item;
            first = first.sig;
            n--;
            return i;
        }

        public Boolean IsEmpty() 
        {
            return first==null;
        }

        public int size()
        {
            return n;
        }

        // TODO Remueve el ultimo elemento de la lista
        public T? removeLast() { return default(T); }

        // TODO Agregar un elemento al final de la lista */
        public void addLast() {  }

        // TODO Obtener el item en la i-ésima posición de la lista */
        public T? get(int i) { return default(T); }

        // TODO Insertar un item en la i-ésima posición de la lista */
        public void insert(int i, T dato) { }

        // TODO remueve el item de la i-ésima posición de la lista */
        public T? remove(int i) { return default(T); }

        /** Obtener una nueva ListaSimple con todos los items en orden inverso */
        public ListaSimple<T> invert() { return null; }

        /** Dividir una lista en dos mitades */
        public ListaSimple<T>[] splitList() { return null; }

        /** 
         * Implementacion del iterador para la ListaSimple
         */
        public IEnumerator<T> GetEnumerator()
        {
            for(Nodo pos = first; pos!=null; pos=pos.sig) {
                yield return pos.item;
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }


        public static void Demo()
        {
            ListaSimple<string> lista = new ListaSimple<string>();
            lista.AddHead("Hola");
            lista.AddHead("Mundo");
            Console.WriteLine($"lista.size() = {lista.size()}");

            foreach(string w in lista)
                Console.WriteLine(w);
            


        }

    }

}
