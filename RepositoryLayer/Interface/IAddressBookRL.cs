using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Interface for the Address Book Repository Layer.
    /// Defines methods for managing contact data in the database.
    /// </summary>
    public interface IAddressBookRL
    {
        List<Contact> GetAllContacts();
        Contact GetContactById(int id);
        Contact AddContact(Contact contact);
        bool UpdateContact(int id, Contact contact);
        bool DeleteContact(int id);
    }
}
