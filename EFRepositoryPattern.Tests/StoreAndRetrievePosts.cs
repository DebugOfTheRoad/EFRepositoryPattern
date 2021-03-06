using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using EFRepository;
using EFRepositoryPattern.Tests.Models;
using EFRepositoryPattern.Tests.Repositories;
using FluentAssertions;
using NUnit.Framework;

namespace EFRepositoryPattern.Tests
{
    [TestFixture]
    public class StoreAndRetrievePosts
    {
        private int CreatePost(string title, string text, DateTime published)
        {
            IPostRepository repo = new PostRepository(new BlogContext());

            var post = new Post
                           {
                               Title = title,
                               Text = text,
                               PublishDate = published
                           };

            return repo.Save(post);
        }

        [Test]
        public void StoreAndRetrievePost()
        {
            IPostRepository repo = new PostRepository(new BlogContext());

            var r = new Random();
            var title = "Post Title " + r.Next();
            var text = "Text of Post " + r.Next();
            var publishDate = new DateTime(2012, 1, 1).AddDays(r.Next(10));

            var id = CreatePost(title, text, publishDate);

            var post = repo.Retrieve(id);

            post.Title.Should().Be(title);
            post.Text.Should().Be(text);
            post.PublishDate.Should().Be(publishDate);
        }

        [Test]
        public void ExpectDataValidationException()
        {
            CommentRepository repo = new CommentRepository(new BlogContext());

            var comment = new Comment();
            comment.PostID = -1;
            comment.Text = "This shouldn't be saveable";

            Action save = () => repo.Save(comment);

            save.ShouldThrow<DataValidationException>();
        }
    }
}