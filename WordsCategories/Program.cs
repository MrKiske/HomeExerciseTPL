using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WordsCategories
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            var targetDirectory = "..\\..\\..\\Files";
            Results result = new Results();

            string[] fileEntries = Directory.GetFiles(targetDirectory, "*.dat", SearchOption.AllDirectories);
            Task task1 = Task.Factory.StartNew(() => {
                foreach (string file in fileEntries)
                {

                    var fileContent = File.ReadAllLines(file).ToList();

                    //count words 
                    var words = fileContent.Count();
                    result.TotalWords += words;

                    //distinct words
                    var distinct = fileContent.Distinct().Count();
                    result.TotalDistinct += distinct;

                    //group words 
                    var groupW = (fileContent.GroupBy(w => w.Length)
                      .Select(w =>
                          new
                          {
                              key = (w.Key >= 0 && w.Key <= 5 ? "XS" :
                                        (w.Key >= 6 && w.Key <= 10 ? "S" :
                                        (w.Key >= 11 && w.Key <= 15 ? "M" :
                                         w.Key >= 16 && w.Key <= 20 ? "L" : "XL"))),
                              value = w.Count()
                          }
                       ).GroupBy(k => k.key)
                        .Select(t =>
                           new
                           {
                               key = t.Key,
                               value = t.Sum(p => p.value)
                           }
                    )).ToList();

                    //sum categories
                    result.TotalXS += !groupW.Any(w => w.key == "XS") ? 0 : groupW.Find(w => w.key == "XS").value;
                    result.TotalS += !groupW.Any(w => w.key == "S") ? 0 : groupW.Find(w => w.key == "S").value;
                    result.TotalM += !groupW.Any(w => w.key == "M") ? 0 : groupW.Find(w => w.key == "M").value;
                    result.TotalL += !groupW.Any(w => w.key == "L") ? 0 : groupW.Find(w => w.key == "L").value;
                    result.TotalXL += !groupW.Any(w => w.key == "XL") ? 0 : groupW.Find(w => w.key == "XL").value;

                }

                return result;
            }).ContinueWith(ant =>
            {
                Console.WriteLine($"Results:\n\n" +
                    $"Total words:{ant.Result.TotalWords}.\n" +
                    $"Total distinct words: {ant.Result.TotalDistinct}.\n" +
                    $"Total categories words XS: {ant.Result.TotalXS}.\n" +
                    $"Total categories words S: {ant.Result.TotalS}.\n" +
                    $"Total categories words M: {ant.Result.TotalM}.\n" +
                    $"Total categories words L: {ant.Result.TotalL}.\n" +
                    $"Total categories words XL: {ant.Result.TotalXL}.\n");
            }
            );

            task1.Wait();


        }
    }
}
