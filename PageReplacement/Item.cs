namespace PageReplacement
{
    public class Item
    {
        public static int DEF_VALUE = int.MinValue;

        public int index = -1;//被置换的index
        public int[] value; //当前的内存值，默认值是DEF_VALUE
        public bool change = false; //是否置换
        public bool exist = false; //是否已存在

        public Item(Item item)
        {
            index = item.index;
            change = item.change;
            if (item.value != null)
            {
                value = (int[])item.value.Clone();
            }
        }

        public Item(int len)
        {
            value = new int[len];
            for (int i = 0; i < len; i++)
            {
                value[i] = DEF_VALUE;
            }
        }
    }
}
