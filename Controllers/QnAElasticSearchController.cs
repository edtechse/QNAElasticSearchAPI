using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using QnAElasticSearchService.Helper;
using QnAElasticSearchService.Queries;
using QnAElasticSearchService.ResponseModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QnAElasticSearchService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class QnAElasticSearchController : ControllerBase
    {
        private IConfiguration _configuration;
        JsonConfiguration _jsonconfiguration;
        ResponseTraversal _responseTraversal;

        public QnAElasticSearchController(IConfiguration iConfig)
        {
            _configuration = iConfig;
            _jsonconfiguration = new JsonConfiguration(_configuration);
            _responseTraversal = new ResponseTraversal();
        }

        [HttpGet("autoComplete/{text}")]
        public async Task<IActionResult> GetAutoCompleteQnA(string text)
        {
            if (text == "" || text == null)
                return BadRequest(new Exception("Search string is incorrect"));
            using (var httpClient = new HttpClient())
            {
                var query = QnAQuery.AutoCompleteQuery(text);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + EncodedBasicValue());
                var response = await httpClient.PostAsync(_jsonconfiguration.GetUrl(), QueryContentForSendingData(query));
                var result = await response.Content.ReadAsStringAsync();

                JObject jObject = JObject.Parse(result);
                JArray arrays = (JArray)jObject["hits"]["hits"];
                List<string> responseObject = new List<string>();
                foreach (var array in arrays)
                {
                    var abc = array["_source"]["questionTitle"]["S"].ToString();
                    responseObject.Add(abc);
                }

                return Ok(responseObject);
            }
        }

        [HttpGet("search/{text}/{page}")]
        public async Task<IActionResult> GetSearchBlog(string text, int page)
        {
            if (text == "" || text == null)
                return BadRequest(new Exception("Search string is incorrect"));
            using (var httpClient = new HttpClient())
            {
                var query = QnAQuery.SearchQNAQuery(text, page);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + EncodedBasicValue());
                var response = await httpClient.PostAsync(_jsonconfiguration.GetUrl(), QueryContentForSendingData(query));
                var result = await response.Content.ReadAsStringAsync();

                JObject jObject = JObject.Parse(result);
                JArray arrays = (JArray)jObject["hits"]["hits"];
                List<QNA> responseObject = new List<QNA>();
                foreach (var array in arrays)
                {
                    responseObject.Add(_responseTraversal.SearchResponse(array));
                }
                return Ok(responseObject);
            }
        }

        [HttpGet("personalizedFeed")]
        public async Task<IActionResult> GetPersonalizedFeed([FromQuery] string text)
        {
            if (text == "" || text == null)
                return BadRequest(new Exception("Search string is incorrect"));
            var ReplaceCommaWithSpaceStringTags = text.Replace('\'', ' ');
            using (var httpClient = new HttpClient())
            {
                var query = QnAQuery.PersonalizedFeedQuery(ReplaceCommaWithSpaceStringTags);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + EncodedBasicValue());
                var response = await httpClient.PostAsync(_jsonconfiguration.GetUrl(), QueryContentForSendingData(query));
                var result = await response.Content.ReadAsStringAsync();

                var jObject = JObject.Parse(result);
                JArray arrays = (JArray)jObject["hits"]["hits"];

                List<QNA> responseObject = new List<QNA>();
                foreach (var array in arrays)
                {
                    responseObject.Add(_responseTraversal.PersonalizedFeedResponse(array));
                }
                return Ok(responseObject);
            }
        }


        [HttpGet("trendingBlogTags")]
        public async Task<IActionResult> TrendingBlogTags()
        {
            using (var httpClient = new HttpClient())
            {
                var query = QnAQuery.TrendingQNAQuery();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + EncodedBasicValue());
                var response = await httpClient.PostAsync(_jsonconfiguration.GetUrl(), QueryContentForSendingData(query));
                var result = await response.Content.ReadAsStringAsync();

                JObject jobject = JObject.Parse(result);
                JArray arrays = (JArray)jobject["aggregations"]["Trendings"]["buckets"];

                List<string> responseObject = new List<string>();
                foreach (var array in arrays)
                {
                    var responsestr = array["key"].ToString();
                    responseObject.Add(responsestr);
                }
                return Ok(responseObject);
            }
        }

        private string EncodedBasicValue()
        {
            var authDetail = _jsonconfiguration.GetBasicAuthDetails();
            var plainTextBytes = Encoding.UTF8.GetBytes(authDetail.Item1 + ":" + authDetail.Item2);
            string encodedValue = Convert.ToBase64String(plainTextBytes);
            return encodedValue;
        }

        private StringContent QueryContentForSendingData(string query)
        {
            var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
            return stringContent;
        }

    }
}