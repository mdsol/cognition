using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;

namespace Cognition.Documents.Library
{
    public class KnowledgeBaseArticle : Document
    {
        public KnowledgeBaseArticle()
        {
            Type = "kb";
        }

        public override string GetFullName()
        {
            return "Knowledge Base Article";
        }

        public override string Title
        {
            get
            {
                return String.Format("KB{0} - {1}", ArticleNumber.GetValueOrDefault(), ArticleTitle);
            }
        }

        [Display(Name = "Article number")]
        [Required]
        public int? ArticleNumber { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Article title")]
        [Required]
        public string ArticleTitle { get; set; }

        [Display(Name = "Applies to")]
        public string AppliesTo { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Article")]
        public string ArticleBody { get; set; }

    }
}
