using System;
using System.Collections.Generic;
using System.Linq;
using ReadLater.Entities;
using ReadLater.Repository;

namespace ReadLater.Services
{
    public class BookmarkService : IBookmarkService
    {
        protected IUnitOfWork _unitOfWork;

        public BookmarkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Bookmark CreateBookmark(Bookmark bookmark, Guid userId)
        {
            if (bookmark.CategoryId == null)
            {
                bookmark.CategoryId = 0;
                bookmark.Category.UserCreatedId = userId;
                bookmark.Category.ObjectState = ObjectState.Added;
            }
            else
            {
                bookmark.Category = null;
            }

            if (!bookmark.URL.StartsWith("http"))
            {
                bookmark.URL = "http://" + bookmark.URL;
            }
            bookmark.CreateDate = DateTime.Now;
            bookmark.UserCreatedId = userId;
            _unitOfWork.Repository<Bookmark>().Insert(bookmark);
            _unitOfWork.Save();
            return bookmark;
        }

        public List<Bookmark> GetBookmarks(string category, Guid userId)
        {
            if (string.IsNullOrEmpty(category))
            {
                return _unitOfWork.Repository<Bookmark>().Query()
                                                        .Filter(b => b.UserCreatedId == userId)
                                                        .OrderBy(l => l.OrderByDescending(b => b.CreateDate))
                                                        .Get()
                                                        .ToList();
            }
            else
            {
                return _unitOfWork.Repository<Bookmark>().Query()
                                                            .Filter(b => (b.Category != null && b.Category.Name == category) && b.UserCreatedId == userId)
                                                            .Get()
                                                            .ToList();
            }
        }

        public void UpdateBookmark(Bookmark bookmark)
        {
            var bookmarkToUpdate = _unitOfWork.Repository<Bookmark>().FindById(bookmark.ID);
            if (bookmarkToUpdate != null)
            {
                bookmarkToUpdate.ShortDescription = bookmark.ShortDescription;
                bookmarkToUpdate.URL = bookmark.URL;

                _unitOfWork.Repository<Bookmark>().Update(bookmarkToUpdate);
                _unitOfWork.Save();
            }
        }

        public Bookmark GetBookmark(int Id)
        {
            return _unitOfWork.Repository<Bookmark>().Query()
                                                    .Filter(c => c.ID == Id)
                                                    .Get()
                                                    .FirstOrDefault();
        }

        public void DeleteBookmark(Bookmark bookmark)
        {
            _unitOfWork.Repository<Bookmark>().Delete(bookmark);
            _unitOfWork.Save();
        }

        public Bookmark BookmarkClicked(int id)
        {
            var bookmark = _unitOfWork.Repository<Bookmark>().FindById(id);
            ++bookmark.ClickCount;
            _unitOfWork.Repository<Bookmark>().Update(bookmark);
            _unitOfWork.Save();

            return bookmark;
        }
    }
}
