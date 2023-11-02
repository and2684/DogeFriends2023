using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace BreedsToJsonParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "https://hvost.news/animals/dogs-breeds/";

            var dogBreeds = new List<Breed>();

            using (var client = new HttpClient())
            {
                var html = await client.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var breedItems = doc.DocumentNode.SelectNodes("//a[@class='breeds-list-i']");

                if (breedItems != null)
                {
                    foreach (var breedItem in breedItems)
                    {
                        // Название породы
                        var breedName = breedItem.SelectSingleNode(".//span").InnerText;
                        // Картинка
                        var imgSrc = $"https://hvost.news{breedItem.SelectSingleNode(".//img").GetAttributeValue("data-src", "")}";
                        // Тип собаки
                        var breedGroups = breedItem.SelectSingleNode(".//div[@class='breeds-list-i__label']").InnerText.Trim();
                        // Cсылка на страницу с описанием
                        var href = $"https://hvost.news{breedItem.GetAttributeValue("href", "")}";
                        var description = await ParseDescription(href);
                        // Загрузка изображения и кодирование в base64
                        var imageBytes = await client.GetByteArrayAsync(imgSrc);
                        var base64Image = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

                        Console.WriteLine("Выгружаю породу: " + breedName);
                        Console.WriteLine(new string('-', 50));

                        dogBreeds.Add(new Breed
                        {
                            Name = breedName,
                            BreedGroups = breedGroups,
                            Description = description,
                            Base64Image = base64Image
                        });
                    }

                    // Сериализация в JSON
                    var json = JsonConvert.SerializeObject(dogBreeds, Formatting.Indented);
                    // Сохранение JSON в файл
                    await File.WriteAllTextAsync("Breeds.json", json);
                }
            }
        }

        public static async Task<string> ParseDescription(string url)
        {
            using var client = new HttpClient();
            var htmlText = await client.GetStringAsync(url);

            string pattern = @"<meta property=""og:description"" content=""([^""]+?)""\s*/>";
            Match match = Regex.Match(htmlText, pattern);

            if (match.Success)
            {
                var content = match.Groups[1].Value;
                var cleanedDescription = System.Net.WebUtility.HtmlDecode(content)
                                                              .Trim()
                                                              .Replace("\r", "")
                                                              .Replace("\n", "")
                                                              .Replace("\t", "");
                return cleanedDescription;
            }
            return string.Empty;
        }
    }
}