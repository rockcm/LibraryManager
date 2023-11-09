namespace Lab6.Services
{
    using Lab6.Pages;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LibraryService
    {
        public Dictionary<int, Book> Books { get; } = new Dictionary<int, Book>();
        public Dictionary<int, User> Users { get; } = new Dictionary<int, User>();
        public Dictionary<int, Dictionary<int, Book>> BorrowedBooks { get; } = new Dictionary<int, Dictionary<int, Book>>();

        public void AddBook(Book book)
        {
            int newId = Books.Any() ? Books.Keys.Max() + 1 : 1;
            book.Id = newId;
            Books[newId] = book;
        }

        public void DeleteBook(int bookId)
        {
            if (Books.ContainsKey(bookId))
            {
                Books.Remove(bookId);
            }
        }

        public void AddUser(User user)
        {
            int newId = Users.Any() ? Users.Keys.Max() + 1 : 1;
            user.Id = newId;
            Users[newId] = user;
        }

        public void DeleteUser(int userId)
        {
            if (Users.ContainsKey(userId))
            {
                Users.Remove(userId);
            }
        }

        public bool BorrowBook(int bookId, int userId)
        {
            // Check if the book with the given bookId exists.
            if (Books.TryGetValue(bookId, out Book book))
            {
                // Check if the user with the given userId exists.
                if (Users.TryGetValue(userId, out User user))
                {
                    // Check if the user has borrowed books.
                    if (!BorrowedBooks.ContainsKey(user.Id))
                    {
                        BorrowedBooks[user.Id] = new Dictionary<int, Book>();
                    }

                    // Check if there are available copies of the book.
                    if (Books.Values.Count(b => b.Id == bookId) > 0)
                    {
                        // Remove the book from the main collection and add it to the borrowedBooks.
                        Books.Remove(bookId);
                        BorrowedBooks[user.Id][bookId] = book;
                        return true; // Borrowing was successful.
                    }
                    else
                    {
                        // No available copies of the book.
                        return false;
                    }
                }
            }

            // Book or user not found.
            return false;
        }

        public bool ReturnBook(int userId, int bookId)
        {
            // Check if the user with the given userId exists.
            if (Users.TryGetValue(userId, out User user))
            {
                // Check if the user has borrowed books.
                if (BorrowedBooks.TryGetValue(user.Id, out Dictionary<int, Book> borrowed))
                {
                    // Check if the book with the given bookId exists.
                    if (borrowed.TryGetValue(bookId, out Book bookToReturn))
                    {
                        // Remove the book from the borrowed collection and add it back to the main collection.
                        borrowed.Remove(bookId);
                        Books[bookId] = bookToReturn;
                        return true; // Returning was successful.
                    }
                }
            }

            // User not found or no borrowed books.
            return false;
        }





        public List<Book> GetBorrowedBooks(int userId)
        {
            if (BorrowedBooks.TryGetValue(userId, out Dictionary<int, Book> userBorrowedBooks))
            {
                return userBorrowedBooks.Values.ToList();
            }

            return new List<Book>(); // Return an empty list if the user has no borrowed books or doesn't exist
        }

    }




}
