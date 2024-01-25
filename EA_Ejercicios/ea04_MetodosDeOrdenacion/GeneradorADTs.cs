using StdLib;

namespace EA_UPB {

public class GeneradorADTs {

    private static String[] mujer = { "Isabella", "Olivia", "Alexis", "Sofía", "Victoria", "Amelia", "Alexa", "Julia",
            "Camila", "Alexandra", "Maya", "Andrea", "Ariana", "María", "Eva", "Angelina", "Valeria", "Natalia",
            "Isabel", "Sara", "Liliana", "Adriana", "Juliana", "Gabriela", "Daniela", "Valentina", "Lila", "Vivian",
            "Nora", "Ángela", "Elena", "Clara", "Eliana", "Alana", "Miranda", "Amanda", "Diana", "Ana", "Penélope",
            "Aurora", "Alexandría", "Lola", "Alicia", "Amaya", "Alexia", "Jazmín", "Mariana", "Alina", "Lucía",
            "Fátima", "Ximena", "Laura", "Cecilia", "Alejandra", "Esmeralda", "Verónica", "Daniella", "Miriam",
            "Carmen", "Iris", "Guadalupe", "Selena", "Fernanda", "Angélica", "Emilia", "Lía", "Tatiana", "Mónica",
            "Carolina", "Jimena", "Dulce", "Talía", "Estrella", "Brenda", "Lilian", "Paola", "Serena", "Celeste",
            "Viviana", "Elisa", "Melina", "Gloria", "Claudia", "Sandra", "Marisol", "Asia", "Ada", "Rosa", "Isabela",
            "Regina", "Elsa", "Perla", "Raquel", "Virginia", "Patricia", "Linda", "Marina", "Leila", "América",
            "Mercedes" };

    private static String[] hombre = { "Daniel", "David", "Gabriel", "Benjamín", "Samuel", "Lucas", "Ángel", "José",
            "Adrián", "Sebastián", "Xavier", "Juan", "Luis", "Diego", "Óliver", "Carlos", "Jesús", "Alex", "Max",
            "Alejandro", "Antonio", "Miguel", "Víctor", "Joel", "Santiago", "Elías", "Iván", "Óscar", "Leonardo",
            "Eduardo", "Alan", "Nicolás", "Jorge", "Omar", "Paúl", "Andrés", "Julián", "Josué", "Román", "Fernando",
            "Javier", "Abraham", "Ricardo", "Francisco", "César", "Mario", "Manuel", "Édgar", "Alexis", "Israel",
            "Mateo", "Héctor", "Sergio", "Emiliano", "Simón", "Rafael", "Martín", "Marco", "Roberto", "Pedro",
            "Emanuel", "Abel", "Rubén", "Fabián", "Emilio", "Joaquín", "Marcos", "Lorenzo", "Armando", "Adán", "Raúl",
            "Julio", "Enrique", "Gerardo", "Pablo", "Jaime", "Saúl", "Esteban", "Gustavo", "Rodrigo", "Arturo",
            "Mauricio", "Orlando", "Hugo", "Salvador", "Alfredo", "Maximiliano", "Ramón", "Ernesto", "Tobías", "Abram",
            "Noé", "Guillermo", "Ezequiel", "Lucián", "Alonzo", "Felipe", "Matías", "Tomás", "Jairo" };

    private static String[] apellidos = { "González", "Muñoz", "Rojas", "Díaz", "Pérez", "Soto", "Contreras", "Silva",
            "Martínez", "Sepúlveda", "Morales", "Rodríguez", "López", "Fuentes", "Hernández", "Torres", "Araya",
            "Flores", "Espinoza", "Valenzuela", "Castillo", "Tapia", "Reyes", "Gutiérrez", "Castro", "Pizarro",
            "Álvarez", "Vásquez", "Sánchez", "Fernández", "Ramírez", "Carrasco", "Gómez", "Cortés", "Herrera", "Núñez",
            "Jara", "Vergara", "Rivera", "Figueroa", "Riquelme", "García", "Miranda", "Bravo", "Vera", "Molina", "Vega",
            "Campos", "Sandoval", "Orellana", "Cárdenas", "Olivares", "Alarcón", "Gallardo", "Ortiz", "Garrido",
            "Salazar", "Guzmán", "Henríquez", "Saavedra", "Navarro", "Aguilera", "Parra", "Romero", "Aravena", "Vargas",
            "Vázquez", "Cáceres", "Yáñez", "Leiva", "Escobar", "Ruiz", "Valdés", "Vidal", "Salinas", "Zúñiga", "Peña",
            "Godoy", "Lagos", "Maldonado", "Bustos", "Medina", "Pino", "Palma", "Moreno", "Sanhueza", "Carvajal",
            "Navarrete", "Sáez", "Alvarado", "Donoso", "Poblete", "Bustamante", "Toro", "Ortega", "Venegas", "Guerrero",
            "Mendoza", "Farías", "San", "Martín" };

    public static void arrayFromInput() {
        String[] lines = StdIn.ReadAllStrings();
        String comma = "";
        System.Text.StringBuilder sb = new System.Text.StringBuilder("{");
        foreach(string s in lines) {
            sb.Append(comma + "\"" + s + "\"");
            comma = ",";
        }
        sb.Append("};");
        Console.WriteLine(sb);
    }

    public static List<Person> Generar(int n) {
        List<Person> personas = new List<Person>();
        for (int i = 0; i < n; i++) {
            int genero = StdRandom.Uniform(0, 2);
            string nombre1, nombre2;
            if (genero == 0) {
                nombre1 = mujer[StdRandom.Uniform(mujer.Length)];
                nombre2 = mujer[StdRandom.Uniform(mujer.Length)];
            } else {
                nombre1 = hombre[StdRandom.Uniform(hombre.Length)];
                nombre2 = hombre[StdRandom.Uniform(hombre.Length)];
            }
            string apellido1 = apellidos[StdRandom.Uniform(apellidos.Length)];
            string apellido2 = apellidos[StdRandom.Uniform(apellidos.Length)];
            Person p = new Person(nombre1+" "+nombre2, apellido1+" "+apellido2, StdRandom.Uniform(0, 80), (float) StdRandom.Uniform(2.0, 100.0));
            personas.Add(p);
        }
        return personas;
    }

    public static void Main() {
        // arrayFromInput();
        List<Person> personas = Generar(10);
        foreach(Person p in personas)
            Console.WriteLine(p);

        // Test correct handling of special characters
        string test ="привет"; 
        Console.WriteLine(test);

    }

}


}