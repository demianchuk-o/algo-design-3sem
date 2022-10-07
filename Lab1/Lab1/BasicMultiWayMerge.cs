namespace Lab1;

public class BasicMultiWayMerge
{
    private readonly string initFilePath;
    private readonly int filesinArray;
    private readonly long totalIntegers;
    private readonly int buffSize;

    public BasicMultiWayMerge(long integers ,int mOfFiles)
    {
        initFilePath = Constants.initFilePath;
        filesinArray = mOfFiles;
        totalIntegers = integers;
        buffSize = GetBuffSize(integers);
    }
    
    private int GetBuffSize(long integers) => (integers * sizeof(Int32)) switch
    {
        <= 10*Constants.Mb => Constants.Mb,
        (> 10*Constants.Mb) and (< Constants.Gb) => (int)(totalIntegers * sizeof(Int32) / 12),
        >= Constants.Gb => (100 * Constants.Mb),
    }; 
    
    public void Sort()
    {
        PreSortDistribution();
    }

    private void PreSortDistribution()
    {
        using (BinaryReader readFileA = new BinaryReader(File.Open(initFilePath, FileMode.OpenOrCreate)))
        {
            int index = 0;

            byte[] buff = new byte[buffSize];

            long len = (totalIntegers * 4) / (buffSize);
            for (long j = 0; j < len; j++)
            {
                readFileA.BaseStream.Seek(buffSize*j, SeekOrigin.Begin);
                buff = readFileA.ReadBytes(buffSize);
                while (index < buff.Length)
                {
                    for (int k = 0; k < filesinArray; k++)
                    {
                        using (BinaryWriter writeFileB = new BinaryWriter(File.Open($"B{k}.dat", File.Exists($"B{k}.dat") ? FileMode.Append : FileMode.OpenOrCreate)))
                        {
                            int start = index;
                            int end = ReadSequence(ref buff, ref index);
                            writeFileB.Write(buff[start..end]);
                        }
                    }
                }
            }
        }
    }

    private void Merge(string sourceFiles, string destinationFiles)
    {
        int activeFilesArrayLength = 0;
        for (int i = 0; i < filesinArray; i++)
        {
            using(FileStream fs = File.Open(destinationFiles + $"{i}.dat", FileMode.Open))
            {
                fs.SetLength(0);
            }

            FileInfo info = new FileInfo(sourceFiles + $"{i}. dat");
            if (info.Length > 0)
                activeFilesArrayLength++;
        }

        var activeFiles = new (int index, int[] array) [activeFilesArrayLength];
        int[] pointers = new int[activeFilesArrayLength];
        for (int i = 0; i < activeFilesArrayLength; i++)
            pointers[i] = 0;

        for (int j = 0; j < activeFilesArrayLength; j++)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(sourceFiles + $"{j}.dat", FileMode.Open)))
            {
                reader.BaseStream.Seek((buffSize * (pointers[j] / buffSize)), SeekOrigin.Begin);
                byte[] buffer = reader.ReadBytes(buffSize);
                activeFiles[j] = (index: j, array: SequenceToInts(ref buffer, ref pointers[j])); 
            }
        }
    }
    private int ReadSequence(ref byte[] buffer, ref int position)
    {
        if (position >= buffer.Length - 3) return buffer.Length;
        int end = position + 4;
        while(end < (buffer.Length - 1) && BitConverter.ToInt32(buffer[(end-4)..end]) <= BitConverter.ToInt32(buffer[end..(end+4)]))
        {
            end += 4;
        }
        position = end;
        return end;
    }

    private int[] SequenceToInts(ref byte[] buff, ref int position)
    {
        int start = position;
        int end = ReadSequence(ref buff, ref position);
        int[] array = new int[(end - start) / 4];
        for (int i = 0; i < (end - start) / 4; i++)
        {
            array[i] = BitConverter.ToInt32(buff[(4 * i)..(4 * i + 3)]);
        }

        return array;
    }
}