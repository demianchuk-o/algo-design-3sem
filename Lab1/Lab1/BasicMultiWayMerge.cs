using System.Diagnostics;

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
        CreateFiles();
    }
    
    private int GetBuffSize(long integers) => (integers * sizeof(Int32)) switch
    {
        <= 10*Constants.Mb => Constants.Mb,
        (> 10*Constants.Mb) and (< Constants.Gb) => (int)(totalIntegers * sizeof(Int32) / 16),
        >= Constants.Gb => (100 * Constants.Mb),
    };

    private void CreateFiles()
    {
        for (int i = 0; i < filesinArray; i++)
        {
            if (!File.Exists($"B{i}.dat"))
            {
                File.Create($"B{i}.dat").Close();
            }

            if (!File.Exists($"C.{i}.dat"))
            {
                File.Create($"C{i}.dat").Close();
            }
        }
    }
    
    public void Sort()
    {
        PreSortDistribution();
    }

    private void PreSortDistribution()
    {
        using (BinaryReader readFileA = new BinaryReader(File.Open(initFilePath, FileMode.OpenOrCreate)))
        {
            BinaryWriter[] bFileWriters = new BinaryWriter[filesinArray];
            for (int i = 0; i < filesinArray; i++)
            {
                bFileWriters[i] = new BinaryWriter(new FileStream($"B{i}.dat", FileMode.Append, FileAccess.Write));
            }

            long len = (totalIntegers * 4) / (buffSize);
            for (long j = 0; j < len; j++)
            {
                byte[] buff = readFileA.ReadBytes(buffSize);
                int index = 0;
                while (index < buff.Length)
                {
                    for (int k = 0; k < filesinArray; k++)
                    {
                        int start = index;
                        int end = ReadSequence(ref buff, ref index);
                        bFileWriters[k].Write(buff[start..end]);
                    }
                }
                
            }
            foreach (BinaryWriter bw in bFileWriters)
            {
                bw.Close();
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