using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QnAElasticSearchService.ResponseModel;
using System;
using System.Collections.Generic;

namespace QnAElasticSearchService.Helper
{
    public class ResponseTraversal
    {
        public QNA SearchResponse(JToken responseItem)
        {
            QNA _qna = new QNA();
            _qna.Id = responseItem["_source"]["questionId"]["S"].ToString();
            _qna.QuestionAuthor = responseItem["_source"]["author"]["S"].ToString();
            _qna.QuestionText = responseItem["_source"]["questionText"]["S"].ToString();
            _qna.QuestionTitle = responseItem["_source"]["questionTitle"]["S"].ToString();
            _qna.QuestionCreationDate = responseItem["_source"]["questionTimestamp"]["S"].ToString();
            return _qna;
        }

        public QNA PersonalizedFeedResponse(JToken responseItem)
        {
            QNA _qna = new QNA();
            _qna.Id = responseItem["_source"]["questionId"]["S"].ToString();
            _qna.QuestionAuthor = responseItem["_source"]["author"]["S"].ToString();
            _qna.QuestionText = responseItem["_source"]["questionText"]["S"].ToString();
            _qna.QuestionTitle = responseItem["_source"]["questionTitle"]["S"].ToString();
            _qna.QuestionCreationDate = responseItem["_source"]["questionTimestamp"]["S"].ToString();

            var TagList = JsonConvert.DeserializeObject<List<string>>(responseItem["_source"]["questionTag"]["SS"].ToString());
            _qna.QuestionTags = String.Join(",", TagList);
            return _qna;
        }
    }
}
