namespace EA_UPB {

    class ThreeSum
    {
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
            
            long inicio = Environment.TickCount64;
            int c = Count(data);
            long fin = Environment.TickCount;
            Console.WriteLine($"Se encontraron {c} ternas en {fin-inicio} msec");
        }
    }

}
