using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hash
{
    class HashTable
    {
        public class KeyValuePair
        {
            public object Key { get; set; }
            public object Value { get; set; }
        }

        public static List<List<KeyValuePair>> hashTable;
        public static List<int> hash;

        /// <summary>
        /// Конструктор контейнера
        /// summary>
        /// size">Размер хэ-таблицы
        public static void NewHashTable(int size)
        {
            hashTable = new List<List<KeyValuePair>>(size);
            hash = new List<int>(size);
            for (int i = 0; i < size; i++)
            {
                hashTable.Add(new List<KeyValuePair>());
            }
        }

        ///
        /// Метод складывающий пару ключ-значение в таблицу
        /// 
        /// key">
        /// value">
        public static void PutPair(object key, object value)
        {
            var index = SearchHash(GetHashCode(key));
            var keyValue = new KeyValuePair { Key = key, Value = value };
            if (index == -1 && hash.Count < hashTable.Count)
            {
                hash.Add(GetHashCode(key));
                index = SearchHash(GetHashCode(key));
                hashTable[index].Add(keyValue);
                return;
            }
            foreach (var e in hashTable[index])
            {
                if (e.Key.Equals(key))
                    e.Value = value;
            }
        }

        public static int SearchHash(int bucket)
        {
            return hash.IndexOf(bucket);
        }

        /// <summary>
        /// Поиск значения по ключу
        /// summary>
        /// key">Ключь
        /// <returns>Значение, null если ключ отсутствуетreturns>
        public static object GetValueByKey(object key)
        {
            var index = SearchHash(GetHashCode(key));
            if (index == -1)
                return null;
            foreach (var e in hashTable[index])
            {
                if (e.Key.Equals(key))
                    return e.Value;
            }
            return null;
        }

        public static int GetHashCode(object obj)
        {
            return Math.Abs(obj.GetHashCode()) % (hashTable.Count);
        }

        static void Main(string[] args)
        {
            ThreeElementsTest();
            SameValueElements();
            TenThousandsElements();
            OneThousandMissedElements();
            Console.ReadKey();
        }

        //Тест на добавление трёх элементов, поиск трёх элементов
        public static void ThreeElementsTest()
        {
            var check = -1;
            var size = 3;
            NewHashTable(size);
            KeyValuePair[] array = new KeyValuePair[size];

            array[0] = new KeyValuePair { Key = "1", Value = "один" };
            array[1] = new KeyValuePair { Key = "2", Value = "два" };
            array[2] = new KeyValuePair { Key = "3", Value = "три" };

            PutPair("1", "один");
            PutPair("2", "два");
            PutPair("3", "три");

            for (int i = 0; i < size; i++)
            {
                if (array[i].Value != GetValueByKey(array[i].Key))
                {
                    check = i;
                    break;
                }
            }

            if (check != -1)
                Console.WriteLine("Значение {0} из массива не было добавлено в хештаблицу, на его месте находится {1}", check + 1, GetValueByKey(array[check].Key));
            else Console.WriteLine("Проверка на добавление 3х элементов и их последующий поиск пройдена");
        }

        //Тест на добавление одного и того же ключа дважды с разными значениями сохраняет последнее добавленное значение
        public static void SameValueElements()
        {
            var size = 1;
            NewHashTable(size);
            var keyValue = new KeyValuePair { Key = 1, Value = "два"};

            PutPair(1,"один");
            PutPair(1, "два");
            var value = GetValueByKey(1);

            if (!value.Equals(keyValue.Value))
                Console.WriteLine("Вместо значения '{0}' по ключу {1} было полученно значение '{2}'", keyValue.Value, keyValue.Key, value );
            else Console.WriteLine("Проверка на добавление элементов с одинаковыми ключами пройдена");
        }

        //Тест на добавление 10000 элементов в структуру и поиск одного из них
        public static void TenThousandsElements()
        {
            var size = 10000;
            NewHashTable(size);

            var keyValue = new KeyValuePair { Key = 255, Value = "255number" };

            for (int i = 0; i < size; i++)
            {
                PutPair(i, i + "number");
            }

            var value = GetValueByKey(255);

            if (!value.Equals(keyValue.Value))
                Console.WriteLine("Вместо значения '{0}' по ключу {1} было полученно значение '{2}'", keyValue.Value, keyValue.Key, value);
            else Console.WriteLine("Проверка на создание структуры с 10000 элементов и поиском одного из них пройдена");
        }

        //Тест на добавление 10000 элементов в структуру и поиск 1000 недобавленных ключей, поиск которых должен вернуть null
        public static void OneThousandMissedElements()
        {
            var size = 10000;
            NewHashTable(size);          

            for (int i = 0; i < size; i++)
            {
                PutPair(i, i + "number");
            }

            var check = -1;

            for (int i = size; i < size + 1000; i++)
            {
                var value = GetValueByKey(i);
                if (value != null)
                {
                    check = 1;
                    break;
                }                    
            }

            if (check != -1)
                Console.WriteLine("Было возвращено значение не Null");
            else Console.WriteLine("Проверка на создание структуры с 10000 элементов и поиском 1000 недобавленных пройдена");
        }
    }
}
