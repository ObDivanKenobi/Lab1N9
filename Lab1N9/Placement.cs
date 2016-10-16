using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1N9
{
    class Placement
    {
        int MAX;
        int[] perm;
        public int Length { get { return perm.Length; } }

        public Placement(int length, int max)
        {
            MAX = max;
            perm = new int[length];
            for (int i = 0; i < length; ++i)
                perm[i] = 1;
        }

        public Placement(int[] array)
        {
            perm = array;
        }

        public bool NextLexicographical()
        {
            int N = perm.Length;
            int j = N - 1;
            while (j >= 0 && perm[j] == MAX) --j;
            if (j < 0) return false;
            if (perm[j] >= MAX) --j;
            ++perm[j];
            if (j == N - 1) return true;
            for (int i = j + 1; i < N; ++i)
                perm[i] = 1;
            return true;
        }

        public int this[int i]
        {
            get
            {
                return perm[i];
            }
        }
    }
}
