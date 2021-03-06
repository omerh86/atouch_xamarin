﻿using System;
using LinphoneXamarin.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Linphone;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace LinphoneXamarin.Services
{
    public sealed class ContactService
    {

        public List<Contact> allContacts = new List<Contact>();
        private FavListener favListener;
        private static ContactService instance = null;
        private static readonly object padlock = new object();

        ContactService()
        {
            //allContacts.Add(new Contact("1", "Roi", "2007", ContactType.Aeonix));
            //allContacts.Add(new Contact("2", "Yuval", "2008", ContactType.Aeonix));

            phoneContactsAsync();
        }

        public static ContactService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ContactService();
                    }
                    return instance;
                }
            }
        }

        public void updateContact(Contact contact)
        {
            Contact result = allContacts.Find(delegate (Contact c)
            {

                return c.userName.Equals(contact.userName);
            });
            if (result == null)
                allContacts.Add(contact);
            else
            {
                result = contact;
            }

        }

        public Contact getContactByUserName(String userName)
        {

            return null;
        }

        public Contact getContactByAlias(string alias)
        {
            Contact result = allContacts.Find(i => i.primaryAlias.Equals(alias));
            return result;
        }

        public bool toggleContactFavStatus(string userName)
        {
            Contact relevantContact = allContacts.Find(i => i.userName.Equals(userName));
            if (relevantContact != null)
            {
                relevantContact.isFav = !relevantContact.isFav;
                if (favListener != null)
                {
                    favListener.onFavListChanged();
                }
                return relevantContact.isFav;
            }
            return false;
        }

        public List<Contact> getContactByPartialName(string name)
        {
            List<Contact> result = allContacts.FindAll(delegate (Contact c)
              {
                  string s = c.primaryAlias.ToString() + c.displayName + c.userName;
                  return s.ToLower().IndexOf(name.ToLower()) > -1;
              });
            if (result == null)
                result = new List<Contact>();

            return result;

        }

        public List<Contact> getAllFav()
        {
            List<Contact> result = allContacts.FindAll(i => i.isFav);
            return result;
        }

        public void setFavListener(FavListener favListener)
        {
            this.favListener = favListener;
        }


        private async Task phoneContactsAsync()
        {
            try
            {
                var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
                Console.WriteLine("omer40: " + contacts);
                foreach (var contact in contacts)
                {
                    this.allContacts.Add(new Contact(contact.Name, contact.Name, contact.Number, ContactType.PhoneBook));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("omer40: " + e);
            }
        }





    }
}