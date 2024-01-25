namespace EA_UPB {
    
public class OrdenarADTs {

    public static void Main() {

        List<Person> personas = GeneradorADTs.Generar(10);
        foreach(Person p in personas)
            Console.WriteLine(p);  // TODO: Arreglar para que imprima correctamente caracteres UTF-8
        
        // Uso de la interfaz Comparable
        Console.WriteLine("persona[0] < persona[1] ? " + personas.ElementAt(0).CompareTo(personas.ElementAt(1)));


        // Ejercicios
        // 1. Ordenar por apellido y nombre ascendiente
        // 2. Ordenar por edad ascendiente
        // 3. Ordenar por peso descendiente
        // 4. Ordenar por edad y peso
    
        // Medición de tiempos y comparación, para N grandes
        // 1. Generar un arreglo de tamaño N
        // 2. Hacer tres copias del arreglo
        // 3. Ordenar una copia por un método cuadratico
        // 4. Ordenar por un método linearitmético
        // 5. Ordenar por la biblioteca del lenguaje
        // 6. Comparar los tiempos obtenidos
        // 7. Hace diferencia en el tiempo si se ordenan ascendente/descendente ?
    
                
    }

}


}
