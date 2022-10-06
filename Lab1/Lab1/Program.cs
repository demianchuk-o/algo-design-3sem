using System.Buffers.Text;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Lab1;
internal class Program
    {
        public static void Main(string[] args)
        {
            
            Console.Write("Enter the amount of Integers: ");
            long integersToGen = Int64.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.Write("Enter the number of files (m): ");
            int mOffiles = Int32.Parse(Console.ReadLine());
            for (int i = 0; i < mOffiles; i++)
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open($"B{i}.dat", FileMode.OpenOrCreate)))
                {
                    bw.BaseStream.SetLength(0);
                }
            }
            Stopwatch sw = Stopwatch.StartNew();
            Generator.Generate(Constants.initFilePath, integersToGen);
            BasicMultiWayMerge mwm = new BasicMultiWayMerge(integersToGen, mOffiles);
            mwm.Sort();
            sw.Stop();
            Console.WriteLine($"Done in {sw.ElapsedMilliseconds}");
            
        }
        
       
    }