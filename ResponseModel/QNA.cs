using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QnAElasticSearchService.ResponseModel
{
    [Serializable]
    public class QNA
    {
        [JsonPropertyName("questionId")]
        public string Id { get; set; }

        [JsonPropertyName("questionTitle")]
        public string QuestionTitle { get; set; }

        [JsonPropertyName("questionText")]
        public string QuestionText { get; set; }

        [JsonPropertyName("questionTimestamp")]
        public string QuestionCreationDate { get; set; }

        [JsonPropertyName("questionTag")]
        public string QuestionTags { get; set; }

        [JsonPropertyName("author")]
        public string QuestionAuthor { get; set; }
    }

}
