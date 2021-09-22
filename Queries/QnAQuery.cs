using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QnAElasticSearchService.Queries
{
    public static class QnAQuery
    {
        public static string AutoCompleteQuery(string text)
        {
            return @"{ ""size"":5,""query"" : { ""wildcard"" : { ""questionTitle.S"" : { ""value"" : ""*" + text + @"*"" } } }, ""_source"" : [""questionTitle.S""] }";
        }
        public static string SearchQNAQuery(string text, int page)
        {
            return @"{  ""size"":10,""from"":" + page + @",""query"":{""bool"":{""must"":[{""match"": {""questionText.S"" :""" + text + @"""}}]}},""_source"":[""questionId"",""questionText.S"",""questionTitle.S"",""author.S"",""questionTimestamp.S""]}";
        }

        public static string TrendingQNAQuery()
        {
            return @"{  ""size"":1,""query"":{""bool"":{""filter"":[{""range"": {""questionTimestamp.S"":{""gte"": ""now-1M/M"", ""lte"":""now+5M/M""}}}]}},""_source"":[""questionId""],""aggs"":{""Trendings"":{""terms"":{""field"":""questionTag.SS.keyword"",""order"":{""_count"":""desc""},""size"":10}}}}";
        }

        public static string PersonalizedFeedQuery(string tags)
        {
            return @"{  ""size"":10,""query"":{""bool"":{""must"":[{""match"": {""questionTag.SS"" :""" + tags + @"""}}]}},""sort"": [{""questionTimestamp.S"": {""order"": ""desc""}}],""_source"":[""questionId"",""author.S"",""questionTitle.S"",""questionText.S"",""questionTimestamp.S"",""questionTag.SS""]}";
        }
    }
}
