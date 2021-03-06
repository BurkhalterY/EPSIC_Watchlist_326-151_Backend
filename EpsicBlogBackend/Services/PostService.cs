﻿using EpsicBlogBackend.Data;
using EpsicBlogBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EpsicBlogBackend.Services
{
    public class PostService : IPostService
    {
        private readonly BlogDataContext _context;

        public PostService(BlogDataContext context)
        {
            _context = context;
        }

        public Post Add(Post post)
        {
            post.Date = DateTime.Now;
            _context.Posts.Add(post);
            _context.SaveChanges();
            return post;
        }

        public void Delete(int id)
        {
            _context.Posts.Remove(GetSingle(id));
            _context.SaveChanges();
        }

        public bool ExistsById(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        public List<Post> GetAll()
        {
            return _context.Posts
                .Include(i => i.Author)
                .Include(i => i.Comments)
                .ToList();
        }

        public List<Comment> GetComments(int id)
        {
            return _context.Comments.Include(i => i.Author).Where(x => x.PostId == id).ToList();
        }

        public Post GetSingle(int id)
        {
            return _context.Posts
                .Include(i => i.Author)
                .Include(i => i.Comments)
                .FirstOrDefault(e => e.Id == id);
        }

        public Post Update(int id, Post model)
        {
            var post = GetSingle(id);

            post.Title = model.Title;
            post.Date = model.Date;
            post.Content = model.Content;
            post.AuthorId = model.AuthorId;

            _context.SaveChanges();
            return post;
        }
    }
}
