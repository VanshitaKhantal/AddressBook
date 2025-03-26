﻿using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Interface for Address Book Service defining CRUD operations.
    /// </summary>
    public interface IAddressBookService
    {
        List<Contact> GetAllContacts();
        Contact GetContactById(int id);
        Contact AddContact(Contact contact);
        bool UpdateContact(int id, Contact contact);
        bool DeleteContact(int id);
    }
}
