﻿using SimpleBlog2.Infrastructure;
using SimpleBlog2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleBlog2.Areas.Admin.ViewModels
{

    public class TagCheckBox
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool isChecked { get; set; }
    }

    public class PostsIndex
    {
        public PageData<Post> Posts { get; set; }
    }

    public class PostsForm
    {
        public bool isNew { get; set; }
        public int? postId { get; set; }
        
        [Required,MaxLength(128)]
        public string Title { get; set; }
        
        [Required, MaxLength(128)]
        public string Slug { get; set; }
        
        [Required,DataType(DataType.MultilineText)]
        public string Content { get; set; }


        public IList<TagCheckBox> Tags { get; set; }
    }
}