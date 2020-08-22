using System;
using System.Collections.Generic;
using ReadLater.Entities;

namespace ReadLater.Services
{
    public interface IBookmarkService
    {
        Bookmark CreateBookmark(Bookmark bookmark, Guid userId);
        List<Bookmark> GetBookmarks(string category, Guid userId);
        Bookmark GetBookmark(int Id);
        void UpdateBookmark(Bookmark bookmark);
        void DeleteBookmark(Bookmark bookmark);
        Bookmark BookmarkClicked(int id);
    }
}