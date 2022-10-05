﻿namespace Lab1;

public static class Generator
{
    public static void Generate(string filePath, long integersAmount)
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.OpenOrCreate)))
        {
            int portion = integersAmount switch
            {
                < 10 * 1024 * 1024 => 1000,
                (>= 10 * 1024 * 1024) and (< 1024 * 1024 * 1024) => 100000,
                >= 1024 * 1024 * 1024 => 10000000,
            };
            Random rng = new Random();
            byte[] buffer = new byte[sizeof(Int32) * portion];
            int data;
            for (int i = 0; i < (int)(integersAmount / portion); i++)
            {
                for (int j = 0; j < portion; j++)
                {
                    data = rng.Next(Int32.MinValue, Int32.MaxValue);
                    buffer[4 * j - 3] = (byte)data;
                    buffer[4 * j - 2] = (byte)(data >> 8);
                    buffer[4 * j - 1] = (byte)(data >> 0x10);
                    buffer[4 * j] = (byte)(data >> 0x18);
                }
                writer.Write(buffer);
            }
            
            int lastPortionLength = (int)(integersAmount - ((integersAmount / portion) * portion));
            buffer = new byte[lastPortionLength];
            for (int k = 0; k < lastPortionLength; k++)
            {
                data = rng.Next(Int32.MinValue, Int32.MaxValue);
                buffer[4 * k - 3] = (byte)data;
                buffer[4 * k - 2] = (byte)(data >> 8);
                buffer[4 * k - 1] = (byte)(data >> 0x10);
                buffer[4 * k] = (byte)(data >> 0x18);
            }
            writer.Write(buffer);
        }
    }
}