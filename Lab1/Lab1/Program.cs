namespace Lab1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter the amount of integers to write: ");
            int fileLength = Int32.Parse(Console.ReadLine());

            Random rng = new Random();

            using (BinaryWriter wr = new BinaryWriter(File.Open("A.dat", FileMode.OpenOrCreate)))
            {
                for (int i = 0; i < fileLength; i++)
                    wr.Write(rng.Next());
            }

            int amountOfFiles = fileLength / 100;

            BinaryWriter[] writers = new BinaryWriter[amountOfFiles];
            

            using (BinaryReader reader = new BinaryReader(File.Open("A.dat", FileMode.Open)))
            {
                FileInfo startFileInfo = new FileInfo("A.dat");
                int index = 0;
                while (index < startFileInfo.Length)
                {
                    for (int j = 0; j < amountOfFiles; j++)
                    {
                        int[] serie = DetectSequence(reader, "A.dat", ref index);
                        writers[j] = new BinaryWriter(File.Open($"B{j + 1}.dat", FileMode.OpenOrCreate));
                        foreach(int elem in serie)
                        {
                            writers[j].Write(elem);
                        }
                        writers[j].Close();
                    }
                }
                
            }

            for (int k = 0; k < fileLength; k++)
            {
                using (BinaryReader reader = new BinaryReader(File.Open($"B{k+1}.dat", FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        Console.WriteLine(reader.ReadInt32());
                    }     
                }
                
            }
        }

        protected static int[] DetectSequence(BinaryReader reader, string path, ref int start)
        {
            FileInfo fileInfo = new FileInfo(path);
            int[] seria = new int[] { };
            if (start < fileInfo.Length)
            {
                int end = start;
                bool isSequence = true;
                reader.BaseStream.Seek(start, SeekOrigin.Begin);
                while (end < fileInfo.Length - 1 && isSequence)
                {
                    int pos1 = reader.ReadInt32();
                    int pos2 = reader.ReadInt32();
                    if (pos1 <= pos2)
                    {
                        seria.Append(pos1);
                        start += 4;
                    }
                    else
                    {
                        isSequence = false;
                    }
                }

            }
            return seria;
        }
    }
}