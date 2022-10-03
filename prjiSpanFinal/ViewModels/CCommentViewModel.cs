using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CCommentViewModel
    {
        private Comment _comment;
        public Comment comment
        {
            get { return _comment; }
            set { _comment = value; }
        }
        public CCommentViewModel()
        {
            _comment = new Comment();
        }
        public int ProductId 
        {
            get { return _comment.ProductId; }
            set { _comment.ProductId = value; }
        }
        public int MemberId
        {
            get { return _comment.ProductId; }
            set { _comment.ProductId = value; }
        }
        public string Comment1
        {
            get { return _comment.Comment1; }
            set { _comment.Comment1 = value; }
        }
        public byte Star
        {
            get { return _comment.Star; }
            set { _comment.Star = value; }
        }
        public int CommentId
        {
            get { return _comment.CommentId; }
            set { _comment.CommentId = value; }
        }
    }
}
