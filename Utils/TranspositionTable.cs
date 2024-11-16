/*
using System.Numerics;

namespace Utils.Storage
{
    enum EntryFlag
    {
        LowerBound,
        Exact,
        UpperBound
    }

    struct TableEntry(int depth, int score, EntryFlag flag)
    {
        public int depth = depth;
        public int score = score;
        public EntryFlag flag = flag;
    }

    struct TranspositionTable
    {
        int capacity;
        TableEntry?[] entries;

        public TranspositionTable()
        {
            // 32MB table by default
            entries = new TableEntry?[32768];
        }

        public TranspositionTable(int capacity)
        {
            if (BitOperations.PopCount((uint)capacity) != 1)
            {
                throw new Exception("invalid capacity value - not a power of two.");
            }

            entries = new TableEntry?[capacity];
        }

        public TableEntry? Get(ulong key)
        {
            return entries[(int)key & (capacity - 1)];
        }

        public void Insert(ulong key, int depth, int score, EntryFlag flag)
        {
            entries[(int)key & (capacity - 1)] = new(depth, score, flag);
        }
    }
}
*/