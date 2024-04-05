using System;
using System.Collections.Generic;

namespace PDSA_Games
{
    public static class SearchAlgorithms
    {
        public static int BinarySearch(List<int> list, int target)
        {
            int left = 0;
            int right = list.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (list[mid] == target)
                    return mid;

                if (list[mid] < target)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return -1; // Not found
        }

        public static int JumpSearch(List<int> list, int target)
        {
            int n = list.Count;
            int step = (int)Math.Sqrt(n);
            int prev = 0;

            while (list[Math.Min(step, n) - 1] < target)
            {
                prev = step;
                step += (int)Math.Sqrt(n);
                if (prev >= n)
                    return -1; // Not found
            }

            while (list[prev] < target)
            {
                prev++;
                if (prev == Math.Min(step, n))
                    return -1; // Not found
            }

            if (list[prev] == target)
                return prev;

            return -1; // Not found
        }

        public static int ExponentialSearch(List<int> list, int target)
        {
            int n = list.Count;
            if (list[0] == target)
                return 0;

            int i = 1;
            while (i < n && list[i] <= target)
                i *= 2;

            return BinarySearch(list.GetRange(i / 2, Math.Min(i, n) / 2), target);
        }

        public static int FibonacciSearch(List<int> list, int target)
        {
            int fibPrev = 0;
            int fibCurr = 1;
            int fibNext = fibPrev + fibCurr;

            while (fibNext < list.Count)
            {
                fibPrev = fibCurr;
                fibCurr = fibNext;
                fibNext = fibPrev + fibCurr;
            }

            int offset = -1;
            while (fibNext > 1)
            {
                int i = Math.Min(offset + fibPrev, list.Count - 1);

                if (list[i] < target)
                {
                    fibNext = fibCurr;
                    fibCurr = fibPrev;
                    fibPrev = fibNext - fibCurr;
                    offset = i;
                }
                else if (list[i] > target)
                {
                    fibNext = fibPrev;
                    fibCurr -= fibPrev;
                    fibPrev = fibNext - fibCurr;
                }
                else
                {
                    return i;
                }
            }

            if (fibCurr == 1 && list[offset + 1] == target)
                return offset + 1;

            return -1; // Not found
        }
    }
}
