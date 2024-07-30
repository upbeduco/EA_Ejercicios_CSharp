namespace EA_UPB
{

    class RpnEvaluator {

        private static double Eval() {
            Stack<double> stack = new Stack<double>();
            while (true) {
                string? line = Console.ReadLine();
                if (line == null) { break;}         // When the user presses Ctrl+d (in linux)
                if (line.Equals("+")) {
                    stack.Push(stack.Pop()+stack.Pop());
                }
                // TODO: Implement the other arithmetic operations.
                else {
                    double num = Double.Parse(line);
                    stack.Push(num);
                }
                // TODO: Add error handling in case the input is not a number or an arithmetic operator.
            }
            return stack.Pop();
        }

        public static void Main()
        {
            Console.WriteLine("Result: {0}", Eval());
        }

    }


}
