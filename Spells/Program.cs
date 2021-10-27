﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spells
{
    class Program
    {
        static void Main(string[] args)
        {
            //1) Вывести значения Label и ImpactScript для всех спеллов, которые принадлежат к классу Paladin
            //2) Создать отдельный файл с таким же заголовком, как в изначальном файле(первые 3 строки одинаковые), в котором будут только спеллы со значением Acid в столбце ImmunityType
            //3) Создать отдельные файлы, в которых спеллы будут отфильтрованы по классам Bard, Cleric, Druid, Paladin, Ranger, Wiz_Sorc. В одном файле все спеллы класса Bard, в другом -все спеллы класса Cleric и тд

            /*
             Итак, если смотреть файлик, то там куча столбцов, разделенных пробелами. Мы можем получить массив этих столбцов, если используем функцию Split для строки, в качестве разделителя нужен пробел
             В каждой строке идет множество информации по одному спеллу, каждое значение столбца идет в одно слово, если значения нет, то в столбце указаны звездочки ****
             Выходит, по заданию 1, нужно вывести данные о строках, в которых столбец Paladin имеет значение, отличное от ****
             В задании 2 нужно сравнить значение столбца на равенство с строкой Acid, и все такие строки скопировать в отдельный массив и сохранить его в отдельный файл
             А 3 задание - микс 1 и 2, находим отдельные спеллы, пихаем в отдельные массивы. Так как файлов надо создать несколько, а процедура поиска по сути одинаковая, то лучше это оформить отдельным методом с параметрами
             * */

            // Первым делом надо прочесть файл, нам нужны все строки по отдельности
            // файл spells.2da я скопировал в папку Bin\Debug проекта, поэтому нет необходимости писать какой-то специальный путь
            string[] rows = File.ReadAllLines("spells.2da");

            // теперь в массиве rows находятся все строки из файла, найдем те, которые нужны по заданию 1
            // создадим разделитель для строк, нам нужен разделитель в виде массива символов, посколько вместе с ним мы сможем использовать опцию удаление пустых ячеек при разделении строки
            char[] splitter = new char[] { ' ' };

            // нам нет нужды обрабатывать первые 3 строки, они информационные и не содержат информации о спеллах, поэтому пока их можно пропустить
            for (int i = 3; i < rows.Length - 1; i++) // перебираем индексы для всех строк, кроме первых 3х
            {
                // теперь мы можем обратиться к каждой строке по ее индексу, разобьем ее на отдельные строки со значениями столбцов
                string[] cols = rows[i].Split(splitter, StringSplitOptions.RemoveEmptyEntries);// аргумент StringSplitOptions.RemoveEmptyEntries удалит пустые ячейки, которые образуются из-за множества последовательностей из пробелов
                // теперь массив cols содержит отдельные значения по всем столбцам, нам нужен столбец Paladin, путем сгибания пальцев и пристального взора в текстовый файл мы можем посчитать его индекс, это число 13. по сути это 14ый столбец с информацией, индексация с 0, поэтому 13
                if (cols.Length > 13 && cols[13] != "****") // так как в файле в конце есть пустая строка, то добавим условие перед обращением к индексу, что кол-во ячеек подходящее
                {
                    // итак, если столбец Paladin имеет значение, отличное от звездочек, значит там число, выводим данные по спеллу. Label - 2 столбец (индекс 1), ImpactScript - 10 столбец (индекс 9)
                    Console.WriteLine($"{cols[1]} {cols[9]}");// это ответ к 1 заданию!
                }
            }

            // теперь нам надо создать второй файл, нам нужны первые 3 строки для заголовка
            // можно создать коллекцию, заполнить ее нужными строками, а потом записать их все в файл через File.WriteAllLines
            // или можно создать файл и заполнять его построчно, причем это можно делать через File.AppendAllLines или через FileStream
            // давайте задание 2 сделаем через File.AppendAllLines

            // получим первые 3 строки для заголовка файла
            var header = rows.Take(3); // var - получение типа из контекста. Take возращает коллекцию данных с типом IEnumerable, для удобства работы можно ее преобразовать в List или в массив, но нам нужно будет происто записать эти 3 строки в файл, что мы и сделаем в следующей строке
            string file = "Acid.2da"; // переменная с названием нового файла, создаем, чтобы не писать каждый раз "Acid.2da"
            if (File.Exists(file)) // сначала проверяем, есть ли файл, мы его будем заполнять через дополнение строк, поэтому удостоверимся, что файла нет, чтобы не дописывать его
                File.Delete(file); // удалим файл, если он уже существует
            File.AppendAllLines(file, header); // если файл не существует, то будет создан. Если создан, то строки добавятся в конец файла

            // нам нужен столбец ImmunityType, он где-то посередине файла, считать самому уже лень, давайте воспользуемся функцией Split и поиском строки в массиве
            // разобьем на подстроки строку с заголовком столбцов:
            var headerCols = header.Last().Split(splitter, StringSplitOptions.RemoveEmptyEntries).ToList(); // в коллекции List есть метод IndexOf
            // получим нужный нам индекс c поправкой на то, что для первого столбца нет заголовка
            int immunityTypeIndex = headerCols.IndexOf("ImmunityType") + 1;
            // найдем нужные нам строки
            for (int i = 3; i < rows.Length; i++) // перебираем индексы для всех строк, кроме первых 3х
            {
                string[] cols = rows[i].Split(splitter, StringSplitOptions.RemoveEmptyEntries); // разделяем строку на столбцы
                if (cols.Length > immunityTypeIndex && cols[immunityTypeIndex] == "Acid") //выбираем только нужные строки, тут все аналогично
                    File.AppendAllText(file, rows[i] + "\n"); // AppendAllText не добавляет символ перехода на новую строку, поэтому добавляем его сами
            }// файл будет готов после выполнения этого цикла

            // 3 задание, сделаем его с помощью отдельного метода с параметром, а так принцип точно такой же
            // у нас есть несколько заголовков, они идут подряд Bard   Cleric   Druid   Paladin   Ranger   Wiz_Sorc
            // Значение в колонке Bard имеет индекс 10, соответственно остальные 11, 12, 13, 14, 15
            // выполняем несколько вызовов этого метода с разными аргументами
            CreateFileFilterByColumn(rows, 10, "Bard.2da");
            CreateFileFilterByColumn(rows, 11, "Cleric.2da");
            CreateFileFilterByColumn(rows, 12, "Druid.2da");
            CreateFileFilterByColumn(rows, 13, "Paladin.2da");
            CreateFileFilterByColumn(rows, 14, "Ranger.2da");
            CreateFileFilterByColumn(rows, 15, "Wiz_Sorc.2da");

        }

        static void CreateFileFilterByColumn(string[] rowsFile, int indexClass, string fileName)
        {
            // давайте сформируем коллекцию с нужными данными и запишем ее в файл через File.WriteAllLines
            List<string> newFileData = new List<string>();
            var header = rowsFile.Take(3); // заголовок, первые 3 строки везде одинаковые
            newFileData.AddRange(header); // первым идет заголовок
            // дальше знакомая конструкция, разделитель, цикл и разбитие каждой строки на подстроки
            char[] splitter = new char[] { ' ' };
            for (int i = 3; i < rowsFile.Length; i++)
            {
                var cols = rowsFile[i].Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                if (cols.Length > indexClass && cols[indexClass] != "****")
                    newFileData.Add(rowsFile[i]); // если в строке столбец с нужным классом имеет значение, то добавляем эту строку в новую коллекцию
            }

            File.WriteAllLines(fileName, newFileData); // создаем новый файл со всеми нужными строками
        }

        // The end
    }
}
