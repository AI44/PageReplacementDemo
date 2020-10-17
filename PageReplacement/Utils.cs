using System;

namespace PageReplacement
{
    public class Utils
    {
        //传入一个数组,求出一个数组的最大值的位置
        public static int MaxIndex<T>(T[] arr) where T : IComparable<T>
        {
            if (arr != null && arr.Length > 0)
            {
                var pos = 0;
                var a = arr[0];
                for (var i = 1; i < arr.Length; i++)
                {
                    var b = arr[i];
                    if (b.CompareTo(a) > 0)
                    {
                        a = b;
                        pos = i;
                    }
                }
                return pos;
            }
            return -1;
        }

        //计算需要被置换的index（最佳置换算法）
        //思路：计算当前内存值 到 下一次出现相同值 的距离，距离越远的优先替换。
        public static int GetFarIndex<T>(T[] memData, T[] inputData, int startIndex) where T : IComparable<T>
        {
            if (memData == null || memData.Length < 1)
            {
                return -1;
            }
            if (inputData == null || inputData.Length < 1 || inputData.Length <= startIndex)
            {
                return 0;
            }

            int i = 0;
            int[] re = new int[memData.Length];
            foreach (T value in memData)
            {
                int index = Array.IndexOf(inputData, value, startIndex);
                if (index < 0)
                {
                    return i;
                }
                re[i] = index;

                i++;
            }

            int result = MaxIndex(re);
            if (result < 0)
            {
                result = 0;
            }
            return result;
        }

        //最佳置换算法（OPT）
        public static Item[] RunOPT(int max, int[] arr)
        {
            if (arr == null)
            {
                return null;
            }

            Item[] result = new Item[arr.Length];
            if (arr.Length > 0)
            {
                int per = -1;
                foreach (int value in arr)
                {
                    Item temp;
                    if (per > -1)
                    {
                        temp = new Item(result[per]);
                    }
                    else
                    {
                        temp = new Item(max);
                    }
                    result[per + 1] = temp;

                    //搜索是否有相同
                    int j = Array.IndexOf(temp.value, value);
                    if (j > -1)
                    {
                        //有相同
                        temp.index = j;
                        temp.change = false;
                        temp.exist = true;
                    }
                    else
                    {
                        //搜索是否有空位
                        int k = Array.IndexOf(temp.value, Item.DEF_VALUE);
                        if (k > -1)
                        {
                            temp.index = k;
                            temp.value[temp.index] = value;
                            temp.change = false;
                            temp.exist = false;
                        }
                        else
                        {
                            int index = GetFarIndex(temp.value, arr, per + 1);
                            temp.value[index] = value;
                            temp.index = index;
                            temp.change = true;
                            temp.exist = false;
                        }
                    }

                    per++;
                }
            }
            return result;
        }

        //先进先出页面置换算法（FIFO）
        public static Item[] RunFIFO(int max, int[] arr)
        {
            if (arr == null)
            {
                return null;
            }

            Item[] result = new Item[arr.Length];
            if (arr.Length > 0)
            {
                int per = -1;
                int pop = 0;
                foreach (int value in arr)
                {
                    Item temp;
                    if (per > -1)
                    {
                        temp = new Item(result[per]);
                    }
                    else
                    {
                        temp = new Item(max);
                    }
                    result[per + 1] = temp;

                    //搜索是否有相同
                    int j = Array.IndexOf(temp.value, value);
                    if (j > -1)
                    {
                        //有相同
                        temp.index = j;
                        temp.change = false;
                        temp.exist = true;
                    }
                    else
                    {
                        //push
                        int index = pop;
                        int oldValue = temp.value[index];
                        temp.value[index] = value;
                        temp.index = index;
                        temp.change = oldValue != Item.DEF_VALUE;
                        temp.exist = false;

                        pop++;
                        pop = pop % max;
                    }

                    per++;
                }
            }
            return result;
        }

        //最近最久未使用置换算法（LRU）
        public static Item[] RunLRU(int max, int[] arr)
        {
            if (arr == null)
            {
                return null;
            }

            Item[] result = new Item[arr.Length];
            if (arr.Length > 0)
            {
                LruCache<int, int?> cache = new LruCache<int, int?>(max);
                int per = -1;
                foreach (int value in arr)
                {
                    Item temp;
                    if (per > -1)
                    {
                        temp = new Item(result[per]);
                    }
                    else
                    {
                        temp = new Item(max);
                    }
                    result[per + 1] = temp;

                    int? re = cache.Put(value, value);
                    int index;
                    if (re != null)
                    {
                        //删除了一个
                        index = Array.IndexOf(temp.value, re);
                        temp.value[index] = value;
                        temp.index = index;
                        temp.change = true;
                        temp.exist = false;
                    }
                    else
                    {
                        //存在 或 有空位
                        index = Array.IndexOf(temp.value, value);
                        if (index > -1)
                        {
                            //有相同
                            temp.index = index;
                            temp.change = false;
                            temp.exist = true;
                        }
                        else
                        {
                            //搜索是否有空位
                            index = Array.IndexOf(temp.value, Item.DEF_VALUE);
                            temp.value[index] = value;
                            temp.index = index;
                            temp.change = false;
                            temp.exist = false;
                        }
                    }

                    per++;
                }
            }
            return result;
        }

        public static float GetMissRatio(Item[] arr)
        {
            if (arr != null && arr.Length > 0)
            {
                return (float)GetMissNum(arr) / (float)arr.Length;
            }

            return 0;
        }

        public static int GetMissNum(Item[] arr)
        {
            if (arr != null)
            {
                int total = 0;
                foreach (Item item in arr)
                {
                    if (!item.exist)
                    {
                        total++;
                    }
                }
                return total;
            }
            return 0;
        }

        public static int GetChangeNum(Item[] arr)
        {
            if (arr != null)
            {
                int total = 0;
                foreach (Item item in arr)
                {
                    if (item.change)
                    {
                        total++;
                    }
                }
                return total;
            }
            return 0;
        }
    }
}
