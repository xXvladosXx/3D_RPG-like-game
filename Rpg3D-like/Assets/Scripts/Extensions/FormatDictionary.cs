using System.Text;
using Resistance;

namespace Extensions
{
    public static class FormatDictionary
    {
        public static string Format(this SerializableDictionary<DamageType, float> e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var pair in e)
            {
                stringBuilder.Append(pair.Key.ToString());
                stringBuilder.Append(": ");
                stringBuilder.Append(pair.Value.ToString());
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
    }
}