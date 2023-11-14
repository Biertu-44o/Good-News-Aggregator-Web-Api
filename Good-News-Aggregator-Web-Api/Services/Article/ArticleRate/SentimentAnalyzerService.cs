using Core.DTOs.Article;
using IServices.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using Services.Article.ArticleRate.jsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Article.ArticleRate
{
    public class SentimentAnalyzerService: ISentimentAnalyzerService
    {
        readonly IConfiguration _configuration;
        delegate Task<Double> ArticleAnalyzerMethods(FullArticleDto article);
        const double currentMin = 2;
        const double currentMax = 8;
        const double newMin = 1;
        const double newMax = 10;


        public SentimentAnalyzerService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new NullReferenceException(nameof(configuration));
        }


        public async Task<List<FullArticleDto>> GetArticlesWithSentimentScore(List<FullArticleDto> fullArticles)
        {
            var articleSentimentAnalyzerDelegate = new List<ArticleAnalyzerMethods>();
            var articlesWithRate = new List<FullArticleDto>();

            try
            {
                if (Convert.ToBoolean(
                        _configuration["ArticleSentimentAnalyzerMethods:DictionaryAnalyzer"]))
                {
                    articleSentimentAnalyzerDelegate.Add(GetRateArticleBySentimentDictionary);
                }

                if (Convert.ToBoolean(
                        _configuration["ArticleSentimentAnalyzerMethods:MachineAnalyzer"]))
                {
                    articleSentimentAnalyzerDelegate.Add(GetRateArticleBySentimentDetect);
                }
            }
            catch (FormatException)
            {
                throw new ArgumentException("Can't read ArticleSentimentAnalyzerMethods  from configuration file");
            }



            Double totalRate = 0;
            Int32 totalMarks = 0;

            foreach (var article in fullArticles)
            {
                try
                {

                    var tasks = articleSentimentAnalyzerDelegate
                        .Select(method => method(article));

                    Double[] rates = await Task.WhenAll(tasks);

                    foreach (var rate in rates)
                    {
                        if (rate != 0)
                        {
                            totalRate += rate;
                            totalMarks++;
                        }

                    }

                    article.PositiveRate = Math.Round(((totalRate / totalMarks * 10 - currentMin)
                        * (newMax - newMin) / (currentMax - currentMin)) + newMin, 1);

                    totalRate = 0;
                    totalMarks = 0;

                    if (!Double.IsNaN(article.PositiveRate))
                    {
                        article.FirstRate = Math.Round(rates[0], 1);
                        article.SecondRate = Math.Round(rates[1], 1);
                        articlesWithRate.Add(article);
                    }

                }
                catch (Exception ex)
                {
                    Log.Warning("something went wrong when evaluating the article {0} : {1}", article.Id, ex);
                }

            }

            return articlesWithRate;
        }

        public async Task<Double> GetRateArticleBySentimentDictionary(FullArticleDto article)
        {
            String text = PrepareText(article.Title +" " + article.ShortDescription +" "+ article.FullText);

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=4ed954dd519bd8359b7c4c8759b33c01d1eb1822"))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");

                    request.Content = new StringContent("[ { \"text\" : \"" + text + "\" } ]");
                    
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        Dictionary<String, Int32?>? articlesSentiment;

                        var responseString = await response.Content.ReadAsStringAsync();

                        var lemmas = (JsonConvert.DeserializeObject<RootObject1[]>(responseString))
                            .SelectMany(root => root.Annotations.Lemma).Select(lemma => lemma.Value).ToArray();

                        using (var jsonReader = new StreamReader(@"C:\Users\User\Desktop\Good-News-Aggregator-Web-Api\Good-News-Aggregator-Web-Api\Services\Article\ArticleRate\AFINN-ru.json"))
                        {
                            var json = await jsonReader.ReadToEndAsync();

                            articlesSentiment = JsonConvert.DeserializeObject<Dictionary<String, Int32?>>(json);
                        }

                        if (lemmas.Any())
                        {
                            Double? totalRate = 0;
                            Int32 articleSentimentFound = 0;
                            foreach (var lemma in lemmas)
                            {
                                if (articlesSentiment != null && articlesSentiment.ContainsKey(lemma))
                                {
                                    totalRate += articlesSentiment[lemma];
                                    articleSentimentFound++;
                                }
                            }

                            return Convert.ToDouble(totalRate) / (articleSentimentFound * 10);

                        }
                    }
                    else
                    {
                        Log.Warning("Bad response from ispras API:{0},{1}", response.StatusCode, article.Id);
                    }

                }
            }

            return 0;
        }

        public async Task<Double> GetRateArticleBySentimentDetect(FullArticleDto article)
        {
            Double totalRate = 0;
            
            var text = Regex.Replace(PrepareText(article.Title + " " + article.ShortDescription + " " + article.FullText)
                , "(?<=^.{1000}).*", "");

            var translatedText = await TranslateText(text);

            if (!String.IsNullOrEmpty(translatedText))
            {
                using (HttpClient client = new HttpClient())
                {
                    var parameters = new Dictionary<string, string>
                    {
                        { "text",  translatedText},
                    };
                    var content = new FormUrlEncodedContent(parameters);

                    var response = await client.PostAsync("http://text-processing.com/api/sentiment/", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                        totalRate = JsonConvert.DeserializeObject<RootObject3>(responseString).probability.pos;
                    }
                    else
                    {
                        Log.Warning("Bad response from text-processing API:{0},{1}", response.StatusCode, article.Id);
                    }
                }
            }

            return totalRate;
        }

        public async Task<String> TranslateText(String text)
        {

            if (!String.IsNullOrEmpty(text))
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET")
                               , "https://api.mymemory.translated.net/?de=demehchik.maks@mail.ru&q=" + text + "&langpair=ru|en"))
                    {
                        var response = await httpClient.SendAsync(request);


                        if (response.IsSuccessStatusCode)
                        {

                            var responseString = await response.Content.ReadAsStringAsync();

                            if (responseString.Contains("INVALID LANGUAGE PAIR SPECIFIED"))
                            {
                                Log.Warning("Translated Api: INVALID LANGUAGE PAIR SPECIFIED {0}", text);
                                return null;
                            }

                            var translatedText = JsonConvert.DeserializeObject<RootObject2>(responseString)
                                .responseData
                                .translatedText;


                            return translatedText;
                        }

                    }
                }
            }
            return null;
        }


        private String PrepareText(String notPrepareText)
        {
            String prepareText = notPrepareText.Trim();

            prepareText = Regex.Replace(prepareText, "\\r?\\n", " ");
            prepareText = Regex.Replace(prepareText, @"\s+", " ");
            prepareText = Regex.Replace(prepareText, "&#\\b", " ");
            prepareText = Regex.Replace(prepareText, "<.*?>", "");

            return prepareText;
        }
    }
}
