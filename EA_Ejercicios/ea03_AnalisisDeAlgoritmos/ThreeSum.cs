using System.Diagnostics;

namespace EA_UPB {

    class ThreeSum
    {
        /// <summary>
        /// Cuenta el número de ternas (i, j, k) distintas cuyos elementos suman cero.
        /// Time: O(n^3), Space: O(1).
        /// </summary>
        /// <param name="data">Arreglo de enteros a analizar.</param>
        /// <returns>Número de ternas que suman cero.</returns>
        public static int Count(int[] data)
        {
            int count = 0;
            int N = data.Length;
            for(int i=0; i<N; i++)
                for(int j=i+1; j<N; j++)
                    for(int k=j+1; k<N; k++)
                        if (data[i]+data[j]+data[k]==0)
                            count++;
            return count;
        }

        public static void Main()
        {
            int N = 2000;
            int[] data = new int[N];
            var rand = new Random();

            for(int i=0; i<data.Length; i++)
                data[i] = rand.Next(-100,100);

            Stopwatch sw = Stopwatch.StartNew();
            int c = Count(data);
            sw.Stop();
            Console.WriteLine($"Se encontraron {c} ternas en {sw.ElapsedMilliseconds} msec");
        }
    }

}
