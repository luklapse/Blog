﻿using System;
using System.Linq;
using LinkDotNet.Blog.Domain;
using LinkDotNet.Blog.TestUtilities;

namespace LinkDotNet.Blog.UnitTests.Domain;

public class BlogPostTests
{
    [Fact]
    public void ShouldUpdateBlogPost()
    {
        var blogPostToUpdate = new BlogPostBuilder().Build();
        blogPostToUpdate.Id = "random-id";
        var blogPost = BlogPost.Create("Title", "Desc", "Content", "Url", true, previewImageUrlFallback: "Url2");
        blogPost.Id = "something else";

        blogPostToUpdate.Update(blogPost);

        blogPostToUpdate.Title.Should().Be("Title");
        blogPostToUpdate.ShortDescription.Should().Be("Desc");
        blogPostToUpdate.Content.Should().Be("Content");
        blogPostToUpdate.PreviewImageUrl.Should().Be("Url");
        blogPostToUpdate.PreviewImageUrlFallback.Should().Be("Url2");
        blogPostToUpdate.IsPublished.Should().BeTrue();
        blogPostToUpdate.Tags.Should().BeNullOrEmpty();
    }

    [Fact]
    public void ShouldUpdateTagsWhenExisting()
    {
        var blogPostToUpdate = new BlogPostBuilder().WithTags("tag 1").Build();
        blogPostToUpdate.Id = "random-id";
        var blogPost = new BlogPostBuilder().WithTags("tag 2").Build();
        blogPost.Id = "something else";

        blogPostToUpdate.Update(blogPost);

        blogPostToUpdate.Tags.Should().HaveCount(1);
        blogPostToUpdate.Tags.Single().Content.Should().Be("tag 2");
    }

    [Fact]
    public void ShouldTrimWhitespacesFromTags()
    {
        var blogPost = BlogPost.Create("Title", "Sub", "Content", "Preview", false, tags: new[] { " Tag 1", " Tag 2 ", });

        blogPost.Tags.Select(t => t.Content).Should().Contain("Tag 1");
        blogPost.Tags.Select(t => t.Content).Should().Contain("Tag 2");
    }

    [Fact]
    public void ShouldSetDateWhenGiven()
    {
        var somewhen = new DateTime(1991, 5, 17);

        var blog = BlogPost.Create("1", "2", "3", "4", false, somewhen);

        blog.UpdatedDate.Should().Be(somewhen);
    }

    [Fact]
    public void ShouldNotDeleteTagsWhenSameReference()
    {
        var bp = new BlogPostBuilder().WithTags("tag 1").Build();

        bp.Update(bp);

        bp.Tags.Should().HaveCount(1);
        bp.Tags.Single().Content.Should().Be("tag 1");
    }
}