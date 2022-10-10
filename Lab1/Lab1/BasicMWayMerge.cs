using System.Diagnostics;

namespace Lab1;

public class BasicMWayMerge
{
    public readonly string initFilePath;
    private readonly int filesinArray;
    private readonly long totalIntegers;
    private int buffSize;

    public BasicMWayMerge(long integers ,int mOfFiles)
    {
        initFilePath = Constants.initFilePath;
        filesinArray = mOfFiles;
        totalIntegers = integers;
        buffSize = GetBuffSize(integers);
        CreateFiles();
    }
    
    private int GetBuffSize(long integers) => (integers * sizeof(Int32)) switch
    {
        <= 10*Constants.Mb => 10 / filesinArray * Constants.Mb,
        (> 10*Constants.Mb) and (< Constants.Gb) => 100 / filesinArray * Constants.Mb,
        >= Constants.Gb => 500 / filesinArray * Constants.Mb,
    };

    private void CreateFiles()
    {
        for (int i = 0; i < filesinArray; i++)
        {
            if (!File.Exists($"B{i}.dat"))
            {
                File.Create($"B{i}.dat").Close();
            }
            else
            {
                FileStream fs = new FileStream($"B{i}.dat", FileMode.Open);
                fs.SetLength(0);
                fs.Close();
            }

            if (!File.Exists($"C.{i}.dat"))
            {
                File.Create($"C{i}.dat").Close();
            }
            else
            {
                FileStream fs = new FileStream($"C{i}.dat", FileMode.Open);
                fs.SetLength(0);
                fs.Close();
            }
        }
    }
    
    public void Sort()
    {
        PreSortDistribution();
        bool flag = true;
        while (!SortIsDone())
        {
            if (flag)
            {
                Merge(Constants.bFiles, Constants.cFiles);
            }
            else
            {
                Merge(Constants.cFiles, Constants.bFiles);
            }

            flag = !flag;
        }
        FileInfo infoB = new FileInfo(String.Concat(Constants.bFiles, "0.dat"));
        FileInfo infoC = new FileInfo(String.Concat(Constants.cFiles, "0.dat"));
        string writtenFile = (infoB.Length > infoC.Length) ? "B0.dat" : "C0.dat";
        File.Copy(writtenFile, initFilePath, true);
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
            
            for (long j = 0; j < ((totalIntegers * 4) / (buffSize)); j++)
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
            if (((totalIntegers * 4) % buffSize) != 0)
            {
                byte[] buff = readFileA.ReadBytes((int)(totalIntegers * 4) % buffSize);
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

    private bool SortIsDone()
    {
        FileInfo infoB = new FileInfo(String.Concat(Constants.bFiles, "0.dat"));
        FileInfo infoC = new FileInfo(String.Concat(Constants.cFiles, "0.dat"));
        return infoB.Length == (totalIntegers * 4) || infoC.Length == (totalIntegers * 4);
    }

    private void Merge(string sourceFiles, string destinationFiles)
    {
        int activeFilesArrayLength = 0;
        for (int i = 0; i < filesinArray; i++)
        {
            using(FileStream fs = File.Open(String.Concat(destinationFiles, $"{i}.dat"), FileMode.Open))
            {
                fs.SetLength(0);
            }

            FileInfo info = new FileInfo(String.Concat(sourceFiles, $"{i}.dat"));
            if (info.Length > 0)
                activeFilesArrayLength++;
        }

        BinaryReader[] sourceFileReaders = new BinaryReader[activeFilesArrayLength];
        for (int j = 0; j < activeFilesArrayLength; j++)
        {
            sourceFileReaders[j] = new BinaryReader(File.Open(String.Concat(sourceFiles, $"{j}.dat"), FileMode.Open));
        }

        int[] positions = new int[activeFilesArrayLength];
        for (int i = 0; i < activeFilesArrayLength; i++)
        {
            positions[i] = 0;
        }

        BinaryWriter[] destinationFileWriters = new BinaryWriter[filesinArray];
        for (int i = 0; i < filesinArray; i++)
        {
            destinationFileWriters[i] = new BinaryWriter(File.Open(String.Concat(destinationFiles, $"{i}.dat"), FileMode.Append));
        }

        int writeInto = 0;
        while (!ReadersAreMerged(sourceFileReaders))
        {
                int readableFiles = 0;
                foreach (BinaryReader reader in sourceFileReaders)
                {
                    if (!EndOfStream(reader))
                    {
                        readableFiles++;
                    }
                }

                byte[][] buffer = new byte[readableFiles][];
                int[] pointers = new int[readableFiles];
                for (int p = 0; p < readableFiles; p++)
                    pointers[p] = 0;
                
                int iter = 0;
                for (int j = 0; j < activeFilesArrayLength; j++)
                {
                    if (!EndOfStream(sourceFileReaders[j]))
                    {
                        if (sourceFileReaders[j].BaseStream.Length - sourceFileReaders[j].BaseStream.Position >= buffSize)
                        {
                            buffer[iter++] = sourceFileReaders[j].ReadBytes(buffSize);
                        }
                        else
                        {
                            buffer[iter++] = sourceFileReaders[j].ReadBytes((int)(sourceFileReaders[j].BaseStream.Length -
                                                 sourceFileReaders[j].BaseStream.Position));
                        }
                    }
                }

                int? lastOfPrevSeries = null;
                while (!BufferIsAllRead(ref buffer, ref pointers))
                {
                    int readableBuffers = 0;
                    for (int k = 0; k < buffer.Length; k++)
                    {
                        if (pointers[k] < buffer[k].Length)
                        {
                            readableBuffers++;
                        }
                    }
                    int[][] activeSequences = new int[readableBuffers][];
                    int ASIter = 0; 
                    for (int k = 0; k < buffer.Length; k++)
                    {
                        if (pointers[k] < buffer[k].Length)
                        {
                            activeSequences[ASIter++] = SequenceToInts(ref buffer[k], ref pointers[k]);
                        }
                    }

                    long ASLength = 0;
                    for (int l = 0; l < activeSequences.Length; l++)
                        ASLength += activeSequences[l].Length;

                    int[] mergedSequence = new int[ASLength];
                    MergeSequences(ref activeSequences, ref mergedSequence);

                    if (lastOfPrevSeries.HasValue)
                    {
                        if (lastOfPrevSeries > mergedSequence[0])
                        {
                            writeInto = (writeInto + 1) % destinationFileWriters.Length;
                        }
                    }
                    for (int m = 0; m < mergedSequence.Length; m++)
                    {
                        destinationFileWriters[writeInto].Write(mergedSequence[m]);
                    }

                    lastOfPrevSeries = mergedSequence[^1];
                }

                if (sourceFileReaders[0].BaseStream.Length / buffSize > 1.5)
                {
                    buffSize = (int)sourceFileReaders[0].BaseStream.Length;
                }
        }

        foreach (var reader in sourceFileReaders)
        {
            reader.Close();    
        }

        foreach (var writer in destinationFileWriters)
        {
            writer.Close();
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

    private bool ReadersAreMerged(BinaryReader[] readers)
    {
        bool flag = true;
        foreach (var reader in readers)
        {
            if (!EndOfStream(reader))
            {
                flag = false;
            }
        }

        return flag;
    }
    
    private bool EndOfStream(BinaryReader reader) => reader.BaseStream.Position == reader.BaseStream.Length;
    private int[] SequenceToInts(ref byte[] buff, ref int position)
    {
        int start = position;
        int end = ReadSequence(ref buff, ref position);
        int[] array = new int[(end - start) / 4];
        for (int i = 0; i < (end - start) / 4; i++)
        {
            array[i] = BitConverter.ToInt32(buff[(4 * i)..(4 * i + 4)]);
        }

        return array;
    }

    private bool BufferIsAllRead(ref byte[][] buffer, ref int[] pointers)
    {
        bool flag = true;
        for (int i = 0; i < buffer.Length; i++)
        {
            if (pointers[i] < buffer[i].Length)
            {
                flag = false;
            }
            
        }

        return flag;
    }

    private void MergeSequences(ref int[][] activeSequences, ref int[] mergedSequence)
    {
        int[] localPointers = new int[activeSequences.Length];
        for (int i = 0; i < localPointers.Length; i++)
            localPointers[i] = 0;

        var comparables = new int?[activeSequences.Length];
        for (int j = 0; j < comparables.Length; j++)
        {
            comparables[j] = activeSequences[j][localPointers[j]];
        }

        long iter = 0;
        int? minimalIndex = 0;
        minimalIndex = FindMinimalIndex(ref comparables);
        while (minimalIndex != null)
        {
            mergedSequence[iter++] = comparables[minimalIndex.Value].Value;
            if (localPointers[minimalIndex.Value] < activeSequences[minimalIndex.Value].Length - 1)
            {
                comparables[minimalIndex.Value] =
                    activeSequences[minimalIndex.Value][++localPointers[minimalIndex.Value]];
            }
            else
            {
                comparables[minimalIndex.Value] = null;
            }
            minimalIndex = FindMinimalIndex(ref comparables);
        }
    }

    private int? FindMinimalIndex(ref int?[] comparables)
    {
        int min = Int32.MaxValue;
        int? index = null;
        for (int i = 0; i < comparables.Length; i++)
        {
            if (comparables[i] is not null)
            {
                if (min > comparables[i])
                {
                    min = comparables[i].Value;
                    index = i;
                }
            }
        }

        return index;
    }

    public bool CheckIfSorted()
    {
        using (BinaryReader reader = new BinaryReader(new FileStream(Constants.initFilePath, FileMode.Open)))
        {
            int prev = reader.ReadInt32();
            while (!EndOfStream(reader))
            {
                int temp = reader.ReadInt32();
                if (temp >= prev)
                {
                    prev = temp;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}