namespace EA_UPB {

    class ThreeSum
    {
        public static int Count(int[] data)
        {
            int count = 0;
            int N = data.Length;
            for(int i=0; i<N; i++)
                for(int j=i+1; i<N; i++)
                    for(int k=j+1; i<N; i++)
                        if (data[i]+data[j]+data[k]==0)
                            count++;
            return count;
        }

        public static void Main()
        {
            int N = 1000;
            int[] data = new int[N];
            var rand = new Random();

            for(int i=0; i<data.Length; i++)
                data[i] = rand.Next(-100,100);
            
            int c = Count(data);
            Console.WriteLine($"Se encontraron {c} ternas");
        }
    }

}
