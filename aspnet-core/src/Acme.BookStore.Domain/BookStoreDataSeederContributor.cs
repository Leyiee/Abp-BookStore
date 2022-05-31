using System;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore
{
    public class BookStoreDataSeederContributor
        : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly AuthorManager _authorManager;

        public BookStoreDataSeederContributor(
            IRepository<Book, Guid> bookRepository,
            IAuthorRepository authorRepository,
            AuthorManager authorManager)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _authorManager = authorManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _bookRepository.GetCountAsync() > 0)
            {
                return;
            }

            var ghian = await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "Ghian Carlos Rabino Tan",
                    new DateTime(2000, 10, 18),
                    "Ghian is currently at school taking Bachelor of Science in Computer Science, he's improving his skills to become a good game developer someday!"
                )
            );

            var fingertips = await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "Fingertips",
                    new DateTime(1952, 03, 11),
                    "Hey everyone, it's me Fingertips! I am currently developing a game entitled with StickFight: Stick Em' Up"
                )
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    AuthorId = ghian.Id, // SET THE AUTHOR
                    Name = "Fingertips",
                    Type = BookType.Fantastic,
                    PublishDate = new DateTime(2022, 5, 28),
                    Price = 69.69f
                },
                autoSave: true
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    AuthorId = fingertips.Id, // SET THE AUTHOR
                    Name = "The Secrets of CloverField",
                    Type = BookType.ScienceFiction,
                    PublishDate = new DateTime(2000, 10, 18),
                    Price = 21.5f
                },
                autoSave: true
            );
        }
    }
}
