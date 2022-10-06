namespace Lab1;

public class BasicMultiWayMerge
{
    private string initFilePath;
    private int filesinArray;
    private long totalIntegers;

    public BasicMultiWayMerge(long Integers ,int mOfFiles)
    {
        initFilePath = Constants.initFilePath;
        filesinArray = mOfFiles;
        totalIntegers = Integers;
    }
    
    public void Sort()
    {
        PreSortDistribution();
    }

    private void PreSortDistribution()
    {
        using (BinaryReader readFileA = new BinaryReader(File.Open(initFilePath, FileMode.OpenOrCreate)))
        {
            int index = 0;
            byte[] buff = new byte[100 * Constants.Mb];

            long len = (totalIntegers * 4) / (100 * Constants.Mb);
            for (long j = 0; j < len; j++)
            {
                readFileA.BaseStream.Seek((100 * Constants.Mb)*j, SeekOrigin.Begin);
                buff = readFileA.ReadBytes(100 * Constants.Mb);
                while (index <= buff.Length)
                {
                    for (int k = 0; k < filesinArray; k++)
                    {
                        using (BinaryWriter writeFileB = new BinaryWriter(File.Open($"B{k}.dat",FileMode.OpenOrCreate)))
                        {
                            int start = index;
                            int end = ReadSequence(buff, ref index);
                            writeFileB.Write(buff[start..end]);
                            writeFileB.Close();
                        }
                    }
                }
            }
        }
    }

    private int ReadSequence(byte[] buffer, ref int position)
    {
        if (position >= buffer.Length - 4) return buffer.Length - 4;
        int end = position + 4;
        while(end < (buffer.Length - 4) && BitConverter.ToInt32(buffer[(end-4)..end]) <= BitConverter.ToInt32(buffer[end..(end+4)]))
        {
            end += 4;
        }
        position = end;
        return end;
    }
}