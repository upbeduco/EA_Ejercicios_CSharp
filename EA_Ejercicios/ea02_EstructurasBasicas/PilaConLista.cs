using System.Collections;

namespace EA_UPB
{

    public class PilaConLista<Item> : IEnumerable<Item>
    {


        private class Nodo
        {
            public Item? item;
            public Nodo? sig;
        }
        private Nodo? first;

        private int n;

        public PilaConLista() {
            first = null;
        }

        public void Push(Item s)
        {
            // TODO: Implementar
        }

        public Item? Pop()
        {
            // TODO: Implementar
            return default(Item);
        }

        public bool IsEmpty()
        {
            return n == 0;
        }

        public int Size()
        {
            return n;
        }


        /** 
        * Implementacion del iterador para la PilaConLista
        */
        public IEnumerator<Item> GetEnumerator()
        {
            for (Nodo? pos = first; pos != null; pos = pos.sig)
            {
                yield return pos.item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (Nodo? pos = first; pos != null; pos = pos.sig)
            {
                yield return pos.item;
            }
        }



        public static void Main()
        {
            // TODO: Implementar algunos ejemplos de uso de la PilaConLista

        }

    }


    // Ejercicios:
    // 1. Completar la implementación del API de la Pila
    // 2. Hacer pruebas unitarias
    // 3. Implementar la interfacez Iterable
    // 4. Hacer algunos ejemplos con iteración


}