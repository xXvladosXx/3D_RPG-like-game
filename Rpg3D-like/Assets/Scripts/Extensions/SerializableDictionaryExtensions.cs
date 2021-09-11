namespace Extensions
{
    public static class SerializableDictionaryExtensions
    {
        public static SerializableDictionary<Tkey, Tvalue> ShallowClone<Tkey, Tvalue>(
            this SerializableDictionary<Tkey, Tvalue> D)
        {
            var nDict = new SerializableDictionary<Tkey, Tvalue>();
            foreach (var pair in D)
            {
                nDict.Add(new SerializableDictionary<Tkey, Tvalue>.Pair(pair.Key, pair.Value));
            }

            return nDict;
        }
    }
}