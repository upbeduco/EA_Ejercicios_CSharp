namespace EA_UPB
{

    public class PilaArreglo
    {

        private string[] pila;
        private int n;

        public PilaArreglo(int max)
        {
            pila = new string[max];
        }

        public void Push(String s)
        {
            // TODO: Implementar el método
        }

        public string Pop()
        {
            // TODO: Implementar el método
            return default;
        }

        public bool IsEmpty()
        {
            return n == 0;
        }

        public int Size()
        {
            return n;
        }

        // TODO: Dar una implementación del Iterador para la pila
        // implements Iterable<Item>
        // public Iterator<Item> iterator() {


        // TODO: Implementar el procedimiento para cambiar el tamaño del arreglo
        // @SuppressWarnings("unchecked")
        // private void resize(int m)


        public static void Main()
        {
            // TODO: Implementar algunos ejemplos de uso de la pila
            Console.WriteLine("Ejemplo Pila");
        }



    }   

}    
// Ejercicios
// 1. Completar la implementación de la Pila utilizando un arreglo
// 2. Hacer algunas pruebas unitarias de la implementación
// 3. Hacer la implementacion genérica utilizando un parámetro de tipo
// 4. Implementar el iterador de la pila utilizando el orden LIFO
// 5. Hacer algunos ejemplos utilizando Pilas con distintos tipos de datos
