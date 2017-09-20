using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Sharetop.Wopi.Models;
using System.Text;
using System.IO;

namespace Sharetop.Wopi.Utils
{
    public static class CatalogUtil
    {
        private static DataTable _table = null;
        private static DataTable table()
        {
            if (_table == null)
            {
                _table = new DataTable("Catalog");

                var schemaGenerator = new JsonSchemaGenerator();
                var schema = schemaGenerator.Generate(typeof(DetailedFileModel));

                foreach (var property in schema.Properties)
                {
                    _table.Columns.Add(property.Key, JsonUtil.GetTypeFromSchemaType(property.Value.Type));
                }

                try
                {
                    _table.ReadXml(HttpContext.Current.Server.MapPath("~/App_Data/Catalog.db"));
                }
                catch
                {
                }
            }
            return _table;
        }

        private static void save()
        {
            try
            {
                table().AcceptChanges();
                table().ReadXml(HttpContext.Current.Server.MapPath("~/App_Data/Catalog.db"));
            }
            catch (Exception ex)
            {
                ServerUtil.LogException(ex);
                throw ex;
            }
        }

        public static IEnumerable<DetailedFileModel> GetItems(string Query = "")
        {
            var r = table().Select(Query);
            var l = new List<DetailedFileModel>();
            for (int i = 0; i < r.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);

                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartObject();

                    foreach (DataColumn c in table().Columns)
                    {
                        writer.WritePropertyName(c.ColumnName);
                        writer.WriteValue(r[i][c.ColumnName]);
                    }
                    writer.WriteEnd();
                    writer.WriteEndObject();
                }

                DetailedFileModel f = JsonConvert.DeserializeObject<DetailedFileModel>(sb.ToString());

                l.Add(f);
            }

            return l;

            //collectionId = collectionToQuery;
            //return Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
            //    .Where(predicate)
            //    .AsEnumerable();
        }

        public static DetailedFileModel GetItem(string Query = "")
        {
            var l = GetItems(Query);
            try
            {
                return l.First();
            }
            catch
            {
                return null;
            }

            //collectionId = collectionToQuery;
            //return Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
            //            .Where(predicate)
            //            .AsEnumerable()
            //            .FirstOrDefault();
        }

        public static void CreateItem(DetailedFileModel item)
        {
            var json = JsonConvert.SerializeObject(item);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, Object>>(json);
            var r = table().NewRow();

            foreach (var kv in dictionary)
            {
                r[kv.Key] = kv.Value;
            }

            r.AcceptChanges();
            table().Rows.Add(r);
            save();
        }

        public static void DeleteItemAsync(string id)
        {
            Document doc = GetDocument(collectionToQuery, id);
            return await Client.DeleteDocumentAsync(doc.SelfLink);
        }
    }
}