
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Sharetop.Wopi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Sharetop.Wopi.Utils
{
    public static class DatabaseUtil<T> where T:mongoDocument
    {
        private static IMongoCollection<T> GetCollection(string Name)
        {
            var client = new MongoClient(ServerUtil.Config().ConnectionString());
            return client.GetDatabase(ServerUtil.Config().DatabaseName).GetCollection<T>(Name);
        }

        //private static void save()
        //{
        //    try
        //    {
        //        Database().Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        ServerUtil.LogException(ex);
        //        throw ex;
        //    }
        //}

        public static IEnumerable<T> GetItems(string collectionToQuery, Expression<Func<T, bool>> predicate)
        {
            var items = GetCollection(collectionToQuery).AsQueryable<T>().Where(predicate);
            return items;
        }

        public static T GetItem(string collectionToQuery, Expression<Func<T, bool>> predicate)
        {
            try
            {
                return GetItems(collectionToQuery, predicate).FirstOrDefault();
            }
            catch
            {
                return default(T);
            }

            //collectionId = collectionToQuery;
            //return Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
            //            .Where(predicate)
            //            .AsEnumerable()
            //            .FirstOrDefault();
        }

        public static void CreateItem(string collectionToQuery, T item)
        {
            GetCollection(collectionToQuery).InsertOne(item);
        }

        internal static void DeleteItem(string collectionToQuery, T item)
        {
            GetCollection(collectionToQuery).DeleteOne(i => i.documentId == item.documentId);
        }

        internal static void DeleteItems(string collectionToQuery, Expression<Func<T, bool>> predicate)
        {
            GetCollection(collectionToQuery).DeleteMany(predicate);
        }

        public static void UpdateItem(string collectionToQuery, T item)
        {
            GetCollection(collectionToQuery).ReplaceOne(i => i.documentId == item.documentId, item);
        }
    }
}