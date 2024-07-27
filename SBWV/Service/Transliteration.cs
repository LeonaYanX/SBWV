namespace SBWV.Service
{
    public class Transliteration
    {

        private readonly Dictionary<string, string> _transliterationTable = new Dictionary<string, string>
    {
        {"a", "а"}, {"b", "б"}, {"c", "ц"}, {"d", "д"}, {"e", "е"}, {"f", "ф"}, {"g", "г"},
        {"i", "и"}, {"j", "й"}, {"k", "к"}, {"l", "л"}, {"m", "м"}, {"n", "н"},
        {"o", "о"}, {"p", "п"}, {"q", "к"}, {"r", "р"}, {"s", "с"}, {"t", "т"}, {"u", "у"},
        {"v", "в"}, {"w", "в"}, {"x", "кс"}, {"z", "з"},{"dg","дж" },{"gj","дж"},
        {"ch", "ч"}, {"sh", "ш"}, {"sch", "щ"}, {"yu", "ю"}, {"ya", "я"}, {"zh", "ж"},
        {"yo", "ё"}, {"ye", "е"}, {"yi", "и"}, {"kh", "х"}, {"ts", "ц"}, {"ee", "и"},
        {"ju", "ю"}, {"ja", "я"}, {"jo", "ё"}, {"ij", "ий"}, {"ks", "кс"}, {"iy", "ий"},
        {"h", "х"}, {"th", "т"}, {"ph", "ф"}, {"y", "ы"}
    };
        // todo delate if not need
        /*public string TransliterateENToRU(string input)
        {
            var result = new StringBuilder();
            int i = 0;
            while (i+1 < input.Length)
            {
               

                // Проверяем для комбинаций символов
                if (i + 1 < input.Length)
                {
                    string twoCharKey = input.Substring(i, 2).ToLower();
                    if (_transliterationTable.TryGetValue(twoCharKey, out var transliteratedTwoChar))
                    {
                        result.Append(transliteratedTwoChar);
                        i += 2;
                        continue;
                    }
                }

 

                // Проверяем для одиночных символов
                string oneCharKey = input.Substring(i, 1).ToLower();
                if (_transliterationTable.TryGetValue(oneCharKey, out var transliteratedChar))
                {
                    result.Append(transliteratedChar);
                }
                else
                {
                    result.Append(input[i]); // Оставляем символ как есть, если нет соответствия
                }
                i++;
            }
            return result.ToString();
        }*/
    }





}
