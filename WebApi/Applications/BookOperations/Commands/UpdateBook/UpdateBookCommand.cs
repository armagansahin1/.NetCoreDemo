using System;
using System.Linq;
using AutoMapper;
using WebApi.DbOperations;

namespace WebApi.Applications.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommand
    {
        private readonly IBookStoreDbContext _dbContext;
        public int Id { get; set; }
        public UpdateBookModel Model { get; set; }

        public UpdateBookCommand(IBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
  
        }

        public void Handle()
        {
            var bookToUpdate = _dbContext.Books.SingleOrDefault(b => b.Id == Id);
            if (bookToUpdate == null)
            {
                throw new InvalidOperationException("Kitap Bulunamadı");
            }

            bookToUpdate.GenreId = Model.GenreId != default ? Model.GenreId : bookToUpdate.GenreId;

            bookToUpdate.Title = Model.Title != default ? Model.Title : bookToUpdate.Title;
            
            _dbContext.SaveChanges();


        }

        public class UpdateBookModel
        {
            public string Title { get; set; }
            public int GenreId { get; set; }
        }
    }
}